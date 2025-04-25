using UnityEngine;

public class BulletHandler : MonoBehaviour
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
            print("Bullet Destroyed"); // Print a message to the console
            Destroy(gameObject); // Destroy the bullet game object
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) // Check if the bullet collides with an object tagged as "Enemy"
        {
            // // GameObject enemy = other.GetComponent<EnemyStats>(); // Get the Enemy component from the collided object
            // if (enemy != null)
            // {
            //     enemy.TakeDamage(damage); // Call the TakeDamage method on the enemy to apply damage
            // }
            // Destroy(gameObject); // Destroy the bullet game object after hitting the enemy
        }
    }
}
