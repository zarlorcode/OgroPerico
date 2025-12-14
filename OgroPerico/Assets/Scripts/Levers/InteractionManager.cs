using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    // ... tus variables existentes ...
    [SerializeField] private GameObject player; 
    [SerializeField] private float detectionRange = 2f;
    [SerializeField] private LayerMask leverLayer; // Capa donde están tus palancas
    // AÑADIDO: Nueva variable para la capa de las puertas
    [SerializeField] private LayerMask doorLayer; // Capa donde están tus puertas

    [SerializeField] private LayerMask archivadorLayer;
    [SerializeField] private LayerMask puertaLayer;
    [SerializeField] private LayerMask puzzleHintLayer;

    // Este método se conecta al evento OnClick() de tu botón de UI
    public void HandleInteractionButtonPress()
    {
        // 1. Detección de palancas (Tu lógica actual)
        Collider2D[] leverHits = Physics2D.OverlapCircleAll(player.transform.position, detectionRange, leverLayer);

        Lever nearestLever = FindNearestLever(leverHits);

        // 2. Evaluar interacción con la palanca más cercana
        if (nearestLever != null)
        {
            float minLeverDistance = Vector2.Distance(player.transform.position, nearestLever.transform.position);
            
            if (minLeverDistance <= nearestLever.interactionRadius + 0.1f)
            {
                // PRIORIDAD 1: INTERACTUAR con PALANCA.
                nearestLever.TryActivate();
                return; 
            }
        }
        
        
        // 3. Detectar TODOS los colliders de puerta dentro del rango
        Collider2D[] doorHits = Physics2D.OverlapCircleAll(
            player.transform.position, 
            detectionRange, 
            doorLayer
        );

        DoorScript nearestDoor = FindNearestDoor(doorHits);

        // 4. Evaluar interacción con la puerta más cercana
        if (nearestDoor != null)
        {
            float minDoorDistance = Vector2.Distance(player.transform.position, nearestDoor.transform.position);

            if (minDoorDistance <= nearestDoor.interactionRadius + 0.1f)
            {
                // PRIORIDAD 2: INTERACTUAR con PUERTA.
                nearestDoor.TryInteract(); // Llamamos al nuevo método
                return; // Puerta activada, terminamos aquí.
            }
        }

        Collider2D[] archivadorHits = Physics2D.OverlapCircleAll(player.transform.position, detectionRange, archivadorLayer);

        Archivador nearestArchivador = FindNearestArchivador(archivadorHits);

        if (nearestArchivador != null)
        {
            float dist = Vector2.Distance(player.transform.position, nearestArchivador.transform.position);
            
            if (dist <= nearestArchivador.interactionRadius + 0.1f)
            {
                // PRIORIDAD 1: ACTIVAR ARCHIVADOR
                nearestArchivador.TryActivate();
                return; 
            }
        }

        Collider2D[] hintHits = Physics2D.OverlapCircleAll(player.transform.position, detectionRange, puzzleHintLayer);
        PuzzleHintScript nearestHint = FindNearestHint(hintHits);

        if (nearestHint != null)
        {
            float dist = Vector2.Distance(player.transform.position, nearestHint.transform.position);
            // Asumiendo un rango de interacción fijo o añadiendo interactionRadius al script
            if (dist <= nearestHint.interactionRadius + 0.1f) 
            {
                // PRIORIDAD: INTERACTUAR CON PISTA
                nearestHint.TryInteract();
                return;
            }
        }
        
        // 5. Si no hay objetos interactivos cerca, entonces ejecutamos el ataque.
        Debug.Log("Ejecutando la lógica de ATAQUE (ningún objeto interactivo cerca).");
        // Llama a tu función de ataque (por ejemplo):
        // player.GetComponent<PlayerAttack>().ExecuteAttack();
    }
    
    // --- NUEVOS MÉTODOS AUXILIARES para mantener el código limpio ---
    
    // Método auxiliar para encontrar la palanca más cercana (Puedes mover tu lógica anterior aquí)
    private Lever FindNearestLever(Collider2D[] hits)
    {
        Lever nearestLever = null;
        float minDistance = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            Lever currentLever = hit.GetComponent<Lever>();
            if (currentLever != null)
            {
                float distance = Vector2.Distance(player.transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestLever = currentLever;
                }
            }
        }
        return nearestLever;
    }

    // Método auxiliar para encontrar la PUERTA más cercana
    private DoorScript FindNearestDoor(Collider2D[] hits)
    {
        DoorScript nearestDoor = null;
        float minDistance = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            DoorScript currentDoor = hit.GetComponent<DoorScript>();
            if (currentDoor != null)
            {
                float distance = Vector2.Distance(player.transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestDoor = currentDoor;
                }
            }
        }
        return nearestDoor;
    }

    private Archivador FindNearestArchivador(Collider2D[] hits)
    {
        Archivador nearest = null;
        float minDistance = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            Archivador current = hit.GetComponent<Archivador>();
            if (current != null)
            {
                float distance = Vector2.Distance(player.transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = current;
                }
            }
        }
        return nearest;
    }

    private PuzzleHintScript FindNearestHint(Collider2D[] hits)
    {
        PuzzleHintScript nearest = null;
        float minDistance = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            PuzzleHintScript current = hit.GetComponent<PuzzleHintScript>();
            if (current != null)
            {
                float distance = Vector2.Distance(player.transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = current;
                }
            }
        }
        return nearest;
    }
}
