using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public int maxHearts = 5; // Each heart are 2 points of life
    public int health;        // Actual life (in half hearts)

    public event Action OnHealthChanged;

    void Start()
    {
        health = maxHearts * 2;

        // notify anyone who is subscribed
        OnHealthChanged ?.Invoke();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        health = Mathf.Max(0, health);
        OnHealthChanged?.Invoke();

        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        health += amount;
        health = Mathf.Min(health, maxHearts * 2);
        OnHealthChanged?.Invoke();
    }

    void Die()
    {
        Debug.Log("Player died");
        // Show game over or reset the scene
    }
}