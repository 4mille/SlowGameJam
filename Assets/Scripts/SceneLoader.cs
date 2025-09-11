using UnityEngine;
using UnityEngine.SceneManagement; // nécessaire pour gérer les scènes

public class SceneLoader : MonoBehaviour
{
    // Nom de la scène à charger
    public string sceneName;

    // Cette fonction peut être liée à un bouton
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log("Clic !");
    }
}
