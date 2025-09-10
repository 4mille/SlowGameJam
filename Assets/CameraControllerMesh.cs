using UnityEngine;

public class CameraControllerMesh : MonoBehaviour
{
    [Header("Player")]
    public Transform player; // player suivi en side-scroller

    [Header("Side-scroller offset")]
    public Vector3 sideScrollerOffset = new Vector3(0f, 2f, -6f);

    [Header("Mode Mesh")]
    public Vector3 meshCameraPosition; // position exacte pour le mode Mesh
    public bool inMeshMode = false;

    private void LateUpdate()
    {
        if (inMeshMode)
        {
            // Déplace la caméra exactement à la position définie dans l'inspector
            transform.position = meshCameraPosition;
            transform.rotation = Quaternion.Euler(30f, -30f, 0f); // rotation fixe pour le mode mesh
        }
        else
        {
            // Suivi normal du player en side-scroller
            transform.position = player.position + sideScrollerOffset;
            transform.rotation = Quaternion.Euler(10f, 0f, 0f);
        }
    }

    // Appelé par ton MeshController
    public void EnterMeshMode()
    {
        inMeshMode = true;
    }

    public void ExitMeshMode()
    {
        inMeshMode = false;
    }
}
