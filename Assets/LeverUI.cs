using UnityEngine;
using UnityEngine.UI;

public class LeverUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject uiPressE; // UI "Press E"
    public GameObject uiPressB; // UI "Press B" pour contrôle du mesh

    [Header("Mesh à contrôler")]
    public Transform meshToControl;
    public MeshController meshController;

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

        bool anotherLeverActive = LeverManager.Instance != null && LeverManager.Instance.HasActiveLever();

        // UI Press E : seulement si levier précédent actif et pas en contrôle du mesh
        if (!meshController.IsControlling)
        {
            if (uiPressE != null)
                uiPressE.SetActive(anotherLeverActive);
            if (uiPressB != null)
                uiPressB.SetActive(false);
        }

        // Appui sur E pour contrôler le mesh
        if (anotherLeverActive && Input.GetKeyDown(KeyCode.E) && meshController != null && meshToControl != null && !meshController.IsControlling)
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
        if (meshController.IsControlling == false)
        {
            if (uiPressB != null)
                uiPressB.SetActive(false);

            if (uiPressE != null && anotherLeverActive)
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
