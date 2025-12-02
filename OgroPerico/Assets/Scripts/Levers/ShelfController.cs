using UnityEngine;

public class ShelfController : MonoBehaviour
{
    // Arreglo para asignar las 3 palancas desde el Inspector
    public Lever[] allLevers; 

    // Variables para el movimiento
    [SerializeField] private float moveDistance = 5f; // Distancia a mover (hacia la derecha)
    [SerializeField] private float moveDuration = 2f; // Duración del movimiento
    
    private int activatedLeversCount = 0;
    private bool isShelfMoving = false;
    private Vector3 targetPosition;

    private void Start()
    {
        // 1. Inicializar la posición objetivo
        targetPosition = transform.position + Vector3.right * moveDistance;

        // 2. Conectar cada palanca al método CheckActivation en este script
        if (allLevers.Length != 3)
        {
            Debug.LogError("Debes asignar exactamente 3 palancas en el Inspector de ShelfController.");
            return;
        }

        foreach (Lever lever in allLevers)
        {
            // Suscribimos el método CheckActivation al evento de la palanca
            lever.OnLeverActivated.AddListener(CheckActivation);
        }
    }

    private void CheckActivation()
    {
        // Incrementamos el contador y verificamos si ya son 3
        activatedLeversCount++;
        Debug.Log("Palancas activadas: " + activatedLeversCount);

        if (activatedLeversCount >= allLevers.Length && !isShelfMoving)
        {
            // ¡Todas las palancas están activadas! Iniciar el movimiento.
            isShelfMoving = true;
            Debug.Log("¡Todas las palancas activadas! Moviendo la estantería...");
        }
    }

    private void Update()
    {
        // Si la estantería debe moverse, actualizamos su posición cada frame
        if (isShelfMoving)
        {
            MoveShelf();
        }
    }

    private void MoveShelf()
    {
        // Mover la estantería hacia la posición objetivo (targetPosition) usando Lerp
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveDistance / moveDuration * Time.deltaTime);

        // Si hemos llegado al destino, paramos el movimiento
        if (transform.position == targetPosition)
        {
            isShelfMoving = false;
            // Opcional: Desactivar este script para ahorrar rendimiento
            // enabled = false; 
            Debug.Log("La estantería ha terminado de moverse.");
        }
    }
}
