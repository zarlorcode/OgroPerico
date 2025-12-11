using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Animator animator;

    public Collider2D hitbox;

    public GameObject slash;

    private SpriteRenderer slashRenderer;

    private bool isAttacking = false;

    private void Awake()
    {
        if (slash != null)
        {
            slashRenderer = slash.GetComponent<SpriteRenderer>();
        }

        if (hitbox != null) hitbox.enabled = false;
        if (slash != null) slash.SetActive(false);
    }

    public void EnableHitbox()
    {
        hitbox.enabled = true;
        Debug.Log("enableHitBox");
        if (slash != null)
        {
            Debug.Log("slash active");
            slash.SetActive(true);
            if (slashRenderer != null) slashRenderer.enabled = true;
        }
    }

    public void DisableHitbox()
    {
        hitbox.enabled = false;
        Debug.Log("disableHitBox");
        if (slash != null)
        {
            Debug.Log("slash disabled");
            if (slashRenderer != null) slashRenderer.enabled = false;
            slash.SetActive(false);
        }
    }

    public void Attack()
    {
        if (isAttacking)
        {
            Debug.Log("isAttackin already true");
            return;
        }
        isAttacking = true;

        animator.SetTrigger("Attack");
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}
