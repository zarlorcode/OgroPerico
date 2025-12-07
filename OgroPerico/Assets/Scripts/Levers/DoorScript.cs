using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D doorCollider;

    [Header("Sprites")]
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openedSprite;

    [Header("Configuración")]
    // Radio en el que el jugador debe estar para interactuar
    public float interactionRadius = 1.5f; 
    private bool isOpen = false;

    void Start()
    {
        // Asegurarse de que los componentes estén asignados
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (doorCollider == null)
            doorCollider = GetComponent<Collider2D>();

        // Estado inicial de la puerta
        spriteRenderer.sprite = closedSprite;
        doorCollider.enabled = true;
    }

    // Este es el método que llamará el InteractionManager
    public void TryInteract()
    {
        if (!isOpen)
        {
            OpenDoor();
        }
        else
        {
            // Opcional: Si quieres que se pueda cerrar de nuevo
            // CloseDoor();
        }
    }

    private void OpenDoor()
    {
        isOpen = true;
        // Cambia el sprite
        spriteRenderer.sprite = openedSprite; 
        // Desactiva la colisión
        doorCollider.enabled = false; 
        
        Debug.Log("Puerta Abierta. Colision desactivada.");
    }

    private void CloseDoor()
    {
        isOpen = false;
        // Cambia el sprite
        spriteRenderer.sprite = closedSprite; 
        // Activa la colisión
        doorCollider.enabled = true; 
        
        Debug.Log("Puerta Cerrada. Colision activada.");
    }
}
