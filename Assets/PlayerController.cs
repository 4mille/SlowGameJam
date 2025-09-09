using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Mouvement")]
    public float vitesse = 5f;

    [Header("Saut")]
    public float forceSaut = 5f;
    private bool auSol;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Déplacement gauche/droite
        float inputX = 0f;

        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
            inputX = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            inputX = 1f;

        Vector3 velocity = rb.linearVelocity;
        velocity.x = inputX * vitesse; 
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, rb.linearVelocity.z);

        // Saut
        if (Input.GetKeyDown(KeyCode.Space) && auSol)
        {
            rb.AddForce(Vector3.up * forceSaut, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Vérifie si on touche le sol
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f) // sol assez plat
            {
                auSol = true;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Si on n’est plus en contact, on n’est plus au sol
        auSol = false;
    }
}
