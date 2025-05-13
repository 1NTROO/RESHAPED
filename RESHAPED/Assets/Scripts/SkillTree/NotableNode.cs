using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class NotableNode : MonoBehaviour
{
    [Header("Notable Properties")]
    public string nodeName; // Name of the skill tree node

    [SerializeField] private SkillTreeNode thisNode; // Reference to the SkillTreeNode component

    public enum NotableType // Type of the notable node
    { 
        SpeedDamage,
        CooldownDamage,
        SpeedHealth,
        CooldownHealth
    };

    public NotableType thisNotableType; // Type of the notable node

    void Start()
    {
        thisNode = GetComponent<SkillTreeNode>(); // Get the SkillTreeNode component attached to this GameObject
    }

    void Update()
    {
        
    }

    public void AddNotableSkill()
    {
        switch (thisNotableType) // Check the type of the notable node
        {
            case NotableType.SpeedDamage:
                var tempSpeedMult = PlayerStats.Instance.speedMult; // Get the current damage multiplier
                PlayerStats.Instance.IncreaseStatMult("MS", 100 * ((PlayerStats.Instance.damageMult - 1) / 4)); // Increase movement speed by 25% of damage multiplier
                PlayerStats.Instance.IncreaseStatMult("DMG", 100 * ((tempSpeedMult - 1) / 2)); // Increase damage by 50% of speed multiplier
                break;
            case NotableType.CooldownDamage:
                break;
            case NotableType.SpeedHealth:
                PlayerStats.Instance.IncreaseStatMult("HP", 33); // Increase health by 33%
                break;
            case NotableType.CooldownHealth:
                break;
            default:
                Debug.LogWarning("Invalid notable type: " + thisNotableType); // Log a warning for invalid notable type
                break;
        }
        PlayerStats.Instance.notableTypes.Add(thisNotableType); // Add the notable type to the player's stats
    }
}
