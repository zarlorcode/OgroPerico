using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Animator animator;

    public Collider2D hitbox;

    private bool isAttacking = false;

    private void Start()
    {
        hitbox.enabled = false;
    }

    public void EnableHitbox()
    {
        hitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        hitbox.enabled = false;
    }

    public void Attack()
    {
        if (isAttacking) return;
        isAttacking = true;

        animator.SetTrigger("Attack");
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}
