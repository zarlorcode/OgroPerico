using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class EnemyBase : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 3f;
    public float chaseRange = 5f;
    public float stopDistance = 0.5f;

    [Header("Movimiento Aleatorio")]
    public float wanderSpeed = 1.5f;
    public float wanderInterval = 2f;

    protected Vector2 wanderTarget;
    protected float nextWanderTime = 0f;

    [Header("Vida")]
    public int maxHealth = 6;
    protected int currentHealth;

    [Header("blink")]
    [SerializeField] private float blinkTime = 1.5f;


    [Header("Referencias")]
    public Transform player;
    private SpriteRenderer spriteRenderer;

    // Área donde el enemigo puede moverse
    public BoxCollider2D movementArea;

    protected Rigidbody2D rb;
    protected Animator animator;

    protected bool isDead = false;
    protected bool stunned = false;

    protected Vector2 movement;

    [Header("Knockback")]
    protected Vector2 knockbackVelocity = Vector2.zero;
    protected float knockbackTimer = 0f;    // time of knoback remaining
    public float knockbackDuration = 0.3f;     // knoback time

    protected enum EnemyState { Wandering, Chasing, Attacking }
    protected EnemyState currentState;

    public event Action OnDeath;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Si el spawner asigna el área, perfecto.
        // Si no, intenta tomarla del padre.
        if (movementArea == null)
            movementArea = GetComponentInParent<BoxCollider2D>();

        ChooseNewWanderTarget();
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
            float currentSpeed =
                (currentState == EnemyState.Wandering) ? wanderSpeed :
                (currentState == EnemyState.Attacking) ? 0f :
                moveSpeed;

            MoveInsideArea(movement.normalized * currentSpeed);
        }
    }

    // ===================== ESTADOS =====================
    protected virtual void HandleState()
    {
        float distance = Vector2.Distance(player.position, rb.position);

        if (distance <= chaseRange && distance > stopDistance)
        {
            currentState = EnemyState.Chasing;
            movement = ((Vector2)player.position - rb.position).normalized;
        }
        else if (distance <= stopDistance)
        {
            currentState = EnemyState.Attacking;
            movement = Vector2.zero;
        }
        else
        {
            currentState = EnemyState.Wandering;
            HandleWandering();
        }
    }

    // ===================== WANDER DENTRO DEL ÁREA =====================
    protected void HandleWandering()
    {
        if (Time.time >= nextWanderTime)
        {
            ChooseNewWanderTarget();
            nextWanderTime = Time.time + wanderInterval;
        }

        movement = (wanderTarget - rb.position).normalized;

        if (Vector2.Distance(rb.position, wanderTarget) < 0.2f)
            ChooseNewWanderTarget();
    }

    protected void ChooseNewWanderTarget()
    {
        if (movementArea == null)
        {
            wanderTarget = rb.position;
            return;
        }

        Bounds b = movementArea.bounds;

        float x = UnityEngine.Random.Range(b.min.x + 0.5f, b.max.x - 0.5f);
        float y = UnityEngine.Random.Range(b.min.y + 0.5f, b.max.y - 0.5f);

        wanderTarget = new Vector2(x, y);
    }

    // ===================== MOVIMIENTO LIMITADO AL ÁREA =====================
    protected void MoveInsideArea(Vector2 velocity)
    {
        Vector2 newPos = rb.position + velocity * Time.fixedDeltaTime;

        if (movementArea != null)
        {
            Bounds b = movementArea.bounds;

            newPos.x = Mathf.Clamp(newPos.x, b.min.x + 0.3f, b.max.x - 0.3f);
            newPos.y = Mathf.Clamp(newPos.y, b.min.y + 0.3f, b.max.y - 0.3f);
        }

        rb.MovePosition(newPos);
    }

    // ===================== FLIP =====================
    protected virtual void HandleSpriteFlip()
    {
        if (movement.x > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (movement.x < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    // ===================== KNOCKBACK =====================
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

    // ===================== VIDA =====================
    public virtual void TakeDamage(int amount, Vector2 hitSourcePosition)
    {
        if (isDead) return;
        Debug.Log("Enemy received damage");

        Vector2 dir = ((Vector2)transform.position - hitSourcePosition).normalized;
        ApplyKnockback(dir);

        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();

        // ACtivate invulnerability
        StartCoroutine(blink());
    }

    private IEnumerator blink()
    {

        float elapsed = 0f;
        while (elapsed < knockbackDuration)
        {
            // blink
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        spriteRenderer.enabled = true;
    }

    protected virtual void Die()
    {
        if (isDead) return;

        isDead = true;
        OnDeath?.Invoke();

        animator?.SetTrigger("Die");

        Collider2D col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        Destroy(gameObject, 0.5f);
    }

    protected void OnDrawGizmosSelected()
    {
        if (movementArea == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(movementArea.bounds.center, movementArea.bounds.size);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
