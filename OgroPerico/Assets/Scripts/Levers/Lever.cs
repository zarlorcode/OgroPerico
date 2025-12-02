using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    // --- Variables existentes ---
    [SerializeField] private bool isActivated = false;
    [SerializeField] public float interactionRadius = 2f; 
    [SerializeField] private LayerMask playerLayer; 
    public UnityEvent OnLeverActivated;
    private Transform playerTransform; 
    
    // --- NUEVAS VARIABLES para cambiar el Tile/Sprite ---
    [Header("Visuales de la Palanca")]
    [SerializeField] private Sprite inactiveSprite; // El Sprite cuando la palanca está en reposo (Tile inactivo)
    [SerializeField] private Sprite activatedSprite; // El Sprite cuando la palanca está activa (Tile activo)
    
    // Referencia al componente que dibuja el sprite
    private SpriteRenderer spriteRenderer; 

    private void Awake()
    {
        // Obtener el SpriteRenderer al inicio. Debe estar en el mismo GameObject.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Asegurarse de que el sprite inicial sea el inactivo
        if (spriteRenderer != null && inactiveSprite != null)
        {
            spriteRenderer.sprite = inactiveSprite;
        }
        else
        {
            Debug.LogError("Falta el SpriteRenderer o el Inactive Sprite en " + gameObject.name);
        }
    }

    private void Start()
    {
        // Lógica existente para encontrar al jugador...
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player no encontrado. Asegúrate de que tu objeto Player tenga la etiqueta 'Player'.");
        }
    }
    
    public void TryActivate()
    {
        if (isActivated)
        {
            return; 
        }

        if (IsPlayerInRange())
        {
            ActivateLever();
        }
        else
        {
            Debug.Log("Jugador fuera de rango para interactuar con " + gameObject.name);
        }
    }
    
    private bool IsPlayerInRange()
    {
        if (playerTransform == null) return false;
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        return distance <= interactionRadius;
    }

    private void ActivateLever()
    {
        isActivated = true;
        
        // --- CAMBIO CLAVE: Cambiar el Sprite al activo ---
        if (spriteRenderer != null && activatedSprite != null)
        {
            spriteRenderer.sprite = activatedSprite;
        }
        
        OnLeverActivated.Invoke();
        
        Debug.Log(gameObject.name + " ha sido activada.");
    }
    
    // Opcional: OnDrawGizmosSelected...
}