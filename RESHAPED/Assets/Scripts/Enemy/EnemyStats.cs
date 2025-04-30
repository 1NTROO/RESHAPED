using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Enemy Public Stats")]
    public float healthBase = 100f; // Base health of the enemy
    public float healthMult = 1f; // Health multiplier for the enemy
    public float healthTotal; // Maximum health of the enemy
    public float damageBase = 10f; // Base damage dealt by the enemy
    public float damageMult = 1f; // Damage multiplier for the enemy
    public float damageTotal; // Maximum damage dealt by the enemy

    public float xpBase = 100; // Experience points awarded for defeating the enemy
    public float xpMult = 1f; // Experience points multiplier for the enemy
    public float xpTotal; // Total experience points awarded for defeating the enemy

    [Header("Enemy Misc")]
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    void Start()
    {
        healthTotal = healthBase; // Initialize the total health with the base health
        damageTotal = damageBase; // Initialize the total damage with the base damage
        xpTotal = xpBase; // Initialize the total experience points with the base experience points

        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // Get the SpriteRenderer component from the child object
    }

    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        healthTotal -= damage; // Decrease the enemy's health by the damage amount
        HealthOpacity(); // Update the opacity of the enemy based on its health
        if (healthTotal <= 0f) // Check if the enemy's health is less than or equal to zero
        {
            OnDeath(); // Run death logic
        }
    }

    void HealthOpacity()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, (healthTotal / (healthBase * healthMult))); // Set the opacity of the enemy based on its health
    }

    void OnDeath()
    {
        PlayerStats.Instance.currentXP += xpTotal; // Award experience points to the player
        PlayerStats.Instance.CheckLevelUp(); // Check if the player can level up

        Destroy(gameObject); // Destroy the enemy game object
    }
}
