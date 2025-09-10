using UnityEngine;

public class InteractLever : MonoBehaviour
{
    [Header("UI")]
    public GameObject uiPressE;

    [Header("Rotation du levier")]
    public float rotationAngle = -45f;
    private Quaternion initialRotation;

    [Header("Lampe à contrôler")]
    public GameObject lampe;
    private Light lampeLight;

    [Header("Référence générateur")]
    public GeneratorInteract generator;

    private bool playerInRange = false;
    private bool isRotated = false;

    public bool IsRotated => isRotated;

    private void Start()
    {
        if (uiPressE != null)
            uiPressE.SetActive(false);

        initialRotation = transform.rotation;

        if (lampe != null)
        {
            lampeLight = lampe.GetComponent<Light>();
            if (lampeLight != null)
                lampeLight.enabled = false; // 🔴 Éteinte par défaut
        }

        LeverManager.Instance.RegisterLever(this);
    }

    private void Update()
    {
        if (!playerInRange || !uiPressE.activeSelf || !Input.GetKeyDown(KeyCode.E))
            return;

        if (generator == null || !generator.IsPowered)
            return;

        if (!isRotated)
            LeverManager.Instance.SetActiveLever(this);
        else
            LeverManager.Instance.ClearActiveLever(this);
    }

    public void Activate()
    {
        transform.rotation = initialRotation * Quaternion.Euler(0f, 0f, rotationAngle);
        isRotated = true;

        if (lampeLight != null)
        {
            lampeLight.enabled = true;
            lampeLight.color = Color.green; // 🟢 Active
        }
    }

    public void Deactivate()
    {
        transform.rotation = initialRotation;
        isRotated = false;

        if (lampeLight != null)
        {
            lampeLight.enabled = true;
            lampeLight.color = Color.red; // 🔴 Inactif mais allumé
        }
    }

    // ⚡ Appelé par le générateur quand il est activé
    public void OnGeneratorPowered()
    {
        if (lampeLight != null)
        {
            lampeLight.enabled = true;
            lampeLight.color = Color.red;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInRange = true;

            if (uiPressE != null && generator != null && generator.IsPowered)
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
