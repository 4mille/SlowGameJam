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

    [Header("Caméra (optionnel)")]
    public Transform meshCameraPoint;
    public Vector3 meshCameraRotation = new Vector3(30f, -30f, 0f);

    private bool isControlling = false;
    private bool isLocked = false; // verrouillage du mesh, pas du mode controller
    private Transform meshTransform;
    private PlayerController playerController;
    private CameraControllerUnified camController;

    public bool IsControlling => isControlling;

    private void Start()
    {
        if (Camera.main != null)
            camController = Camera.main.GetComponent<CameraControllerUnified>();
    }

    private void Update()
    {
        if (!isControlling) return;

        // Échap fonctionne toujours
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ForceStopControl();
            return;
        }

        // Si mesh verrouillé, on ignore juste le mouvement
        if (meshTransform == null || isLocked) return;

        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow)) moveX = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moveX = 1f;

        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow)) moveY = 1f;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) moveY = -1f;

        Vector3 move = new Vector3(moveX, moveY, 0f) * moveSpeed * Time.deltaTime;
        meshTransform.position += move;

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
        isLocked = false;

        if (camController != null && meshCameraPoint != null)
        {
            camController.meshCameraPosition = meshCameraPoint.position;
            camController.meshCameraRotation = meshCameraPoint.eulerAngles;
            camController.EnterMeshMode();
        }

        Debug.Log($"MeshController ({name}) : contrôle du mesh activé.");
    }

    public void ForceStopControl()
    {
        if (!isControlling) return;

        isControlling = false;

        if (playerController != null) playerController.canMove = true;

        if (camController != null)
            camController.ExitMeshMode();

        Debug.Log($"MeshController ({name}) : contrôle arrêté.");
    }

    // Verrouille juste le mesh, pas le contrôle global
    public void LockMesh()
    {
        isLocked = true;
        Debug.Log($"MeshController ({name}) : mesh verrouillé.");
    }

    public bool ControlsTransform(Transform t) => meshTransform == t;
}
