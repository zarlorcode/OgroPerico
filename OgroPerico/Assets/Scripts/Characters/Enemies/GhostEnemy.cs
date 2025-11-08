using UnityEngine;

public class GhostEnemy : MonoBehaviour
{
    public float moveSpeed = 3f;           // Velocidad del fantasma
    public float chaseRange = 5f;          // Distancia para empezar a perseguir
    public float stopDistance = 0.5f;      // Distancia mínima al jugador

    private Transform player;              // Referencia al jugador
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(player.position, transform.position);

        if (distance <= chaseRange)
        {
            // Calcular dirección hacia el jugador
            Vector2 direction = (player.position - transform.position).normalized;
            movement = direction;

            animator?.SetBool("IsMoving", true);
        }
        else
        {
            movement = Vector2.zero;
            animator?.SetBool("IsMoving", false);
        }

        // Opcional: voltear sprite
        if (movement.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (movement.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void FixedUpdate()
    {
        if (movement != Vector2.zero)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // Para visualizar el rango de persecución en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
