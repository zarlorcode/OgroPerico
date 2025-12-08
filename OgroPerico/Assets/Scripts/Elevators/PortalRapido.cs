using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PortalRapido : MonoBehaviour
{
    [Header("Configuración Visual")]
    [SerializeField] private Sprite texturaActiva; // La textura al chocar
    private Sprite texturaOriginal; // Para guardar la inicial automáticamente
    private SpriteRenderer spriteRenderer;

    [Header("Configuración de Viaje")]
    [SerializeField] private string nombreEscenaDestino;
    [SerializeField] private Vector2 coordenadasDestino; // Dónde aparecerá en la nueva escena

    // Variable estática temporal para recordar a dónde ir entre el cambio de escenas
    private static Vector2 _posicionPendiente;
    private bool _yaActivado = false; // Para evitar dobles activaciones

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        texturaOriginal = spriteRenderer.sprite; // Guarda la textura inicial
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // El PORTAL comprueba si es el Player y si no se ha activado ya
        if (collision.CompareTag("Player") && !_yaActivado)
        {
            StartCoroutine(SecuenciaDeViajeRapida());
        }
    }

    IEnumerator SecuenciaDeViajeRapida()
    {
        _yaActivado = true;

        // 1. Cambiar de textura (Feedback visual inmediato)
        spriteRenderer.sprite = texturaActiva;

        // --- PEQUEÑA PAUSA VISUAL ---
        // Si quitamos esto, el cambio de escena es tan rápido que no verás
        // la texturaActiva. 0.3 segundos es suficiente para un "flash".
        yield return new WaitForSeconds(0.3f);
        // ---------------------------

        // 2. Volver a su textura base
        // Es vital hacerlo ANTES de cargar la escena, porque en la siguiente línea este objeto deja de existir.
        spriteRenderer.sprite = texturaOriginal;

        // 3. Preparar los datos para el transporte
        _posicionPendiente = coordenadasDestino;

        // Nos suscribimos al evento que avisa cuando la nueva escena está lista
        SceneManager.sceneLoaded += MoverPlayerAlTerminarCarga;

        // 4. Transportar (Cargar la nueva escena)
        SceneManager.LoadScene(nombreEscenaDestino);
    }

    // Este método se ejecuta automáticamente cuando Unity termina de cargar la nueva escena
    private static void MoverPlayerAlTerminarCarga(Scene scene, LoadSceneMode mode)
    {
        // Buscamos al Player en la NUEVA escena
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Lo movemos a la posición que guardamos antes
            player.transform.position = _posicionPendiente;
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto con el tag 'Player' en la escena destino.");
        }

        // ¡Importante! Nos desuscribimos del evento para limpiar la memoria
        SceneManager.sceneLoaded -= MoverPlayerAlTerminarCarga;
    }
}
