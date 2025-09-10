using UnityEngine;
using UnityEngine.Events;

public class MeshTarget : MonoBehaviour
{
    [Header("Target")]
    public Transform targetTransform;
    public Vector3 targetPosition;
    public bool useCurrentPositionAsTarget = false;
    public float tolerance = 0.5f;

    [Header("Snap")]
    public bool smoothSnap = false;
    public float snapSpeed = 10f;
    public float snapFinishThreshold = 0.02f;

    [Header("Après snap")]
    public bool makeKinematic = true;
    public Transform snapParent;
    public UnityEvent onSnapped;

    private bool isAtTarget = false;
    private bool isSnapping = false;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (useCurrentPositionAsTarget && targetTransform == null)
            targetPosition = transform.position;
    }

    private Vector3 CurrentTargetPosition() =>
        targetTransform != null ? targetTransform.position : targetPosition;

    private void Update()
    {
        if (isAtTarget) return;

        float distance = Vector3.Distance(transform.position, CurrentTargetPosition());

        if (!isSnapping && distance <= tolerance)
        {
            if (smoothSnap)
                isSnapping = true;
            else
                SnapImmediate();
        }

        if (isSnapping)
        {
            transform.position = Vector3.Lerp(transform.position, CurrentTargetPosition(), snapSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, CurrentTargetPosition()) <= snapFinishThreshold)
                SnapImmediate();
        }
    }

    private MeshController FindControllerForMesh()
    {
        var controllers = FindObjectsOfType<MeshController>();
        foreach (var mc in controllers)
        {
            if (mc.ControlsTransform(transform))
                return mc;
        }
        return null;
    }

    private void SnapImmediate()
    {
        transform.position = CurrentTargetPosition();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            if (makeKinematic)
                rb.isKinematic = true;
        }

        transform.SetParent(snapParent);

        // Trouver et verrouiller le MeshController
        MeshController mc = FindControllerForMesh();
        if (mc != null)
            mc.LockMesh();

        isAtTarget = true;
        isSnapping = false;

        onSnapped?.Invoke();

        Debug.Log($"{name} : Snap effectué, mesh verrouillé correctement.");
    }

    public bool IsAtTarget() => isAtTarget;

    private void OnDrawGizmosSelected()
    {
        Vector3 pos = targetTransform != null ? targetTransform.position : targetPosition;
        Gizmos.color = isAtTarget ? Color.yellow : Color.green;
        Gizmos.DrawWireSphere(pos, tolerance);
    }

    public void ForceSnapNow() => SnapImmediate();
}
