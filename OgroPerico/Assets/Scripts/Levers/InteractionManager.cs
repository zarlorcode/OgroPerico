using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    // Asigna el Player aquí en el Inspector
    [SerializeField] private GameObject player; 
    [SerializeField] private float detectionRange = 2f;
    [SerializeField] private LayerMask leverLayer; // Capa donde están tus palancas

    // Este método se conecta al evento OnClick() de tu botón de UI
    public void HandleInteractionButtonPress()
    {
        // 1. Detectar TODOS los colliders de palanca dentro del rango
        Collider2D[] hits = Physics2D.OverlapCircleAll(player.transform.position, detectionRange, leverLayer);

        // Variables para encontrar el objeto más cercano
        Lever nearestLever = null;
        float minDistance = float.MaxValue; // Usamos un valor máximo para inicializar la distancia

        // 2. Iterar sobre todos los objetos encontrados para encontrar la palanca más cercana
        foreach (Collider2D hit in hits)
        {
            Lever currentLever = hit.GetComponent<Lever>();
            
            // Verificamos que tenga el script Lever
            if (currentLever != null)
            {
                // Calculamos la distancia
                float distance = Vector2.Distance(player.transform.position, hit.transform.position);

                // Si esta palanca está más cerca que la que habíamos encontrado antes
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestLever = currentLever;
                }
            }
        }

        // 3. Evaluar la interacción con la palanca más cercana
        if (nearestLever != null)
        {
            // Solo intentamos activar la palanca más cercana
            if (minDistance <= nearestLever.interactionRadius + 0.1f) // Usa el radio de la palanca como límite
            {
                // PRIORIDAD 1: INTERACTUAR.
                nearestLever.TryActivate();
                return; // Palanca activada, terminamos aquí.
            }
        }

        // 2. Si no hay palancas cerca, entonces ejecutamos el ataque.
        Debug.Log("Ejecutando la lógica de ATAQUE (ningún objeto interactivo cerca).");
        // Llama a tu función de ataque (por ejemplo):
        // player.GetComponent<PlayerAttack>().ExecuteAttack();
    }
}
