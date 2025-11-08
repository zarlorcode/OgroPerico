using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashTimer : MonoBehaviour
{
    // Start is called before the first frame update
    const float waitTime = 5.0f;
    float startTime;
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            // Se ha detectado al menos un toque en la pantalla
            // Cambiar a la escena MainMenu
            SceneManager.LoadScene("MainMenu");
        }
        if (Time.time - startTime > waitTime)
        {
            // Ha pasado el tiempo de espera
            // Cambiar a la escena MainMenu
            SceneManager.LoadScene("MainMenu");
        }
    }
}
