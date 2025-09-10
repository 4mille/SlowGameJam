using UnityEngine;

public class CameraSideScrollerZones : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Offsets")]
    public Vector3 offset = new Vector3(0f, 2f, -6f);

    [Header("Smooth")]
    public float smoothSpeed = 5f;
    public float zSmoothSpeed = 2f;

    [Header("Limits")]
    public bool useLimits = true;
    public Vector2 xLimits = new Vector2(-10f, 10f);
    public Vector2 yLimits = new Vector2(0f, 5f);

    private float targetZ;

    private void Start()
    {
        targetZ = offset.z; // Z initial
    }

    private void LateUpdate()
    {
        if (target == null) return;

        float desiredX = target.position.x + offset.x;
        float desiredY = target.position.y + offset.y;

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

    // Méthode publique pour changer Z depuis n'importe quel script
    public void SetCameraZ(float newZ)
    {
        targetZ = newZ;
    }
}
