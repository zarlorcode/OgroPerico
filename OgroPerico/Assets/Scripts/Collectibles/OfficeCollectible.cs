using UnityEngine;

public class OfficeCollectible : MonoBehaviour
{
    public enum CollectibleType { TupperDeMama, CafeDeMaquina }
    
    [Header("Configuración del Item")]
    public CollectibleType tipoDeObjeto;

    [Header("Info para UI")]
    public string nombreItem = "Nombre del Objeto";
    [TextArea(3, 5)] // Esto hace la caja de texto más grande en el editor
    public string descripcionItem = "Descripción del Objeto.";
    private SpriteRenderer spriteRenderer;
    public Sprite iconoUI;
    
    [Header("Valores")]
    public int corazonesExtra = 1;       // Para el Tupper
    public float porcentajeVelocidad = 0.10f; // 10% para el Café

    [Header("Feedback")]
    [SerializeField] private AudioClip sonidoRecoger; 
    // Puedes añadir partículas aquí si quieres

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Si no asignamos icono específico para la UI, usamos el mismo que se ve en el juego
        if (iconoUI == null) iconoUI = spriteRenderer.sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si es el jugador
        if (collision.CompareTag("Player"))
        {
            ApplyEffect(collision.gameObject);
        }
    }

    private void ApplyEffect(GameObject player)
    {
        switch (tipoDeObjeto)
        {
            case CollectibleType.TupperDeMama:
                PlayerHealth health = player.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.IncreaseMaxHealth(corazonesExtra);
                    Debug.Log("¡Tupper recogido! +Max HP");
                }
                break;

            case CollectibleType.CafeDeMaquina:
                PlayerMovement movement = player.GetComponent<PlayerMovement>();
                if (movement != null)
                {
                    movement.IncreaseMoveSpeed(porcentajeVelocidad);
                    Debug.Log("¡Café bebido! +Movement Speed");
                }
                break;
        }

        // Reproducir sonido si existe (usando tu AudioManager o un PlayClipAtPoint simple)
        if (sonidoRecoger != null)
        {
            AudioSource.PlayClipAtPoint(sonidoRecoger, transform.position);
        }

        if (CollectibleInfoUI.Instance != null)
        {
            CollectibleInfoUI.Instance.MostrarInformacion(nombreItem, descripcionItem, iconoUI);
        }
        else
        {
            Debug.LogWarning("No se encontró CollectibleInfoUI en la escena.");
        }

        // Destruir el objeto
        Destroy(gameObject);
    }
}
