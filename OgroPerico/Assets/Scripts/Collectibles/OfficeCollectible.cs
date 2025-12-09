using UnityEngine;

public class OfficeCollectible : MonoBehaviour
{
    public enum CollectibleType { TupperDeMama, CafeDeMaquina }
    
    [Header("Configuración del Item")]
    public CollectibleType tipoDeObjeto;
    
    [Header("Valores")]
    public int corazonesExtra = 1;       // Para el Tupper
    public float porcentajeVelocidad = 0.10f; // 10% para el Café

    [Header("Feedback")]
    [SerializeField] private AudioClip sonidoRecoger; 
    // Puedes añadir partículas aquí si quieres

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
                    Debug.Log("¡Café bebido! +Attack Speed");
                }
                break;
        }

        // Reproducir sonido si existe (usando tu AudioManager o un PlayClipAtPoint simple)
        if (sonidoRecoger != null)
        {
            AudioSource.PlayClipAtPoint(sonidoRecoger, transform.position);
        }

        // Destruir el objeto
        Destroy(gameObject);
    }
}
