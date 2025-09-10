using UnityEngine;

public class InactiveUIHandler : MonoBehaviour
{
    [Header("UI")]
    public GameObject uiInactive; // Texte genre "Pas encore activé"

    [Header("Référence générateur (si nécessaire)")]
    public GeneratorInteract generator;

    [Header("Type d'objet")]
    public bool needsGenerator = true; // Ex: leviers attendent le générateur
    public bool isPile = false;        // Ex: pile doit être portée par le joueur

    private bool playerInRange = false;

    private void Start()
    {
        if (uiInactive != null)
            uiInactive.SetActive(false);
    }

    private void Update()
    {
        if (!playerInRange || uiInactive == null)
            return;

        bool showUI = false;

        // Si l'objet dépend du générateur
        if (needsGenerator && generator != null && !generator.IsPowered)
            showUI = true;

        // Si c'est une pile mais elle n'est pas portée → inactif
        if (isPile)
        {
            var pile = GetComponent<InteractablePile>();
            if (pile != null && !pile.IsCarried && !pile.IsDeposited)
                showUI = true;
        }

        uiInactive.SetActive(showUI);
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
            if (uiInactive != null)
                uiInactive.SetActive(false);
        }
    }
}
