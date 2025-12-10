using UnityEngine;

public class OfficeChest : MonoBehaviour
{
    [Header("Configuración Visual")]
    public Sprite openSprite;
    public Sprite closedSprite; // Opcional si ya lo tienes en el SpriteRenderer
    private SpriteRenderer spriteRenderer;

    [Header("Loot")]
    public GameObject[] collectibles; // Arrastra aquí tus prefabs (Tupper, Café, Ruedas)
    
    [Header("Interacción")]
    public float interactionRadius = 1.5f;
    private bool isOpened = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Aseguramos que empiece cerrado
        if (closedSprite != null) spriteRenderer.sprite = closedSprite;
    }

    public void TryInteract()
    {
        if (isOpened) return; // Si ya está abierto, no hacemos nada

        OpenChest();
    }

    void OpenChest()
    {
        isOpened = true;

        // 1. Cambiar Sprite
        if (openSprite != null)
        {
            spriteRenderer.sprite = openSprite;
        }

        // 2. Elegir premio aleatorio
        if (collectibles.Length > 0)
        {
            int randomIndex = Random.Range(0, collectibles.Length);
            GameObject selectedPrefab = collectibles[randomIndex];

            SpawnItem(selectedPrefab);
        }
        
        // Opcional: Sonido de cofre
        // if (AudioManager.Instance != null) AudioManager.Instance.PlayChestSound();
    }

    void SpawnItem(GameObject itemPrefab)
    {
        // Instanciamos el objeto justo encima del cofre
        Vector3 spawnPos = transform.position + Vector3.up * 0.5f; 
        GameObject item = Instantiate(itemPrefab, spawnPos, Quaternion.identity);

        // Añadimos el script de persecución al objeto recién creado
        ItemChaser chaser = item.AddComponent<ItemChaser>();
        
        // Configuramos para que busque al jugador automáticamente
        // Asumimos que el jugador tiene el tag "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            chaser.StartChasing(player.transform);
        }
    }

    // Para visualizar el radio en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
