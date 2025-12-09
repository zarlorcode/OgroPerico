using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthUI2 : MonoBehaviour
{
    public PlayerHealth playerHealth;
    
    [Header("Referencias UI")]
    public GameObject heartPrefab;  // El "molde" del corazón que creaste
    public Transform heartContainer; // El objeto padre (HeartsUI) que tiene el Layout Group
    
    // Usamos una Lista en vez de un Array porque las listas pueden crecer
    private List<Image> hearts = new List<Image>(); 

    [Header("Sprites")]
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    void Start()
    {
        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
        }

        // 1. Llenamos la lista con los corazones que YA existen en la escena (Heart1, Heart2...)
        // para no duplicarlos si ya los pusiste a mano.
        foreach (Transform child in heartContainer)
        {
            Image img = child.GetComponent<Image>();
            if (img != null)
            {
                hearts.Add(img);
            }
        }

        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHearts;
            UpdateHearts();
        }
    }

    void UpdateHearts()
    {
        if (playerHealth == null) return;

        // --- PARTE NUEVA: Gestión dinámica de corazones ---
        
        // Calculamos cuántos corazones deberíamos tener en total
        int targetHeartCount = playerHealth.maxHearts;

        // Si tenemos menos corazones visuales de los que deberíamos...
        while (hearts.Count < targetHeartCount)
        {
            // ...creamos uno nuevo
            GameObject newHeartObj = Instantiate(heartPrefab, heartContainer);
            
            // Le ponemos nombre ordenado (opcional)
            newHeartObj.name = "Heart" + (hearts.Count + 1);
            
            // Lo añadimos a nuestra lista para controlarlo
            hearts.Add(newHeartObj.GetComponent<Image>());
        }
        
        // (Opcional) Si quisieras reducir corazones, aquí iría un while (hearts.Count > targetHeartCount)

        // --------------------------------------------------

        // El bucle de dibujo se mantiene casi igual, pero usando la Lista
        int currentHealth = playerHealth.health;
        
        for (int i = 0; i < hearts.Count; i++)
        {
            // Lógica para saber si este corazón está lleno, a medias o vacío
            int heartStatus = Mathf.Clamp(currentHealth - (i * 2), 0, 2);
        
            if (heartStatus == 2)
                hearts[i].sprite = fullHeart;
            else if (heartStatus == 1)
                hearts[i].sprite = halfHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }
}
