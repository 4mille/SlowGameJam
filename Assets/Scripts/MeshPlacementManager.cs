using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPlacementManager : MonoBehaviour
{
    [Header("Meshes")]
    public int maxMeshes = 3;

    [Header("Caméra et cinématique")]
    public CameraControllerUnified cameraController;
    public Transform cinematicTarget;
    public float moveDuration = 2f;
    public float holdDuration = 2f;
    public Transform playerTransform;

    [Header("Lampe finale")]
    [Tooltip("Objet qui contient la lampe (Light ou GameObject).")]
    public GameObject lampObject;
    public bool lampUsesLightComponent = true;

    [Tooltip("Collider déclenché uniquement quand la lampe s'allume.")]
    public Collider lampTriggerCollider;   // 👉 Collider à activer quand la lampe est allumée

    private PlayerController playerController;
    private List<bool> meshPlaced;

    private void Start()
    {
        meshPlaced = new List<bool>();
        for (int i = 0; i < maxMeshes; i++)
            meshPlaced.Add(false);

        if (playerTransform != null)
            playerController = playerTransform.GetComponent<PlayerController>();

        // Lampe et collider off au départ
        SetLampState(false);
    }

    public void SetMeshPlaced(int index)
    {
        if (index < 0 || index >= maxMeshes) return;

        if (!meshPlaced[index])
        {
            meshPlaced[index] = true;
            Debug.Log($"Mesh index {index} placé !");
        }

        if (AllMeshesPlaced())
        {
            Debug.Log("Tous les meshes sont placés ! Déclenchement cinématique.");
            if (cameraController != null && cinematicTarget != null)
                StartCoroutine(CinematicCoroutine());
        }
    }

    private bool AllMeshesPlaced()
    {
        foreach (bool placed in meshPlaced)
            if (!placed) return false;
        return true;
    }

    private IEnumerator CinematicCoroutine()
    {
        // 🔆 Allume la lampe et active le collider
        SetLampState(true);

        if (playerController != null)
            playerController.LockPlayer();

        Vector3 originalPos = cameraController.transform.position;
        Quaternion originalRot = cameraController.transform.rotation;

        cameraController.StartCinematic(cinematicTarget.position, cinematicTarget.rotation, moveDuration);
        yield return new WaitForSeconds(moveDuration + holdDuration);

        cameraController.StartCinematic(originalPos, originalRot, moveDuration);
        yield return new WaitForSeconds(moveDuration);

        cameraController.EndCinematic();

        if (playerController != null)
            playerController.UnlockPlayer();
    }

    private void SetLampState(bool state)
    {
        if (lampObject != null)
        {
            if (lampUsesLightComponent)
            {
                Light lightComp = lampObject.GetComponent<Light>();
                if (lightComp != null)
                    lightComp.enabled = state;
                else
                    lampObject.SetActive(state);
            }
            else
            {
                lampObject.SetActive(state);
            }
        }

        // 👉 Active ou désactive le collider trigger en même temps
        if (lampTriggerCollider != null)
            lampTriggerCollider.enabled = state;
    }
}
