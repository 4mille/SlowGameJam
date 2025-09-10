using UnityEngine;
using UnityEngine.UI;

public class InteractablePile : MonoBehaviour
{
    [Header("UI")]
    public GameObject uiPressE;

    [Header("Material")]
    public Material carriedMaterial;
    private Material originalMaterial;

    private bool playerInRange = false;
    private Transform playerTransform;
    private Renderer rend;

    private bool isCarried = false;
    private bool isDeposited = false;

    public bool IsCarried => isCarried;
    public bool IsDeposited => isDeposited;

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
        if (playerInRange && uiPressE.activeSelf && Input.GetKeyDown(KeyCode.E) && !isCarried && !isDeposited)
        {
            // La pile devient enfant du joueur
            transform.SetParent(playerTransform);
            transform.localPosition = new Vector3(0f, 1f, 1f);
            transform.localRotation = Quaternion.identity;

            // Change le matériau
            if (rend != null && carriedMaterial != null)
                rend.material = carriedMaterial;

            isCarried = true;

            if (uiPressE != null)
                uiPressE.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isCarried && !isDeposited)
        {
            playerInRange = true;
            playerTransform = other.transform;

            if (uiPressE != null)
                uiPressE.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isCarried && !isDeposited)
        {
            playerInRange = false;
            playerTransform = null;

            if (uiPressE != null)
                uiPressE.SetActive(false);
        }
    }

    public void MarkAsDeposited()
    {
        isCarried = false;
        isDeposited = true;
        transform.SetParent(null);
    }
}
