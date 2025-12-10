using UnityEngine;
using UnityEngine.UI;
using TMPro; // Usaremos TextMeshPro para mejor calidad de texto

public class CollectibleInfoUI : MonoBehaviour
{
    public static CollectibleInfoUI Instance;

    [Header("Elementos de UI")]
    public GameObject panelCompleto; // El objeto padre que activaremos/desactivaremos
    public Image imagenObjeto;
    public TextMeshProUGUI textoNombre;
    public TextMeshProUGUI textoDescripcion;

    [Header("Audio (Opcional)")]
    public AudioClip sonidoPopup;
    private AudioSource audioSource;

    void Awake()
    {
        // Configurar Singleton simple para acceder desde cualquier lado
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Asegurarnos de tener AudioSource si queremos sonido
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        // Empezamos con el panel oculto
        panelCompleto.SetActive(false);
    }

    public void MostrarInformacion(string nombre, string descripcion, Sprite icono)
    {
        // 1. Asignar los datos a la UI
        textoNombre.text = nombre;
        textoDescripcion.text = descripcion;
        imagenObjeto.sprite = icono;
        // Preservar la proporción de la imagen (aspect ratio)
        imagenObjeto.preserveAspect = true;

        // 2. Activar el panel
        panelCompleto.SetActive(true);

        // 3. PAUSAR EL JUEGO COMPLETAMENTE
        Time.timeScale = 0f;

        // 4. Sonido
        if (sonidoPopup != null) audioSource.PlayOneShot(sonidoPopup);
    }

    // Este método lo llamaremos desde un botón invisible que ocupa toda la pantalla
    public void CerrarPopup()
    {
        // 1. Ocultar panel
        panelCompleto.SetActive(false);

        // 2. REANUDAR EL JUEGO
        Time.timeScale = 1f;
    }
}