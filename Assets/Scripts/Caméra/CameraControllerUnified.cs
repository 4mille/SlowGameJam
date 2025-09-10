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
    public Vector3 meshCameraRotation = new Vector3(30f, -30f, 0f);
    public float meshSmoothSpeed = 5f;

    private bool inMeshMode = false;
    private float targetZ;
    private Vector3 sideScrollerPosition;
    private Quaternion sideScrollerRotation;

    private void Start()
    {
        targetZ = sideScrollerOffset.z;
    }

    private void LateUpdate()
    {
        if (inMeshMode)
        {
            // Mode Mesh fixe
            transform.position = Vector3.Lerp(transform.position, meshCameraPosition, meshSmoothSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(meshCameraRotation), meshSmoothSpeed * Time.deltaTime);
            return;
        }

        // Side-scroller
        float desiredX = player.position.x + sideScrollerOffset.x;
        float desiredY = player.position.y + sideScrollerOffset.y;

        if (useLimits)
        {
            desiredX = Mathf.Clamp(desiredX, xLimits.x, xLimits.y);
            desiredY = Mathf.Clamp(desiredY, yLimits.x, yLimits.y);
        }

        // Appliquer targetZ (zones)
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
        sideScrollerPosition = transform.position;
        sideScrollerRotation = transform.rotation;
        inMeshMode = true;
    }

    public void ExitMeshMode()
    {
        inMeshMode = false;
        // Retour automatique → targetZ reste actif
    }
}
