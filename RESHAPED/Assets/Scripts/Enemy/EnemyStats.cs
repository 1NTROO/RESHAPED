using System;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public enum EnemyType // Enum to define the type of enemy (e.g., melee, ranged, etc.)
    {
        Melee,
        Ranged,
        Boss
    }

    [Header("Enemy Type")]
    public EnemyType thisEnemyType; // Type of the enemy

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
        healthTotal = healthBase * healthMult; // Calculate the total health based on base and multiplier
        damageTotal = damageBase * damageMult; // Calculate the total damage based on base and multiplier
        xpTotal = xpBase * xpMult; // Calculate the total experience points based on base and multiplier

        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // Get the SpriteRenderer component from the child object
    }

    void Update()
    {
        
    }

    public void OnSpawn()
    {
        int playerLevel = (int)PlayerStats.Instance.level; // Get the player's level from PlayerStats

        float levelMult = 1 + (playerLevel / 10f); // Calculate the level multiplier based on the player's level
        healthMult = levelMult; // Set the health multiplier based on the level multiplier
        damageMult = levelMult; // Set the damage multiplier based on the level multiplier
        xpMult = levelMult; // Set the experience points multiplier based on the level multiplier
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
        PlayerStats.Instance.OnKill(); // Call the OnKill method in PlayerStats

        Destroy(gameObject); // Destroy the enemy game object
    }
}
