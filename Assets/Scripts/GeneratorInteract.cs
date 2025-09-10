using UnityEngine;

public class GeneratorInteract : MonoBehaviour
{
    [Header("UI")]
    public GameObject uiPressE;

    [Header("Materials")]
    public Material poweredMaterial;
    private Material originalMaterial;
    private Renderer rend;

    private bool playerInRange = false;
    private bool isPowered = false;
    private InteractablePile pileInRange;

    public bool IsPowered => isPowered;

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
        if (playerInRange && uiPressE.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            if (pileInRange != null && pileInRange.IsCarried && !isPowered)
            {
                // Déposer la pile
                pileInRange.transform.SetParent(null);
                pileInRange.MarkAsDeposited();

                // Changer le matériau du générateur
                if (rend != null && poweredMaterial != null)
                    rend.material = poweredMaterial;

                if (uiPressE != null)
                    uiPressE.SetActive(false);

                isPowered = true;

                // ⚡ Allume toutes les lampes en rouge
                foreach (var lever in FindObjectsOfType<InteractLever>())
                {
                    lever.OnGeneratorPowered();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractablePile pile = other.GetComponent<InteractablePile>();
        if (pile != null && pile.IsCarried && !pile.IsDeposited && !isPowered)
        {
            playerInRange = true;
            pileInRange = pile;

            if (uiPressE != null)
                uiPressE.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractablePile pile = other.GetComponent<InteractablePile>();
        if (pile != null && pile == pileInRange)
        {
            playerInRange = false;
            pileInRange = null;

            if (uiPressE != null)
                uiPressE.SetActive(false);
        }
    }
}
