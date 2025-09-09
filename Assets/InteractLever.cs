using UnityEngine;
using UnityEngine.UI;

public class InteractLever : MonoBehaviour
{
    [Header("UI")]
    public GameObject uiPressE; // ton élément UI "Press E"

    [Header("Rotation du levier")]
    public float rotationAngle = -45f; 
    private Quaternion initialRotation;

    [Header("Lampe à contrôler")]
    public GameObject lampe; 
    private Light lampeLight;
    private Color couleurInitiale;

    private bool playerInRange = false;
    private bool isRotated = false;

    private void Start()
    {
        if (uiPressE != null)
            uiPressE.SetActive(false);

        initialRotation = transform.rotation;

        if (lampe != null)
        {
            lampeLight = lampe.GetComponent<Light>();
            if (lampeLight != null)
                couleurInitiale = lampeLight.color;
        }

        // S’enregistre dans le manager
        LeverManager.Instance.RegisterLever(this);
    }

    private void Update()
    {
        if (playerInRange && uiPressE.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            if (!isRotated)
            {
                // Active ce levier
                LeverManager.Instance.SetActiveLever(this);
            }
            else
            {
                // Désactive ce levier
                LeverManager.Instance.ClearActiveLever(this);
            }
        }
    }

    public void Activate()
    {
        transform.rotation = initialRotation * Quaternion.Euler(0f, 0f, rotationAngle);
        isRotated = true;

        if (lampeLight != null)
            lampeLight.color = Color.green;
    }

    public void Deactivate()
    {
        transform.rotation = initialRotation;
        isRotated = false;

        if (lampeLight != null)
            lampeLight.color = couleurInitiale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInRange = true;
            if (uiPressE != null)
                uiPressE.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInRange = false;
            if (uiPressE != null)
                uiPressE.SetActive(false);
        }
    }
}
