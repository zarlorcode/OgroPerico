using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class EnemyBase : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 3f;
    public float chaseRange = 5f;
    public float stopDistance = 0.5f;

    [Header("Movimiento Aleatorio")]
    public float wanderRadius = 3f;
    public float wanderSpeed = 1.5f;
    protected Vector2 wanderTarget;
    protected Vector2 startPosition;

    [Header("Vida")]
    public int maxHealth = 6;
    protected int currentHealth;

    [Header("Referencias")]
    public Transform player;

    protected Rigidbody2D rb;
    protected Animator animator;

    protected bool isDead = false;
    protected bool stunned = false;

    protected Vector2 movement;
    protected Vector2 knockbackVelocity = Vector2.zero;
    protected float knockbackTimer = 0f;

    protected enum EnemyState { Wandering, Chasing, Attacking }
    protected EnemyState currentState;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        startPosition = rb.position;
        ChooseNewWanderTarget();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected virtual void Update()
    {
        if (isDead || player == null) return;

        HandleState();
        HandleSpriteFlip();
    }

    protected virtual void FixedUpdate()
    {
        HandleKnockback();

        if (!isDead && !stunned)
        {
            float currentSpeed = (currentState == EnemyState.Wandering) ? wanderSpeed :
                                 (currentState == EnemyState.Attacking) ? 0f :
                                 moveSpeed;

            rb.MovePosition(rb.position + movement.normalized * currentSpeed * Time.fixedDeltaTime);
        }
    }

    // ===================== Estados =====================
    protected virtual void HandleState()
    {
        float distance = Vector2.Distance(player.position, rb.position);

        if (distance <= chaseRange && distance > stopDistance)
        {
            currentState = EnemyState.Chasing;
            movement = ((Vector2)player.position - (Vector2)rb.position).normalized;
            animator?.SetBool("IsMoving", true);
        }
        else if (distance <= stopDistance)
        {
            currentState = EnemyState.Attacking;
            movement = Vector2.zero;
            animator?.SetBool("IsMoving", false);
        }
        else
        {
            currentState = EnemyState.Wandering;
            movement = ((wanderTarget - (Vector2)rb.position).normalized) * (wanderSpeed / moveSpeed);
            animator?.SetBool("IsMoving", true);

            if (Vector2.Distance(rb.position, wanderTarget) < 0.1f)
                ChooseNewWanderTarget();
        }
    }

    protected void HandleSpriteFlip()
    {
        if (movement.x > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (movement.x < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    // ===================== Movimiento aleatorio =====================
    protected void ChooseNewWanderTarget()
    {
        Vector2 randomOffset = Random.insideUnitCircle * wanderRadius;
        wanderTarget = startPosition + randomOffset;
    }

    // ===================== Knockback / Stun =====================
    protected void HandleKnockback()
    {
        if (knockbackTimer > 0f)
        {
            movement = knockbackVelocity;
            knockbackTimer -= Time.fixedDeltaTime;
            if (knockbackTimer <= 0f)
            {
                knockbackTimer = 0f;
                Stun(0.3f);
            }
        }
    }

    public void ApplyKnockback(Vector2 direction, float force = 2f, float duration = 0.2f)
    {
        knockbackVelocity = direction.normalized * force;
        knockbackTimer = duration;
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

    // ===================== Vida =====================
    public virtual void TakeDamage(int amount, Vector2 hitSourcePosition)
    {
        if (isDead) return;

        Vector2 hitDirection = ((Vector2)transform.position - hitSourcePosition).normalized;
        ApplyKnockback(hitDirection);

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        animator?.SetTrigger("Die");

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 0.5f);
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}

