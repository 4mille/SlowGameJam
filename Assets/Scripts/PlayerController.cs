using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Mouvement")]
    public float speed = 5f;
    public float jumpForce = 5f;

    [HideInInspector]
    public bool canMove = true; // Bloque le mouvement si false

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
        if (collision.contacts.Length > 0)
        {
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

   public void LockPlayer()
{
    canMove = false;
    this.enabled = false; // désactive le script Update
    Rigidbody rb = GetComponent<Rigidbody>();
    rb.linearVelocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;
    rb.isKinematic = true;
}

public void UnlockPlayer()
{
    Rigidbody rb = GetComponent<Rigidbody>();
    rb.isKinematic = false;
    this.enabled = true; // réactive le script Update
    canMove = true;
}
}
