using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 inputDirection;
    private Animator animator;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // by default, PC keyboard
        inputDirection.x = Input.GetAxisRaw("Horizontal");
        inputDirection.y = Input.GetAxisRaw("Vertical");

        inputDirection = inputDirection.normalized;

        animator.SetFloat("MoveX", inputDirection.x);
        animator.SetFloat("MoveY", inputDirection.y);
        animator.SetBool("IsMoving", inputDirection.sqrMagnitude > 0);

        if (inputDirection.x < 0 && facingRight)
        {
            Flip();
        }
        else if (inputDirection.x > 0 && !facingRight)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + inputDirection * moveSpeed * Time.fixedDeltaTime);
    }

    // This method is for applying mobile joystick or something like that
    public void SetInputDirection(Vector2 direction)
    {
        inputDirection = direction;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
