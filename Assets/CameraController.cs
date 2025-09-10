using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Offsets side-scroller")]
    public Vector3 offset = new Vector3(0f, 2f, -6f);

    [Header("Smooth")]
    public float smoothSpeed = 5f;

    [Header("Mesh Mode")]
    public Vector3 meshCameraPosition; // position de la caméra en mode mesh
    public bool inMeshMode = false;
    public float meshSmoothSpeed = 5f;

    private Vector3 targetPosition;

    private void LateUpdate()
    {
        if (inMeshMode)
        {
            // Smooth vers la position définie pour le mode Mesh
            targetPosition = meshCameraPosition;
            transform.position = Vector3.Lerp(transform.position, targetPosition, meshSmoothSpeed * Time.deltaTime);
            // Optionnel : rotation fixe ou définie par toi
            transform.rotation = Quaternion.Euler(30f, -30f, 0f); 
        }
        else
        {
            // Side-scroller classique
            targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(10f, 0f, 0f); // rotation side-scroller
        }
    }

    // Appelé depuis ton Mesh Controller
    public void EnterMeshMode(Vector3 meshCamPos)
    {
        meshCameraPosition = meshCamPos;
        inMeshMode = true;
    }

    public void ExitMeshMode()
    {
        inMeshMode = false;
    }
}
