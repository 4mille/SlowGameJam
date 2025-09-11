using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Mouvement")]
    public float speed = 5f;
    public float jumpForce = 5f;

    [Header("Echelle")]
    public float climbSpeed = 3f;   // vitesse de montée/descente

    [HideInInspector]
    public bool canMove = true;

    private Rigidbody rb;
    private bool isGrounded = true;

    // --- Variables échelle ---
    private bool isOnLadder = false;
    private bool isClimbing = false;
    private Collider currentLadder = null;
    // -------------------------

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!canMove) return;

        // Si on est dans la zone d'une échelle
        if (isOnLadder)
        {
            HandleLadder();
        }
        else
        {
            NormalMovement();
        }
    }

    private void NormalMovement()
    {
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

    private void HandleLadder()
    {
        float vertical = 0f;
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow))
            vertical = 1f;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            vertical = -1f;

        // Passage du mode normal au mode escalade
        if (Mathf.Abs(vertical) > 0.01f)
        {
            if (!isClimbing)
            {
                isClimbing = true;
                rb.linearVelocity = Vector3.zero;
                rb.useGravity = false;
            }

            Vector3 climb = Vector3.up * vertical * climbSpeed * Time.deltaTime;
            rb.MovePosition(transform.position + climb);
        }
        else
        {
            // Pas de touche : rester sur place (si on est encore dans le collider)
            if (isClimbing)
                rb.linearVelocity = Vector3.zero;
        }

        // Autoriser la sortie libre par le haut :
        if (currentLadder != null)
        {
            float topY = currentLadder.bounds.max.y;
            // Si le joueur dépasse légèrement le sommet, on repasse en mouvement normal
            if (transform.position.y > topY + 0.1f)
            {
                ExitLadder();
            }
        }

        // Autoriser mouvement horizontal pendant l'escalade (pour sortir latéralement)
        float moveX = 0f;
        float moveZ = 0f;
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
            moveX = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            moveX = 1f;

        if (Mathf.Abs(moveX) > 0.01f || Mathf.Abs(moveZ) > 0.01f)
        {
            Vector3 horizontal = new Vector3(moveX, 0f, moveZ).normalized * speed * Time.deltaTime;
            rb.MovePosition(transform.position + horizontal);
        }
    }

    private void ExitLadder()
    {
        isOnLadder = false;
        isClimbing = false;
        currentLadder = null;
        rb.useGravity = true;
        // On considère le joueur "au sol" quand il sort par le haut
        isGrounded = true;
    }

    private void OnCollisionEnter(Collision collision)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Echelle") || other.gameObject.layer == LayerMask.NameToLayer("Echelle"))
        {
            isOnLadder = true;
            currentLadder = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Echelle") || other.gameObject.layer == LayerMask.NameToLayer("Echelle"))
        {
            ExitLadder();
        }
    }

    public void LockPlayer()
    {
        canMove = false;
        this.enabled = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
    }

    public void UnlockPlayer()
    {
        rb.isKinematic = false;
        this.enabled = true;
        canMove = true;
    }
}
