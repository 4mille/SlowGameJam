using UnityEngine;
using UnityEngine.UI;

public class InteractablePile : MonoBehaviour
{
    [Header("UI")]
    public GameObject uiPressE; // l'UI "Press E"

    [Header("Material")]
    public Material carriedMaterial; // material à appliquer quand portée
    private Material originalMaterial;

    private bool playerInRange = false;

    public bool isCarried = false;
    private Transform playerTransform;
    private Renderer rend;

    private void Start()
    {
        if (uiPressE != null)
            uiPressE.SetActive(false);

        rend = GetComponent<Renderer>();
        if (rend != null)
            originalMaterial = rend.material;
    }

    private void Update()
    {
        if (playerInRange && uiPressE.activeSelf && Input.GetKeyDown(KeyCode.E) && !isCarried)
        {
            // La Pile devient enfant du Player
            transform.SetParent(playerTransform);

            // Positionne la pile devant le player
            transform.localPosition = new Vector3(0f, 1f, 1f); // ajuste selon la taille du player
            transform.localRotation = Quaternion.identity;

            // Change le material
            if (rend != null && carriedMaterial != null)
                rend.material = carriedMaterial;

            isCarried = true;

            // On peut cacher l'UI si tu veux
            if (uiPressE != null)
                uiPressE.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isCarried)
        {
            playerInRange = true;
            playerTransform = other.transform;

            if (uiPressE != null)
                uiPressE.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isCarried)
        {
            playerInRange = false;
            playerTransform = null;

            if (uiPressE != null)
                uiPressE.SetActive(false);
        }
    }
}
