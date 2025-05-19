using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance  { get { return instance; }} // Singleton instance of PlayerStats
    private static PlayerStats instance; // Static instance for easy access

    private void Awake()
    {
        if (instance != null && instance != this) // Check if an instance already exists
        {
            Destroy(gameObject); // Destroy the duplicate instance
        }
        else
        {
            instance = this; // Set the current instance as the singleton instance
            // DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed on scene load
        }
    }

    [Header("Player Public Stats")]
    public List<NotableNode.NotableType> notableTypes; // Types of the notable nodes

    [Header("Player Health Stats")]
    public float healthBase = 100f; // Base health of the player
    public float healthMult = 1f; // Health multiplier for the player
    public float healthTotal; // Maximum health of the player

    [Header("Player Damage Stats")]
    public float damageBase = 10f; // Base damage dealt by the player
    public float damageMult = 1f; // Damage multiplier for the player
    public float damageTotal; // Maximum damage dealt by the player

    [Header("Player Speed Stats")]
    public float speedBase = 1300f; // Base speed of the player
    public float speedMult = 1f; // Speed multiplier for the player
    public float speedTotal; // Maximum speed of the player
    public float speedTotalMult; // Maximum speed multiplier for the player

    [Header("Player Cooldown Stats")]
    public float cooldownBase = 0.5f; // Base cooldown time for the player
    public float cooldownMult = 1f; // Cooldown multiplier for the player	
    public float cooldownTotal; // Maximum cooldown time for the player
    public float cooldownTotalMult = 1f; // Maximum cooldown multiplier for the player

    [Header("Player Experience Stats")]
    public float currentXP; // Current experience points of the player
    public float xpToLevelUp; // Experience points required to level up
    public float level; // Current level of the player
    private float totalXP; // Total experience points of the player

    [Header("UI Assignables")]
    [SerializeField] private GameObject xpSlider; // Reference to the XP slider UI element
    [SerializeField] private GameObject levelText; // Reference to the level text UI element

    [Header("Player Misc")]
    [SerializeField] private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    [Header("Player Private Stats")]
    [Inspectable] public float health; // Current health of the player
    private bool hasSpeedBuff = false; // Flag to check if the player has a speed buff
    private bool hasHealthCooldownBuff = false; // Flag to check if the player has a health cooldown buff

    void Start()
    {
        healthTotal = healthBase; // Initialize the total health with the base health
        damageTotal = damageBase; // Initialize the total damage with the base damage
        speedTotal = speedBase; // Initialize the total speed with the base speed
        cooldownTotal = cooldownBase; // Initialize the total cooldown with the base cooldown

        health = healthTotal; // Set the current health to the total health
    }

    // Update is called once per frame
    void Update()
    {
        xpSlider.GetComponent<Slider>().value = currentXP / xpToLevelUp; // Update the XP slider value based on current XP and required XP to level up

        GetNotable(); // Call the method to check for notable effects

        if (Input.GetKeyDown(KeyCode.E)) // Check if the E key is pressed
        {
            QuickLevelUp(); // Call the method to quickly level up the player
        }
    }

    public void IncreaseStatMult(string stat, float amount, NotableNode notable = null)
    {
        amount /= 100;
        switch (stat)
        {
            case "HP":
                healthMult += amount; // Increase health by the specified amount
                healthTotal = healthBase * healthMult; // Update the base health with the multiplier
                break;
            case "DMG":
                damageMult += amount; // Increase damage by the specified amount
                if (notableTypes.Contains(NotableNode.NotableType.SpeedDamage)) // Check if the player has a SpeedDamage Notable
                {
                    speedMult += amount / 4; // Increase damage by the specified amount
                }
                damageTotal = damageBase * damageMult; // Update the base damage with the multiplier
                break;
            case "MS":
                speedMult += amount; // Increase speed by the specified amount
                if (notableTypes.Contains(NotableNode.NotableType.SpeedDamage)) // Check if the player has a SpeedDamage Notable
                {
                    damageMult += amount / 2; // Increase damage by the specified amount
                }
                speedTotal = speedBase * speedMult; // Update the base speed with the multiplier
                break;
            case "CD":
                cooldownMult -= amount; // Increase cooldown by the specified amount
                cooldownTotal = cooldownBase * cooldownMult; // Update the base cooldown with the multiplier
                break;
            case "notable":
                notable.AddNotableSkill(); // Call the method to add a notable skill
                break;
            default:
                Debug.LogWarning("Invalid stat name: " + stat); // Log a warning for invalid stat name
                break;
        }
    }

    public void CheckLevelUp()
    {
        if (currentXP >= xpToLevelUp) // Check if the current experience points are greater than or equal to the required experience points to level up
        {
            LevelUp(); // Call the method to level up the player
        }
    }

    public void LevelUp()
    {
        totalXP += xpToLevelUp; // Increase the total experience points by the required experience points for leveling up
        level++; // Increase the player's level by 1
        currentXP -= xpToLevelUp; // Deduct the required experience points for leveling up from the current experience points
        xpToLevelUp += 66; // Increase the required experience points for the next level up
        SkillTreeManager.Instance.AddSkillPoint(); // Add a skill point to the player

        levelText.GetComponent<TMPro.TextMeshProUGUI>().text = "Level: " + level; // Update the level text UI element with the new level
    }

    public void GetNotable()
    {
        foreach (NotableNode.NotableType notable in notableTypes) // Iterate through the notable types
        {
            if (notable == NotableNode.NotableType.CooldownDamage)
            {

            }
            else if (notable == NotableNode.NotableType.SpeedHealth)
            {
                if (health > healthTotal / 2 && !hasSpeedBuff) // Check if the player's health is above half of the total health
                {
                    hasSpeedBuff = true; // Set the flag to true
                    speedTotal *= speedTotalMult; // Increase speed by 20%
                }
                else if (health < healthTotal / 2 && hasSpeedBuff) // Check if the player's health is below half of the total health
                {
                    hasSpeedBuff = false; // Set the flag to false
                }
            }
            else if (notable == NotableNode.NotableType.CooldownHealth)
            {
                continue;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage; // Decrease the player's health by the damage amount
        HealthOpacity(); // Update the opacity of the player based on its health
        if (health <= 0f) // Check if the player's health is less than or equal to zero
        {
            OnDeath(); // Call the method to handle player death
        }
    }

    void HealthOpacity()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, (health / (healthBase * healthMult))); // Set the opacity of the enemy based on its health
    }

    public void OnDeath()
    {
        // Handle player death logic here
        Debug.Log("Player has died!"); // Log a message indicating player death
        totalXP += currentXP; // Add the current experience points to the total experience points
        KeepInfoOnLoad.Instance.KeepInfo(totalXP); // Call the method to keep player information on load
        Destroy(gameObject); // Destroy the player game object
        SceneManager.LoadScene("GameOverScene"); // Load the GameOver scene
    }

    public void OnFire()
    {
        if (notableTypes.Contains(NotableNode.NotableType.CooldownHealth)) // Check if the player has a CooldownHealth Notable
        {
            health += healthTotal / 50; // Increase health by 2% of the maximum health
        }
    }

    public void OnKill()
    {
        if (notableTypes.Contains(NotableNode.NotableType.CooldownDamage)) // Check if the player has a CooldownDamage Notable
        {
            cooldownTotalMult += 0.005f;
            cooldownTotal = cooldownBase * cooldownMult * (1 / (1 + cooldownTotalMult)); // Increase cooldown by 0.5%
        }
    }

    void QuickLevelUp()
    {
        currentXP = xpToLevelUp; // Set the current experience points to the required experience points for leveling up
        LevelUp(); // Call the method to level up the player
    }
}
