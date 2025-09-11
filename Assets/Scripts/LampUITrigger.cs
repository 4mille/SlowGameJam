using UnityEngine;

public class LampUITrigger : MonoBehaviour
{
    [Header("UI")]
    [Tooltip("L'UI à afficher quand le joueur entre dans la zone.")]
    public GameObject uiPanel;   // Assigne ton panel ou objet UI ici

    [Header("Player")]
    [Tooltip("Référence directe du GameObject Player.")]
    public GameObject playerObject;   // Glisse ton Player ici

    private void Start()
    {
        if (uiPanel != null)
            uiPanel.SetActive(false); // S’assure qu’il est invisible au départ
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerObject != null && other.gameObject == playerObject)
        {
            if (uiPanel != null)
                uiPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerObject != null && other.gameObject == playerObject)
        {
            if (uiPanel != null)
                uiPanel.SetActive(false);
        }
    }
}
