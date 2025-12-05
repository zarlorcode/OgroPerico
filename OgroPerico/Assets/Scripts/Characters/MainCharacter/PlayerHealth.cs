using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHearts = 5; // Each heart are 2 points of life
    public int health;        // Actual life (in half hearts)

    public event Action OnHealthChanged;

    [SerializeField] private float invulnerabilityTime = 1.5f;
    private bool isInvulnerable = false;
    private SpriteRenderer spriteRenderer;

    private PlayerMovement playerMovement;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        health = maxHearts * 2;
        Debug.Log("health: " + health);

        // notify anyone who is subscribed
        OnHealthChanged ?.Invoke();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int amount, Vector2 hitSourcePosition)
    {
        if (isInvulnerable) return;

        health -= amount;
        health = Mathf.Max(0, health);
        OnHealthChanged?.Invoke();

        if (health <= 0)
        {
            Die();
            return;
        }

        Vector2 hitDirection = CalculateHitDirection(hitSourcePosition);


        // Apply recoil
        playerMovement.ApplyKnockback(hitDirection);

        // Activate invulnerability
        StartCoroutine(Invulnerability());
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
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.reproducirMusicaGameOver();
        }
        // Show game over or reset the scene
        SceneManager.LoadScene("GameOver");
    }

    private IEnumerator Invulnerability()
    {
        isInvulnerable = true;

        float elapsed = 0f;
        while (elapsed < invulnerabilityTime)
        {
            // blink
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        spriteRenderer.enabled = true;
        isInvulnerable = false;
    }

    private Vector2 CalculateHitDirection(Vector2 sourcePos)
    {
        return ((Vector2)transform.position - sourcePos).normalized;
    }
}