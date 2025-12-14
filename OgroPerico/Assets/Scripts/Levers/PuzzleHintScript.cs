using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con UI

public class PuzzleHintScript : MonoBehaviour
{
    [SerializeField] public float interactionRadius = 2f; 

    [Header("UI Reference")]
    [SerializeField] private GameObject hintPanel; // Referencia al Panel de la UI

    // Este método será llamado por el InteractionManager
    public void TryInteract()
    {
        ShowHint();
    }

    private void ShowHint()
    {
        hintPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // Este método puede ser llamado por un botón "Cerrar" en la UI o por una tecla
    public void HideHint()
    {
        hintPanel.SetActive(false);
        Time.timeScale = 1f; // Reanuda el juego
        Debug.Log("Panel de pista cerrado mediante clic en el fondo.");
    }
}
