using UnityEngine;

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
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed on scene load
        }
    }
    
    private int skillPoints; // Number of skill points available to the player
    public int SkillPoints // Property to get the number of skill points
    {
        get { return skillPoints; }
        set { skillPoints = value; }
    }

    [SerializeField] private GameObject skillTreeCanvas; // Reference to the skill tree canvas

    [SerializeField] private GameObject skillTreeNodes; // Reference to the skill tree nodes

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GoToSkillTree()
    {

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
            // skillTreeNodes.transform.position = skillTreeCanvas.transform.position; // Reset the position of the skill tree nodes
        }   
    }

    public void ExitSkillTree()
    {
        Time.timeScale = 1; // Resume the game
        print("Game Resumed"); // Debug message to indicate the game is resumed
        skillTreeCanvas.SetActive(false); // Hide the skill tree canvas
        skillTreeNodes.SetActive(false); // Hide the skill tree nodes
    }

    public void AddSkillPoint()
    {
        skillPoints++;
    }
}
