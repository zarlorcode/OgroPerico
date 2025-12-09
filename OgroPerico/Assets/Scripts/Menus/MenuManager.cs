using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
 

    void Start()
    {
       
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public void startMenuClicked() 
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.reproducirMusicaIntroDungeon();
        }

        ResetearDatosJuego();

        SceneManager.LoadScene("IntroDungeon");
    }

    public void restartMenuClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.reproducirMusicaJuego();
        }

        ResetearDatosJuego();

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

    public void menuClickedFromMenu()
    {
       
        SceneManager.LoadScene("MainMenu");
    }

    public void backToMenuClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.reproducirMusicaInicio();
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetearDatosJuego()
    {
        DatosDeJuego.VidaJugador = -1;
        DatosDeJuego.PosicionInicialJugador = null;
        
        // Reseteamos a los valores iniciales de tu juego
        DatosDeJuego.MaxCorazones = 5;
        DatosDeJuego.VelocidadMovimiento = 5f;
        DatosDeJuego.MultiplicadorAtaque = 1f;
    }
}
