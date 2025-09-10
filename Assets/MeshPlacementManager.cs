using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPlacementManager : MonoBehaviour
{
    [Header("Meshes")]
    public int maxMeshes = 3;           // Nombre total de meshes à placer

    [Header("Caméra et cinématique")]
    public CameraControllerUnified cameraController; // Assigner la caméra
    public Transform cinematicTarget;    // Point vers lequel la caméra se déplace
    public float moveDuration = 2f;      // Temps pour atteindre le point
    public float holdDuration = 2f;      // Temps de pause
    public Transform playerTransform;    // Transform du player à bloquer

    private PlayerController playerController;
    private List<bool> meshPlaced;

    private void Start()
    {
        meshPlaced = new List<bool>();
        for (int i = 0; i < maxMeshes; i++)
            meshPlaced.Add(false);

        if (playerTransform != null)
            playerController = playerTransform.GetComponent<PlayerController>();
    }

    // Appelée par chaque mesh snapé
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
            {
                StartCoroutine(CinematicCoroutine());
            }
        }
    }

    private bool AllMeshesPlaced()
    {
        foreach (bool placed in meshPlaced)
        {
            if (!placed) return false;
        }
        return true;
    }

    private IEnumerator CinematicCoroutine()
    {
        Rigidbody playerRb = null;
        bool originalKinematic = false;

        // Bloque complètement le joueur
        if (playerController != null)
        {
            playerRb = playerController.GetComponent<Rigidbody>();
            originalKinematic = playerRb.isKinematic;

            // Désactive le script et met le Rigidbody en kinematic
            playerController.enabled = false;
            playerRb.isKinematic = true;
        }

        Vector3 originalPos = cameraController.transform.position;
        Quaternion originalRot = cameraController.transform.rotation;

        // Déplace la caméra vers le point cible
        cameraController.StartCinematic(cinematicTarget.position, cinematicTarget.rotation, moveDuration);

        // Attente du déplacement + holdDuration
        yield return new WaitForSeconds(moveDuration + holdDuration);

        // Retour caméra vers sa position initiale
        cameraController.StartCinematic(originalPos, originalRot, moveDuration);

        yield return new WaitForSeconds(moveDuration);

        // Fin de la cinématique
        cameraController.EndCinematic();

        // Restaure le joueur
        if (playerController != null)
        {
            playerRb.isKinematic = originalKinematic;
            playerController.enabled = true;
        }
    }
}
