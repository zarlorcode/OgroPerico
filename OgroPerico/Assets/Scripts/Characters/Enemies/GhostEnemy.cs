using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GhostEnemy : EnemyBase
{
    [Header("Ataque")]
    public int damageToPlayer = 2;
    public float damageCooldown = 1f;

    [Header("Ataque Especial")]
    public float chargeSpeedMultiplier = 3f;   // Cuánto más rápido ataca
    public float knockbackBackDistance = 0.5f;   // Distancia que retrocede
    

    private float lastDamageTime;
    private PlayerHealth playerHealth;

    protected override void Start()
    {
        base.Start();
        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isDead || playerHealth == null) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                // Retroceso inicial
                Vector2 knockbackDir = ((Vector2)transform.position - (Vector2)player.position).normalized;
                ApplyKnockback(knockbackDir, knockbackBackDistance, knockbackDuration);

                // Daño al jugador después de un pequeño retraso
                StartCoroutine(DelayedChargeAttack(knockbackDuration, knockbackDir));

                lastDamageTime = Time.time;
            }
        }
    }

    private IEnumerator DelayedChargeAttack(float delay, Vector2 knockbackDir)
    {
        yield return new WaitForSeconds(delay);

        // Daño al jugador
        playerHealth.TakeDamage(damageToPlayer, transform.position);

        // Carga hacia el jugador más rápido
        if (!isDead && !stunned)
        {
            Vector2 chargeDir = ((Vector2)player.position - (Vector2)rb.position).normalized;
            movement = chargeDir;

            // Temporarily increase speed
            float originalSpeed = moveSpeed;
            moveSpeed *= chargeSpeedMultiplier;

            // Mantener carga un breve tiempo
            yield return new WaitForSeconds(0.5f);

            moveSpeed = originalSpeed;
        }
    }

}
