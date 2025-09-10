using UnityEngine;

public class CameraControllerUnified : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Side-scroller offsets")]
    public Vector3 sideScrollerOffset = new Vector3(0f, 2f, -6f);
    public float smoothSpeed = 5f;
    public float zSmoothSpeed = 2f;

    [Header("Limits")]
    public bool useLimits = true;
    public Vector2 xLimits = new Vector2(-10f, 10f);
    public Vector2 yLimits = new Vector2(0f, 5f);

    [Header("Mode Mesh")]
    public Vector3 meshCameraPosition;
    public Vector3 meshCameraRotation = new Vector3(0f, 0f, 0f);
    public float meshSmoothSpeed = 5f;

    private bool inMeshMode = false;

    // **Cinematic override**
    private bool isCinematicActive = false;
    private Vector3 cinematicTargetPosition;
    private Quaternion cinematicTargetRotation;
    private float cinematicSpeed;

    private float targetZ;

    private void Start()
    {
        targetZ = sideScrollerOffset.z;
    }

    private void LateUpdate()
    {
        if (isCinematicActive)
        {
            // Pendant la cinématique, on ignore tout le reste
            transform.position = Vector3.Lerp(transform.position, cinematicTargetPosition, cinematicSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, cinematicTargetRotation, cinematicSpeed * Time.deltaTime);
            return;
        }

        if (inMeshMode)
        {
            transform.position = Vector3.Lerp(transform.position, meshCameraPosition, meshSmoothSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(meshCameraRotation), meshSmoothSpeed * Time.deltaTime);
            return;
        }

        // Side-scroller normal
        float desiredX = player.position.x + sideScrollerOffset.x;
        float desiredY = player.position.y + sideScrollerOffset.y;

        if (useLimits)
        {
            desiredX = Mathf.Clamp(desiredX, xLimits.x, xLimits.y);
            desiredY = Mathf.Clamp(desiredY, yLimits.x, yLimits.y);
        }

        Vector3 newPosition = new Vector3(
            Mathf.Lerp(transform.position.x, desiredX, smoothSpeed * Time.deltaTime),
            Mathf.Lerp(transform.position.y, desiredY, smoothSpeed * Time.deltaTime),
            Mathf.Lerp(transform.position.z, targetZ, zSmoothSpeed * Time.deltaTime)
        );

        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(10f, 0f, 0f);
    }

    public void SetCameraZ(float newZ)
    {
        targetZ = newZ;
    }

    public void EnterMeshMode()
    {
        inMeshMode = true;
    }

    public void ExitMeshMode()
    {
        inMeshMode = false;
    }

    /// <summary>
    /// Lance une cinématique : la caméra se déplace vers un point et ignore les autres logiques.
    /// </summary>
    public void StartCinematic(Vector3 targetPos, Quaternion targetRot, float speed)
    {
        cinematicTargetPosition = targetPos;
        cinematicTargetRotation = targetRot;
        cinematicSpeed = speed;
        isCinematicActive = true;
    }

    /// <summary>
    /// Termine la cinématique et reprend le contrôle normal.
    /// </summary>
    public void EndCinematic()
    {
        isCinematicActive = false;
    }
}
