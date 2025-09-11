using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverTilt : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float tiltAngle = 10f; // angle de basculement
    public float smooth = 5f;     // vitesse de transition

    private Quaternion originalRotation;
    private Quaternion targetRotation;

    void Start()
    {
        originalRotation = transform.rotation;
        targetRotation = originalRotation;
    }

    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smooth);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetRotation = Quaternion.Euler(0f, 0f, tiltAngle); // penche vers la droite
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetRotation = originalRotation; // revient à la rotation initiale
    }
}
