using UnityEngine;

public abstract class BossBase : EnemyBase
{
    protected enum BossState
    {
        Idle,
        WindUp,
        Attacking,
        Recovering
    }

    protected BossState bossState = BossState.Idle;

    [Header("Boss Timing")]
    public float attackCooldown = 6f;
    protected float nextAttackTime = 0f;

    protected override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }
    protected override void Update()
    {
        if (isDead || player == null) return;

        HandleBossLogic();
        HandleSpriteFlip();
    }

    protected override void FixedUpdate()
    {
        HandleKnockback();
        // IMPORTANTE: el boss SOLO se mueve si tú lo ordenas
    }

    protected abstract void HandleBossLogic();

    /*public override void TakeDamage(int amount, Vector2 hitSourcePosition)
    {
        if (isDead) return;

        Vector2 dir = ((Vector2)transform.position - hitSourcePosition).normalized;
        ApplyKnockback(dir, force: 2f, duration: 0.2f);
        AudioManager.Instance.reproducirEfectoDañar();

        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();

        // Activate invulnerability
        StartCoroutine(blink());
    }*/

}

