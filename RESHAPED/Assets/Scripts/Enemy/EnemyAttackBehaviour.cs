using UnityEngine;

public class EnemyAttackBehaviour : MonoBehaviour
{
    [Header("Enemy Attack Settings")]
    public float attackRate = 3000f;
    private float nextAttackTime = 0f; // Time when the next attack can occur

    [Header("Enemy Shooting Settings")]
    public GameObject bulletPrefab; // Prefab for the bullet
    public Transform firePoint; // Point from where the bullet will be fired
    public float bulletSpeed = 20f; // Speed of the bullet
    private float nextFireTime = 0f; // Time when the next bullet can be fired

    private EnemyStats enemyStats; // Reference to the EnemyStats component
    private EnemyMovement enemyMovement; // Reference to the EnemyMovement component

    void Start()
    {
        attackRate /= 1000; // Convert attack rate to seconds

        enemyStats = GetComponent<EnemyStats>(); // Get the EnemyStats component attached to this GameObject
        enemyMovement = GetComponent<EnemyMovement>(); // Get the EnemyMovement component attached to this GameObject
    }

    void Update()
    {
        if (nextAttackTime <= attackRate)
        {
            nextAttackTime += Time.deltaTime; // Increment the attack timer
        }
        if (enemyMovement.readyToAttack)
        {
            Attack(); // Call the attack method
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && nextAttackTime >= attackRate)
        {
            Attack(); // Call the attack method
        }
    }

    void Attack()
    {
        switch (enemyStats.thisEnemyType) // Check the type of enemy
        {
            case EnemyStats.EnemyType.Melee:
                MeleeAttack(); // Call the melee attack method
                break;
            case EnemyStats.EnemyType.Ranged:
                RangedAttack(); // Call the ranged attack method
                break;
            case EnemyStats.EnemyType.Boss:
                BossAttack(); // Call the boss attack method
                break;
            default:
                Debug.LogWarning("Invalid enemy type: " + enemyStats.thisEnemyType); // Log a warning for invalid enemy type
                break;
        }
        nextAttackTime = 0f; // Reset the attack timer
    }

    void MeleeAttack()
    {
        PlayerStats.Instance.TakeDamage(enemyStats.damageTotal); // Example damage to player
        print("Player hit by enemy!"); // Debug message
    }

    void RangedAttack()
    {
        firePoint = transform;
        if (Time.time >= nextFireTime) // Check if the current time is greater than or equal to the next fire time
        {
            nextFireTime = Time.time + attackRate; // Set the next fire time based on the fire rate

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); // Create a new bullet instance
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component of the bullet

            bullet.GetComponent<EnemyBulletHandler>().damage = enemyStats.damageTotal; // Set the damage of the bullet

            Vector2 fireDirection = ((Vector2)enemyMovement.target.position - (Vector2)firePoint.position).normalized; // Calculate the direction from the fire point to the mouse position

            rb.AddForce(fireDirection * bulletSpeed, ForceMode2D.Impulse); // Apply force to the bullet in the direction of the fire point's up vector
        }

    }

    void BossAttack()
    {
        
    }

}
