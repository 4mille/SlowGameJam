using UnityEngine;
using UnityEngine.UI;

public class GeneratorInteract : MonoBehaviour
{
    [Header("UI")]
    public GameObject uiPressE; // UI à afficher quand la pile est proche

    [Header("Matérial")]
    public Material depositedMaterial; // matérial du générateur quand la pile est déposée

    private InteractablePile pileInTrigger = null;
    private Renderer generatorRenderer;

    private void Start()
    {
        if (uiPressE != null)
            uiPressE.SetActive(false);

        generatorRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        // Si la pile est dans le trigger et UI active, on peut déposer
        if (pileInTrigger != null && uiPressE.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            // Dépose la pile
            pileInTrigger.transform.SetParent(null);
            pileInTrigger.transform.position = transform.position + Vector3.up * 0.5f; // ajuster selon la hauteur
            pileInTrigger.isCarried = false;

            // Change le matérial du générateur
            if (generatorRenderer != null && depositedMaterial != null)
                generatorRenderer.material = depositedMaterial;

            // Cache l'UI
            if (uiPressE != null)
                uiPressE.SetActive(false);

            pileInTrigger = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractablePile pile = other.GetComponent<InteractablePile>();
        if (pile != null && pile.isCarried)
        {
            pileInTrigger = pile;

            if (uiPressE != null)
                uiPressE.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractablePile pile = other.GetComponent<InteractablePile>();
        if (pile != null)
        {
            if (pileInTrigger == pile)
                pileInTrigger = null;

            if (uiPressE != null)
                uiPressE.SetActive(false);
        }
    }
}
