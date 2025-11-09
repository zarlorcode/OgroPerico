using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class GhostEnemy : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 3f;           // Velocidad del fantasma
    public float chaseRange = 5f;          // Distancia para empezar a perseguir
    public float stopDistance = 0.5f;      // Distancia minima al jugador

    [Header("Ataque")]
    public int damageToPlayer = 2;         // Dano que causa al jugador
    public float damageCooldown = 1f;      // Tiempo entre ataques

    [Header("Vida del Enemigo")]
    public int maxHealth = 6;              // Vida maxima del enemigo
    private int currentHealth;             // Vida actual

    [Header("Referencias")]
    public Transform player;               // Asignar desde el editor (opcional)

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private PlayerHealth playerHealth;
    private float lastDamageTime;
    private bool isDead = false;

    private bool stunned = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        // Si no esta asignado manualmente, lo busca por tag
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
        if (!isDead && movement != Vector2.zero && !stunned)
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
                playerHealth.TakeDamage(damageToPlayer, transform.position);
                lastDamageTime = Time.time;
                Stun(0.5f);
            }
        }
    }

    // ?? NEW METHOD: receive damage
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

    // ?? NEW METHOD: Die
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Ghost died!");

        // Si tienes animaci�n de muerte:
        animator?.SetTrigger("Die");

        // Desactivar colisiones para que no siga golpeando al jugador
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // Destruir el enemigo despues de un pequeno retraso (para permitir animacion)
        Destroy(gameObject, 0.5f);
    }

    // Visualizacion del rango de persecucion en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

    public void Stun(float duration)
    {
        if (!stunned)
            StartCoroutine(ApplyStun(duration));
    }

    private IEnumerator ApplyStun(float duration)
    {
        stunned = true;              
        rb.linearVelocity = Vector2.zero; 
        yield return new WaitForSeconds(duration);
        stunned = false;         
    }
}
