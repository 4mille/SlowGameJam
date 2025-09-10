using UnityEngine;
using System.Collections;

public class CameraCinematic : MonoBehaviour
{
    public CameraControllerUnified cameraController;
    public Transform targetPosition;
    public float moveDuration = 2f;
    public float holdDuration = 2f;

    private Transform player;
    private PlayerController playerController;

    public void PlayCinematic(Transform playerTransform)
    {
        player = playerTransform;
        playerController = player.GetComponent<PlayerController>();

        if (playerController != null)
            playerController.canMove = false; // bloque le joueur

        StartCoroutine(CinematicCoroutine());
    }

    private IEnumerator CinematicCoroutine()
    {
        Vector3 originalPos = cameraController.transform.position;
        Quaternion originalRot = cameraController.transform.rotation;

        // Déplace caméra vers le point cible
        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
            cameraController.transform.position = Vector3.Lerp(originalPos, targetPosition.position, t);
            cameraController.transform.rotation = Quaternion.Lerp(originalRot, targetPosition.rotation, t);
            yield return null;
        }

        cameraController.transform.position = targetPosition.position;
        cameraController.transform.rotation = targetPosition.rotation;

        // Attente
        yield return new WaitForSeconds(holdDuration);

        // Retour caméra
        elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
            cameraController.transform.position = Vector3.Lerp(targetPosition.position, originalPos, t);
            cameraController.transform.rotation = Quaternion.Lerp(targetPosition.rotation, originalRot, t);
            yield return null;
        }

        cameraController.transform.position = originalPos;
        cameraController.transform.rotation = originalRot;

        // Débloque le joueur
        if (playerController != null)
            playerController.canMove = true;
    }
}
