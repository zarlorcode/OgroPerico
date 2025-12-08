using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input")]
    public JoystickController joystickController;

    [Header("Movement")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 inputDirection;
    private Animator animator;
    private bool facingRight = true;
    private bool joystickActive = false;

    [Header("Knockback")]
    private Vector2 knockbackVelocity = Vector2.zero;
    private float knockbackTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (joystickController == null)
        {
            // Busca en la escena cualquier objeto que tenga el script JoystickController
            joystickController = FindObjectOfType<JoystickController>(); 
        }
    }

    void Update()
    {
        if (joystickController != null && joystickController.InputDirection != Vector2.zero)
        {
            SetInputDirection(joystickController.InputDirection);
        } else
        {
            inputDirection.x = Input.GetAxisRaw("Horizontal");
            inputDirection.y = Input.GetAxisRaw("Vertical");
            inputDirection = inputDirection.normalized;
        }

        // only if joystick is not working
        /*if (!joystickActive)
        {
            inputDirection.x = Input.GetAxisRaw("Horizontal");
            inputDirection.y = Input.GetAxisRaw("Vertical");
            inputDirection = inputDirection.normalized;
        }*/

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
        Vector2 finalMovement = inputDirection * moveSpeed;

        // Aplica knockback si hay
        if (knockbackTimer > 0f)
        {
            finalMovement = knockbackVelocity;
            knockbackTimer -= Time.fixedDeltaTime;
        }


        rb.MovePosition(rb.position + finalMovement * Time.fixedDeltaTime);
    }

    // This method is for applying mobile joystick or something like that
    public void SetInputDirection(Vector2 direction)
    {
        inputDirection = direction;
        joystickActive = direction != Vector2.zero;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void ApplyKnockback(Vector2 direction)
    {
        knockbackTimer = 0.2f;
        knockbackVelocity = direction.normalized * 7f;
    }
}
