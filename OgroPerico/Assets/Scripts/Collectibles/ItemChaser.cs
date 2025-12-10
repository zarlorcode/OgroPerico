using UnityEngine;
using System.Collections;

public class ItemChaser : MonoBehaviour
{
    private Transform target;
    private bool isChasing = false;
    
    // Configuración del movimiento
    private float moveSpeed = 6f;       // Velocidad inicial
    private float acceleration = 8f;    // Cuánto acelera por segundo
    private float popForce = 1.5f;      // El "saltito" inicial al salir del cofre

    public void StartChasing(Transform playerTransform)
    {
        target = playerTransform;
        StartCoroutine(PopAndChaseRoutine());
    }

    private IEnumerator PopAndChaseRoutine()
    {
        // FASE 1: El "POP" (Saltito hacia arriba)
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        // Destino del saltito (un poco arriba y aleatorio a los lados)
        Vector3 popDestination = startPos + Vector3.up * 0.8f + (Vector3)(Random.insideUnitCircle * 0.5f);

        while (elapsed < 0.5f) // El salto dura medio segundo
        {
            // Lerp suave hacia la posición del salto
            transform.position = Vector3.Lerp(transform.position, popDestination, elapsed * 3f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Espera un instante muy breve para que el jugador vea qué objeto es
        yield return new WaitForSeconds(0.2f);

        // FASE 2: La Persecución
        isChasing = true;
    }

    void Update()
    {
        if (isChasing && target != null)
        {
            // Aumentamos la velocidad con el tiempo para que sea imposible escapar
            moveSpeed += acceleration * Time.deltaTime;

            // Movemos el objeto hacia el jugador
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }
}
