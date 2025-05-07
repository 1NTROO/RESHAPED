using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed on scene load
        }
    }

    [Header("Player Public Stats")]

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

    [Header("Player Cooldown Stats")]
    public float cooldownBase = 0.5f; // Base cooldown time for the player
    public float cooldownMult = 1f; // Cooldown multiplier for the player	
    public float cooldownTotal; // Maximum cooldown time for the player

    [Header("Player Experience Stats")]
    public float currentXP; // Current experience points of the player
    public float xpToLevelUp = 100; // Experience points required to level up
    public float level; // Current level of the player

    [Header("UI Assignables")]
    [SerializeField] private GameObject xpSlider; // Reference to the XP slider UI element
    [SerializeField] private GameObject levelText; // Reference to the level text UI element

    [Header("Player Private Stats")]
    [Inspectable] public float health; // Current health of the player

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
    }

    public void IncreaseStatMult(string stat, float amount)
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
                damageTotal = damageBase * damageMult; // Update the base damage with the multiplier
                break;
            case "MS":
                speedMult += amount; // Increase speed by the specified amount
                speedTotal = speedBase * speedMult; // Update the base speed with the multiplier
                break;
            case "CD":
                cooldownMult -= amount; // Increase cooldown by the specified amount
                cooldownTotal = cooldownBase * cooldownMult; // Update the base cooldown with the multiplier
                break;
            case "notable":
                // Handle notable effects here if needed
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
        level++; // Increase the player's level by 1
        currentXP -= xpToLevelUp; // Deduct the required experience points for leveling up from the current experience points
        xpToLevelUp = Mathf.RoundToInt(xpToLevelUp * 1.5f); // Increase the required experience points for the next level up
        SkillTreeManager.Instance.AddSkillPoint(); // Add a skill point to the player

        levelText.GetComponent<TMPro.TextMeshProUGUI>().text = "Level: " + level; // Update the level text UI element with the new level
    }
}
