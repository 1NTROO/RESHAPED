using UnityEngine;
using UnityEngine.TextCore;

public class SkillTreeManager : MonoBehaviour
{
    private static SkillTreeManager instance; // Singleton instance of SkillTreeManager
    public static SkillTreeManager Instance { get { return instance; } } // Public property to access the instance

    private void Awake()
    {
        if (instance != null && instance != this) // Check if an instance already exists
        {
            Destroy(gameObject); // Destroy the duplicate instance
        }
        else
        {
            instance = this; // Set the current instance as the singleton instance
        }
    }

    private int skillPoints; // Number of skill points available to the player
    public int SkillPoints // Property to get the number of skill points
    {
        get { return skillPoints; }
        set { skillPoints = value; }
    }

    private int weaponSkillPoints; // Number of weapon skill points available to the player
    public int WeaponSkillPoints // Property to get the number of weapon skill points
    {
        get { return weaponSkillPoints; }
        set { weaponSkillPoints = value; }
    }

    [Header("Skill Tree Settings")]
    [SerializeField] private GameObject skillTreeCanvas; // Reference to the skill tree canvas

    [SerializeField] private GameObject skillTreeNodes; // Reference to the skill tree nodes

    [SerializeField] private TMPro.TextMeshProUGUI skillPointText; // Reference to the skill point text UI element

    [Header("Weapon Skill Tree Settings")]
    [SerializeField] private GameObject weaponSkillTreeCanvas; // Reference to the weapon skill tree canvas
    [SerializeField] private GameObject weaponSkillTreeNodes; // Reference to the weapon skill tree nodes
    [SerializeField] private TMPro.TextMeshProUGUI weaponSkillPointText; // Reference to the weapon skill point text UI element
    [Header("Pause Menu Settings")]
    [SerializeField] private GameObject pauseMenuCanvas; // Reference to the pause menu canvas

    [Header("Skill Tree Sounds")]
    [SerializeField] private AudioClip skillTreeOpenSound; // Sound to play when the skill tree is opened

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame(); // Pause the game when the Escape key is pressed
        }
    }

    public void GoToSkillTree()
    {
        AudioManager.Instance.PlayClip(skillTreeOpenSound); // Play the skill tree open sound
        if (skillTreeCanvas.activeSelf) // Check if the skill tree canvas is already active
        {
            ExitSkillTree(); // If it is, exit the skill tree
        }
        else
        {
            Time.timeScale = 0; // Pause the game
            print("Game Paused"); // Debug message to indicate the game is paused
            skillTreeCanvas.SetActive(true); // Show the skill tree canvas
            skillTreeNodes.SetActive(true); // Show the skill tree nodes
        }
    }

    public void GoToWeaponSkillTree()
    {
        AudioManager.Instance.PlayClip(skillTreeOpenSound); // Play the skill tree open sound
        if (weaponSkillTreeCanvas.activeSelf) // Check if the weapon skill tree canvas is already active
        {
            ExitWeaponSkillTree(); // If it is, exit the skill tree
        }
        else
        {
            Time.timeScale = 0; // Pause the game
            print("Game Paused"); // Debug message to indicate the game is paused
            weaponSkillTreeCanvas.SetActive(true); // Show the weapon skill tree canvas
            weaponSkillTreeNodes.SetActive(true); // Show the weapon skill tree nodes
        }
    }

    public void ExitSkillTree()
    {
        Time.timeScale = 1; // Resume the game
        print("Game Resumed"); // Debug message to indicate the game is resumed
        skillTreeCanvas.SetActive(false); // Hide the skill tree canvas
        skillTreeNodes.SetActive(false); // Hide the skill tree nodes
    }

    public void ExitWeaponSkillTree()
    {
        Time.timeScale = 1; // Resume the game
        print("Game Resumed"); // Debug message to indicate the game is resumed
        weaponSkillTreeCanvas.SetActive(false); // Hide the weapon skill tree canvas
        weaponSkillTreeNodes.SetActive(false); // Hide the weapon skill tree nodes
    }

    public void AddSkillPoint()
    {
        skillPoints++;
        SkillPointTextUpdate(); // Update the skill point text UI element
    }

    public void AddWeaponSkillPoint()
    {
        weaponSkillPoints++;
        WeaponSkillPointTextUpdate(); // Update the weapon skill point text UI element
    }

    public void SkillPointTextUpdate()
    {
        skillPointText.text = "Skill Points: " + skillPoints; // Update the skill point text UI element
    }

    public void WeaponSkillPointTextUpdate()
    {
        weaponSkillPointText.text = "Weapon Skill Points: " + weaponSkillPoints; // Update the weapon skill point text UI element
    }

    public void PauseGame()
    {
        if (pauseMenuCanvas.activeSelf) // Check if the pause menu canvas is already active
        {
            pauseMenuCanvas.SetActive(false); // If it is, hide the pause menu canvas
            Time.timeScale = 1; // Resume the game
        }
        else
        {
            Time.timeScale = 0; // Pause the game
            pauseMenuCanvas.SetActive(true); // Show the pause menu canvas
        }
    }
}
