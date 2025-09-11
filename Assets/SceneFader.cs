using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance;
    private Image fadeImage;

    void Awake()
    {
        if (Instance == null) Instance = this;
        fadeImage = GetComponent<Image>();
    }

    public IEnumerator FadeAndLoad(string sceneName, float duration = 1f)
    {
        // Fondu noir
        yield return StartCoroutine(Fade(0f, 1f, duration));

        // Charger la scène
        SceneManager.LoadScene(sceneName);
    }

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
    }
}
