using UnityEngine;
using UnityEngine.UI;

public class LeverUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject uiPressE; // UI "Press E"
    public GameObject uiPressB; // UI "Press B"

    [Header("Mesh à contrôler")]
    public Transform meshToControl;
    public MeshController meshController;

    [Header("Levier requis")]
    public InteractLever requiredLever; // Le levier spécifique qui doit être actif

    private bool playerInRange = false;
    private PlayerController playerController;

    private void Start()
    {
        if (uiPressE != null)
            uiPressE.SetActive(false);
        if (uiPressB != null)
            uiPressB.SetActive(false);
    }

    private void Update()
    {
        if (!playerInRange) return;

        // Vérifie si le levier requis est actif
        bool isRequiredLeverActive = requiredLever != null && requiredLever.IsRotated;

        // UI Press E : seulement si levier requis actif et pas en contrôle du mesh
        if (!meshController.IsControlling)
        {
            if (uiPressE != null)
                uiPressE.SetActive(isRequiredLeverActive);
            if (uiPressB != null)
                uiPressB.SetActive(false);
        }

        // Appui sur E pour contrôler le mesh
        if (isRequiredLeverActive && Input.GetKeyDown(KeyCode.E) && meshController != null && meshToControl != null && !meshController.IsControlling)
        {
            if (playerController == null)
                playerController = FindObjectOfType<PlayerController>();

            if (playerController != null)
                meshController.StartControl(meshToControl, playerController);

            // UI Press B apparaît
            if (uiPressE != null)
                uiPressE.SetActive(false);
            if (uiPressB != null)
                uiPressB.SetActive(true);
        }

        // Vérifie si on a quitté le contrôle du mesh pour réafficher Press E
        if (!meshController.IsControlling)
        {
            if (uiPressB != null)
                uiPressB.SetActive(false);

            if (uiPressE != null && isRequiredLeverActive)
                uiPressE.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInRange = false;
            if (uiPressE != null)
                uiPressE.SetActive(false);
            if (uiPressB != null)
                uiPressB.SetActive(false);
        }
    }
}
