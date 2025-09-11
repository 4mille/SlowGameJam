using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Tilt Settings")]
    public float tiltAngle = 10f;
    public float smooth = 5f;

    [Header("Scene Settings")]
    public string sceneName; // nom de la scène à charger

    private Quaternion originalRotation;
    private Quaternion targetRotation;

    void Start()
    {
        originalRotation = transform.rotation;
        targetRotation = originalRotation;
    }

    void Update()
    {
        // Animation de tilt
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smooth);
    }

    // Hover start
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetRotation = Quaternion.Euler(0f, 0f, tiltAngle);
    }

    // Hover end
    public void OnPointerExit(PointerEventData eventData)
    {
        targetRotation = originalRotation;
    }

    // Clic
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Bouton cliqué !");
        SceneManager.LoadScene(sceneName); // charge la scène
    }
}
