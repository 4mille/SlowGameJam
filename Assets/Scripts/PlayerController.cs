using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Mouvement")]
    public float speed = 5f;
    public float jumpForce = 5f;

    [HideInInspector]
    public bool canMove = true; // Bloque le mouvement si false (pendant contrôle du mesh)

    private Rigidbody rb;
    private bool isGrounded = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!canMove) return;

        // Déplacement horizontal (X/Z)
        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
            moveX = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            moveX = 1f;

        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow))
            moveZ = 1f;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            moveZ = -1f;

        Vector3 move = new Vector3(moveX, 0f, moveZ).normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + move);

        // Saut
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Vérifie si on touche le sol pour réactiver le saut
        if (collision.contacts.Length > 0)
        {
            // Simple vérification : si collision par dessous
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    isGrounded = true;
                    break;
                }
            }
        }
    }
}
