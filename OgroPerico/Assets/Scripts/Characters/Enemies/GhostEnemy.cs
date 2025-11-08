using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class GhostEnemy : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 3f;           // Velocidad del fantasma
    public float chaseRange = 5f;          // Distancia para empezar a perseguir
    public float stopDistance = 0.5f;      // Distancia mínima al jugador

    [Header("Ataque")]
    public int damageToPlayer = 2;         // Daño que causa al jugador
    public float damageCooldown = 1f;      // Tiempo entre ataques

    [Header("Vida del Enemigo")]
    public int maxHealth = 6;              // Vida máxima del enemigo
    private int currentHealth;             // Vida actual

    [Header("Referencias")]
    public Transform player;               // Asignar desde el editor (opcional)

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private PlayerHealth playerHealth;
    private float lastDamageTime;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        // Si no está asignado manualmente, lo busca por tag
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector2.Distance(player.position, transform.position);

        if (distance <= chaseRange && distance > stopDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            movement = direction;
            animator?.SetBool("IsMoving", true);
        }
        else
        {
            movement = Vector2.zero;
            animator?.SetBool("IsMoving", false);
        }

        // Volteo del sprite
        if (movement.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (movement.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void FixedUpdate()
    {
        if (!isDead && movement != Vector2.zero)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isDead) return;

        // Si colisiona con el jugador
        if (collision.gameObject.CompareTag("Player") && playerHealth != null)
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                playerHealth.TakeDamage(damageToPlayer);
                lastDamageTime = Time.time;
            }
        }
    }

    // ?? NUEVO MÉTODO: recibe daño
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"Ghost took {amount} damage. Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ?? NUEVO MÉTODO: morir
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Ghost died!");

        // Si tienes animación de muerte:
        animator?.SetTrigger("Die");

        // Desactivar colisiones para que no siga golpeando al jugador
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // Destruir el enemigo después de un pequeño retraso (para permitir animación)
        Destroy(gameObject, 0.5f);
    }

    // Visualización del rango de persecución en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
