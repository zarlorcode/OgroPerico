using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    public int damage = 2;

    // call only if referenced in weaponCOntroller and is enabled teh hitbox
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
           EnemyBase enemy = collision.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, transform.position);
            }
        }
    }
}

