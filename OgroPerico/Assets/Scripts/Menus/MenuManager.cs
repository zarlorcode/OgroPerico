using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioManager audioManager;

    void Start()
    {
        // Unity 6 API
        audioManager = FindFirstObjectByType<AudioManager>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public void startMenuClicked() 
    {
        if (audioManager != null)
        {
            audioManager.StopMusic();
        }
        SceneManager.LoadScene("Game");
    }
    public void optionsClicked()
    {
        SceneManager.LoadScene("Options");
    }

    public void creditsClicked()
    {
        SceneManager.LoadScene("Credits");
    }

    public void menuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
