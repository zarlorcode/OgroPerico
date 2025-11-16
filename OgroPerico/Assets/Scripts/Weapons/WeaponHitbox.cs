using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    public int damage = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger with enemy");
        if (collision.CompareTag("Enemy"))
        {
            GhostEnemy enemy = collision.GetComponent<GhostEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, transform.position);
            }
        }
    }
}

