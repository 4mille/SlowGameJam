using UnityEngine;

public class ZoneTriggerCamera : MonoBehaviour
{
    public float cameraZ = -6f; // Z désiré pour cette zone

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // important !
        {
            CameraControllerUnified cam = Camera.main.GetComponent<CameraControllerUnified>();
            if (cam != null)
            {
                cam.SetCameraZ(cameraZ);
            }
        }
    }
}
