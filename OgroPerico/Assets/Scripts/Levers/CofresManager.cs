using UnityEngine;

public class CofresManager : MonoBehaviour
{
    // ... tus variables existentes ...
    [SerializeField] private GameObject player; 
    [SerializeField] private float detectionRange = 2f;
    [SerializeField] private LayerMask chestLayer;

    // Este método se conecta al evento OnClick() de tu botón de UI
    public void HandleChestInteractionButtonPress()
    {
        Collider2D[] chestHits = Physics2D.OverlapCircleAll(player.transform.position, detectionRange, chestLayer);
        OfficeChest nearestChest = FindNearestChest(chestHits); // <--- Método nuevo abajo

        if (nearestChest != null)
        {
            float dist = Vector2.Distance(player.transform.position, nearestChest.transform.position);
            
            // Verificamos distancia
            if (dist <= nearestChest.interactionRadius + 0.1f)
            {
                nearestChest.TryInteract(); // Abrimos el cofre
                return;
            }
        }
        
        // 5. Si no hay objetos interactivos cerca, entonces ejecutamos el ataque.
        Debug.Log("Ejecutando la lógica de ATAQUE (ningún objeto interactivo cerca).");
        // Llama a tu función de ataque (por ejemplo):
        // player.GetComponent<PlayerAttack>().ExecuteAttack();
    }

    private OfficeChest FindNearestChest(Collider2D[] hits)
    {
        OfficeChest nearest = null;
        float minDistance = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            OfficeChest current = hit.GetComponent<OfficeChest>();
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
