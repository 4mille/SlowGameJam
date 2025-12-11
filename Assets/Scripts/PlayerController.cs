using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;   // <-- glisser ton Animator dans l'inspecteur

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

        // Détection du mouvement horizontal POUR L’ANIMATION (Marche)
        bool mooving =
            Input.GetKey(KeyCode.Q) ||
            Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow);

        if (animator != null)
            animator.SetBool("Mooving", mooving);

        // Mouvement normal ou échelle
        if (isOnLadder)
            HandleLadder();
        else
            NormalMovement();
    }

    private void NormalMovement()
    {
        // Ne pas autoriser le mouvement normal quand on est en train d'escalader
        if (isClimbing) return;

        float moveX = 0f;

        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
            moveX = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            moveX = 1f;

        Vector3 move = new Vector3(moveX, 0f, 0f).normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + move);

        // Rotation selon direction (quand au sol / hors échelle)
        if (moveX > 0f)
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);   // regarde droite
        else if (moveX < 0f)
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);  // regarde gauche

        // Saut
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;

            if (animator != null)
                animator.SetBool("Jumping", true);
        }
    }

    private void HandleLadder()
    {
        float vertical = 0f;
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow))
            vertical = 1f;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            vertical = -1f;

        if (Mathf.Abs(vertical) > 0.01f)
        {
            if (!isClimbing)
            {
                // On entre en mode escalade
                isClimbing = true;
                rb.linearVelocity = Vector3.zero;
                rb.useGravity = false;

                // ORIENTER LE JOUEUR VERS L'ECHELLE
                if (currentLadder != null)
                {
                    Vector3 dirToLadder = currentLadder.transform.position - transform.position;
                    dirToLadder.y = 0f; // on ne veut pas incliner vers le haut/bas
                    if (dirToLadder.sqrMagnitude > 0.0001f)
                    {
                        transform.rotation = Quaternion.LookRotation(dirToLadder);
                    }
                }
            }

            // Animation montée activée
            if (animator != null)
                animator.SetBool("Climbing", true);

            Vector3 climb = Vector3.up * vertical * climbSpeed * Time.deltaTime;
            rb.MovePosition(transform.position + climb);
        }
        else
        {
            // Pas d'entrée verticale : rester sur place (si on est encore dans le collider)
            if (isClimbing)
                rb.linearVelocity = Vector3.zero;

            // On coupe l'animation de montée si immobile sur l'échelle
            if (animator != null)
                animator.SetBool("Climbing", false);
        }

        // Autoriser mouvement horizontal pendant l'escalade (pour sortir latéralement)
        float moveX = 0f;
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
            moveX = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            moveX = 1f;

        if (Mathf.Abs(moveX) > 0.01f)
        {
            Vector3 horizontal = new Vector3(moveX, 0f, 0f).normalized * speed * Time.deltaTime;
            rb.MovePosition(transform.position + horizontal);

            // IMPORTANT : pendant l'escalade on garde l'orientation vers l'échelle
            // (donc on ne flippe pas horizontalement ici)
        }

        // Autoriser sortie libre par le haut :
        if (currentLadder != null)
        {
            float topY = currentLadder.bounds.max.y;
            // Si le joueur dépasse légèrement le sommet, on repasse en mouvement normal
            if (transform.position.y > topY + 0.1f)
            {
                ExitLadder();
            }
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

        // Stop animation échelle
        if (animator != null)
            animator.SetBool("Climbing", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;

                if (animator != null)
                    animator.SetBool("Jumping", false);

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