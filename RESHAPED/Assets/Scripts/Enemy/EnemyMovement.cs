using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Enemy Movement Settings")]
    public float speed = 5f; // Speed of the enemy movement

    [Header("Enemy Targeting")]
    public Transform target; // Target to follow (e.g., player)
    public float detectionRange = 10f; // Range within which the enemy can detect the target

    private EnemyStats enemyStats; // Reference to the EnemyStats component

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform; // Find the player object by tag
        enemyStats = GetComponent<EnemyStats>(); // Get the EnemyStats component attached to this GameObject
    }

    void Update()
    {
        if (IsTargetInRange())
        {
            FollowPlayer();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats.Instance.TakeDamage(enemyStats.damageTotal); // Example damage to player
            Destroy(gameObject); // Destroy the enemy on collision with player
        }
    }

    void FollowPlayer()
    {
        if (target != null)
        {
            // Calculate the direction to the target
            Vector3 direction = (target.position - transform.position).normalized;
            // Move the enemy towards the target
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    bool IsTargetInRange()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            return distanceToTarget <= detectionRange;
        }
        return false;
    }
}
