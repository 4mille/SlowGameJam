using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [Header("Transition")]
    [Tooltip("Image noire plein écran pour le fondu (alpha 0 au départ)")]
    public Image fadeImage;
    public float fadeDuration = 1.5f;

    private bool cinematicPlayed = false;

    private void Start()
    {
        if (uiPanel != null)
            uiPanel.SetActive(false);

        // Si on arrive depuis une autre scène, on fade-in
        if (fadeImage != null)
            StartCoroutine(Fade(1f, 0f, fadeDuration));
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

        // Déplacer la caméra via ton controller existant (si nécessaire)
        cameraController.StartCinematic(
            cinematicTarget.position,
            cinematicTarget.rotation,
            1f / moveDuration
        );

        // Attente de la fin du mouvement de caméra
        yield return new WaitForSeconds(moveDuration + holdDuration);

        // Fondu vers noir puis chargement de la scène des crédits
        if (!string.IsNullOrEmpty(creditsSceneName) && fadeImage != null)
        {
            yield return StartCoroutine(Fade(0f, 1f, fadeDuration));
            SceneManager.LoadScene(creditsSceneName);
        }
        else if (!string.IsNullOrEmpty(creditsSceneName))
        {
            SceneManager.LoadScene(creditsSceneName);
        }
    }

    /// <summary>
    /// Fondu d'alpha entre start et end (0=transparent, 1=noir).
    /// </summary>
    private IEnumerator Fade(float start, float end, float duration)
    {
        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(start, end, elapsed / duration);
            fadeImage.color = new Color(c.r, c.g, c.b, t);
            yield return null;
        }

        fadeImage.color = new Color(c.r, c.g, c.b, end);
    }
}
