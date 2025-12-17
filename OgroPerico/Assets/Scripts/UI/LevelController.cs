using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    // Avanza al siguiente nivel en la Build Settings
    public void LoadNivelFinal()
    {
        Debug.Log("Loading Nivel_Final");
        SceneManager.LoadScene("Nivel_final");
    }

    // Reinicia el nivel actual
    public void RestartFirstLevel()
    {
        Debug.Log("Loading Game scene");
        SceneManager.LoadScene("Game");
    }
}
