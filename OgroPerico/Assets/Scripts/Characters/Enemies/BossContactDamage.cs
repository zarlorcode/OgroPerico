using UnityEngine;

public class BossContactDamage : MonoBehaviour
{
    public int damage = 2;
    public float damageCooldown = 1f;
    private float lastDamageTime;

    private void OnCollisionStay2D(Collision2D collision) //FIXME, enemies have more code here
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                collision.collider.GetComponent<PlayerHealth>()?.TakeDamage(damage, transform.position);

                lastDamageTime = Time.time;
            }
        }
    }
}

