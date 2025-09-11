using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LampUICinematicTrigger : MonoBehaviour
{
    [Header("UI")]
    public GameObject uiPanel;

    [Header("Player")]
    public GameObject playerObject;

    [Header("Camera")]
    public CameraControllerUnified cameraController;
    public Transform cinematicTarget;
    public float moveDuration = 2f;
    public float holdDuration = 2f;

    [Header("Crédits")]
    public string creditsSceneName;

    private bool cinematicPlayed = false;

    private void Start()
    {
        if (uiPanel != null)
            uiPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!cinematicPlayed && other.gameObject == playerObject)
        {
            if (uiPanel != null)
                uiPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerObject)
        {
            if (uiPanel != null)
                uiPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (!cinematicPlayed && uiPanel.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PlayCinematic());
        }
    }

    private IEnumerator PlayCinematic()
    {
        cinematicPlayed = true;

        if (uiPanel != null)
            uiPanel.SetActive(false);

        // Bloquer le joueur
        PlayerController pc = playerObject.GetComponent<PlayerController>();
        if (pc != null)
            pc.LockPlayer();

        // Activer la cinématique sur le CameraControllerUnified
        cameraController.StartCinematic(cinematicTarget.position, cinematicTarget.rotation, 1f / moveDuration); // vitesse inversée pour Lerp

        // Attendre que la caméra atteigne sa position
        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Pause finale
        yield return new WaitForSeconds(holdDuration);

        // Charger la scène de crédits
        if (!string.IsNullOrEmpty(creditsSceneName))
        {
            SceneManager.LoadScene(creditsSceneName);
        }
    }
}
