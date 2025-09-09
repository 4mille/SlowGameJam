using UnityEngine;

public class MeshController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool isControlling = false;
    private Transform meshTransform;
    private PlayerController playerController;

    public bool IsControlling => isControlling; // Propriété publique pour LeverUI

    private void Update()
    {
        if (!isControlling || meshTransform == null)
            return;

        // Quitter le contrôle
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isControlling = false;

            // Rendre le contrôle au Player
            if (playerController != null)
                playerController.canMove = true;

            Debug.Log("Retour au contrôle normal du Player");
            return;
        }

        // Déplacement sur X et Y
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
            moveX = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            moveX = 1f;

        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow))
            moveY = 1f;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            moveY = -1f;

        Vector3 move = new Vector3(moveX, moveY, 0f) * moveSpeed * Time.deltaTime;
        meshTransform.position += move;
    }

    public void StartControl(Transform mesh, PlayerController player)
    {
        meshTransform = mesh;
        playerController = player;

        // Bloque le Player
        if (playerController != null)
            playerController.canMove = false;

        isControlling = true;
        Debug.Log("Contrôle du mesh activé, Player bloqué");
    }
}
