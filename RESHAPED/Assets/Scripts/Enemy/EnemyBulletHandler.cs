using UnityEngine;

public class EnemyBulletHandler : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float lifeTime = 5f; // Time in seconds before the bullet is destroyed
    public float damage; // Damage dealt by the bullet
    void Start()
    {
        
    }

    void Update()
    {
        lifeTime -= Time.deltaTime; // Decrease the lifetime of the bullet
        if (lifeTime <= 0f) // Check if the bullet's lifetime has expired
        {
            Destroy(gameObject); // Destroy the bullet game object
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player")) // Check if the bullet collides with an object tagged as "Enemy"
        {
            GameObject player = collision.collider.gameObject; // Get the Enemy component from the collided object
            if (player != null)
            {
                player.GetComponent<PlayerStats>().TakeDamage(damage); // Call the TakeDamage method on the enemy to apply damage
            }
            Destroy(gameObject); // Destroy the bullet game object after hitting the enemy
        }
        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("EnemyBullet")) // Check if the bullet collides with an object tagged as "Enemy"
        {
            Destroy(gameObject); // Destroy the bullet game object after hitting the enemy
        }
    }
}
