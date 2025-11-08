using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    void Start()
    {
        // execute update when event occurs
        playerHealth.OnHealthChanged += UpdateHearts;
        UpdateHearts();
    }

    void UpdateHearts()
    {
        int health = playerHealth.health;
        for (int i = 0; i < hearts.Length; i++)
        {
            int heartHealth = Mathf.Clamp(health - (i * 2), 0, 2);
            Debug.Log("hearthHealth " + i + ": " + heartHealth);
            if (heartHealth == 2)
                hearts[i].sprite = fullHeart;
            else if (heartHealth == 1)
                hearts[i].sprite = halfHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }
}