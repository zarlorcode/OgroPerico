using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    public int damage = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger with enemy");
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("trigger is enemy");
            GhostEnemy enemy = collision.GetComponent<GhostEnemy>();
            if (enemy != null)
            {
                Debug.Log("call takeDamage Enemy");
                enemy.TakeDamage(damage, transform.position);
            }
        } else
        {
            Debug.Log("trigger not enemy");
        }
    }
}

