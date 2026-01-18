using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OgrePericoBoss : BossBase
{
    [Header("Jump attack")]
    public float jumpForce = 12f;
    public float jumpDamageRadius = 4f;
    public int jumpDamage = 4;
    private bool isJumping = false;
    public float jumpSpeed = 8f;
    private Vector2 jumpDirection;

    [Header("Puñetazo al suelo")]
    public float punchRange = 3f;
    public int punchDamage = 2;

    public DialoguePopup dialoguePopup;

    private bool isGrounded = true;

    protected override void Start()
    {
        base.Start();
        Debug.Log("Ogro Perico Boss START");
    }

    protected override void Update()
    {
        base.Update();
        animator.SetBool("IsMoving", movement.sqrMagnitude > 0.01f);
    }

    protected override void HandleBossLogic()
    {
        bool notReadyToAttack = Time.time < nextAttackTime;

        if (isJumping)
        {
            rb.MovePosition(rb.position + jumpDirection * jumpSpeed * Time.fixedDeltaTime);
        }

            if (bossState != BossState.Idle)
        {
            Debug.Log("bossState " + bossState);
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        //Debug.Log("notReadyToAttack " + notReadyToAttack + " distance " + distance);
        if (!notReadyToAttack && distance > 6f && distance < 12f)
            StartCoroutine(JumpAttack());
        else if (distance < 20f)
        {
            movement = ((Vector2)player.position - rb.position).normalized;
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        /*else
            StartCoroutine(PunchAttack());*/
    }

    private IEnumerator JumpAttack()
    {
        Debug.Log("JumpAttack");
        bossState = BossState.WindUp;
        nextAttackTime = Time.time + attackCooldown;

        animator.SetTrigger("AttackSlam");
        yield return new WaitForSeconds(0.6f);

        bossState = BossState.Attacking;
        isGrounded = false;

        /*rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        yield return new WaitUntil(() => rb.linearVelocity.y <= 0);

        yield return new WaitUntil(() => isGrounded);*/

        //FIXME DoJumpDamage();

        /*bossState = BossState.Recovering;
        yield return new WaitForSeconds(0.5f);

        bossState = BossState.Idle;*/
    }

    public void DoJumpDamage()
    {
        Debug.Log("DoJumpDamage");
        EndJumpMove();
        isGrounded = true;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, jumpDamageRadius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<PlayerHealth>()
                    ?.TakeDamage(jumpDamage, transform.position);
            }
        }
        StartCoroutine(recovering());
    }

    private IEnumerator recovering()
    {
        bossState = BossState.Recovering;
        yield return new WaitForSeconds(0.5f);

        bossState = BossState.Idle;
    }


    /*private IEnumerator PunchAttack()
    {
        Debug.Log("PunchAttack");
        bossState = BossState.WindUp;
        nextAttackTime = Time.time + attackCooldown;

        animator.SetTrigger("Punch");
        yield return new WaitForSeconds(0.4f);

        bossState = BossState.Attacking;

        Vector2 dir = (player.position - transform.position).normalized;
        Vector2 center = (Vector2)transform.position + dir * punchRange * 0.5f;

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            center,
            new Vector2(punchRange, 1.5f),
            0f
        );

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<PlayerHealth>()
                    ?.TakeDamage(punchDamage, transform.position);
            }
        }

        bossState = BossState.Recovering;
        yield return new WaitForSeconds(0.6f);

        bossState = BossState.Idle;
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
            isGrounded = true;
    }

    public void EndAttack()
    {
    }

    public void StartJumpMove()
    {
        if (player == null) return;

        isJumping = true;

        jumpDirection = ((Vector2)player.position - rb.position).normalized;
    }

    private void EndJumpMove()
    {
        isJumping = false;
    }

    protected override void Die()
    {
        if (isDead) return;

        isDead = true;
        //OnDeath?.Invoke();

        animator?.SetTrigger("Die");

        Collider2D col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.reproducirMusicaGameWin();
        }

        if (SceneMaster.Instance != null)
        {
            SceneMaster.Instance.win(dialoguePopup);
        }
    }
}

