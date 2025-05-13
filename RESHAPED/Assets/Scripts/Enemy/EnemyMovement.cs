using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Enemy Movement Settings")]
    public float speed = 5f; // Speed of the enemy movement

    [Header("Enemy Targeting")]
    public Transform target; // Target to follow (e.g., player)
    public float detectionRange = 10f; // Range within which the enemy can detect the target
    public bool readyToAttack = false; // Flag to indicate if the enemy is ready to attack

    private EnemyStats enemyStats; // Reference to the EnemyStats component

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform; // Find the player object by tag
        enemyStats = GetComponent<EnemyStats>(); // Get the EnemyStats component attached to this GameObject
    }

    void Update()
    {
        switch (enemyStats.thisEnemyType) // Check the type of enemy
        {
            case EnemyStats.EnemyType.Melee:
                if (IsTargetInRange())
                {
                    FollowPlayer();
                }
                break;
            case EnemyStats.EnemyType.Ranged:
                if (IsTargetInRange())
                {
                    FollowPlayer(10f);
                }
                break;
            case EnemyStats.EnemyType.Boss:
                if (IsTargetInRange())
                {
                    FollowPlayer();
                }
                break;
            default:
                Debug.LogWarning("Invalid enemy type: " + enemyStats.thisEnemyType); // Log a warning for invalid enemy type
                break;
        }
    
    }
    void FollowPlayer(float targetDistance = 0)
    {
        if (target != null)
        {
            // Calculate the direction to the target
            Vector3 direction = (target.position - transform.position).normalized;
            if (targetDistance != 0 && direction.magnitude <= targetDistance)
            {
                readyToAttack = true; // Set the flag to indicate that the enemy is ready to attack
                return;
            }
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
