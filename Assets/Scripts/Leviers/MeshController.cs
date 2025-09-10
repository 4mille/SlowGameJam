using UnityEngine;

public class MeshController : MonoBehaviour
{
    [Header("Déplacement")]
    public float moveSpeed = 5f;

    [Header("Limites de déplacement")]
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = 0f;
    public float maxY = 5f;

    [Header("Caméra")]
    public Vector3 meshCameraPosition;
    public Vector3 meshCameraRotation = new Vector3(30f, -30f, 0f);
    private CameraControllerUnified camController;

    private bool isControlling = false;
    private Transform meshTransform;
    private PlayerController playerController;

    public bool IsControlling => isControlling;

    private void Start()
    {
        camController = Camera.main.GetComponent<CameraControllerUnified>();
        if (camController == null)
            Debug.LogWarning("CameraControllerUnified non trouvé sur Main Camera !");
    }

    private void Update()
    {
        if (!isControlling || meshTransform == null)
            return;

        // Quitter le contrôle avec Échap
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isControlling = false;

            if (playerController != null)
                playerController.canMove = true;

            // Retour caméra au mode side-scroller
            if (camController != null)
                camController.ExitMeshMode();

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

        // Appliquer les limites avec Clamp
        Vector3 clampedPos = meshTransform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);
        clampedPos.y = Mathf.Clamp(clampedPos.y, minY, maxY);
        meshTransform.position = clampedPos;
    }

    public void StartControl(Transform mesh, PlayerController player)
    {
        meshTransform = mesh;
        playerController = player;

        if (playerController != null)
            playerController.canMove = false;

        isControlling = true;

        // Active la caméra en mode Mesh
        if (camController != null)
        {
            camController.meshCameraPosition = meshCameraPosition;
            camController.meshCameraRotation = meshCameraRotation;
            camController.EnterMeshMode();
        }

        Debug.Log("Contrôle du mesh activé, Player bloqué, caméra en mode Mesh");
    }
}
