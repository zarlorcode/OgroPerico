using UnityEngine;

public class GhostEnemy : EnemyBase
{
    [Header("Ataque")]
    public int damageToPlayer = 2;
    public float damageCooldown = 1f;

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
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player") && playerHealth != null)
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                playerHealth.TakeDamage(damageToPlayer, transform.position);
                lastDamageTime = Time.time;
                Stun(0.5f);
            }
        }
    }
}
