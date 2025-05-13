using UnityEngine;

public class SkillTreeNode : MonoBehaviour
{
    [Header("Node Properties")]
    public string nodeName; // Name of the skill tree node
    public bool isUnlocked; // Indicates if the node is unlocked

    [SerializeField] private bool canBeUnlocked; // Indicates if the node can be unlocked

    [SerializeField] private string[] effects;
    public string[] Effects // Property to get the effect of the node
    {
        get { return effects; }
        set { this.effects = value; }
    }

    [SerializeField] private int[] values;
    public int[] Values // Property to get the value of the node
    {
        get { return values; }
        set { this.values = value; }
    }

    public bool isNotable; // Indicates if the node is a notable node
    [SerializeField] private string notable;
    public string Notable // Property to get the notable effect of the node
    {
        get { return notable; }
        set { this.notable = value; }
    }

    [Header("Node Connections")]
    [SerializeField] private SkillTreeNode[] connectedNodes; // Array of connected nodes
    [SerializeField] private NotableNode notableNode; // Reference to the notable node

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component


    void Start()
    {
        // Initialize the node properties
        nodeName = gameObject.name; // Set the node name to the name of the GameObject
        // canBeUnlocked = false;
        isUnlocked = false; // Initialize the node as locked

        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // Get the SpriteRenderer component attached to the node
        spriteRenderer.enabled = false; // Hide the sprite renderer initially
    }

    void Update()
    {
        
    }

    void OnMouseDown()
    {
        print("Node Clicked: " + nodeName); // Debug message to indicate the node is clicked
        CheckPrevious(); // Check if the previous nodes are unlocked
        TryUnlockNode(); // Attempt to unlock the node
    }

    void CheckPrevious()
    {
        if (isUnlocked) // Check if the node is already unlocked
        {
            return; // If so, exit the method
        }
        for (int i = 0; i < connectedNodes.Length; i++)
        {
            if (connectedNodes[i].isUnlocked) // Check if a previous node is unlocked
            {
                canBeUnlocked = true; // If so, set this node to be able to be unlocked
                return;
            }
        }
    }

    void TryUnlockNode()
    {
        if (canBeUnlocked && SkillTreeManager.Instance.SkillPoints > 0) // Check if the node can be unlocked
        {
            isUnlocked = true; // Unlock the node
            SkillTreeManager.Instance.SkillPoints--; // Decrease skill points
            print("Node Unlocked: " + nodeName); // Debug message to indicate the node is unlocked
            spriteRenderer.enabled = true; // Show the sprite renderer
            spriteRenderer.transform.position = transform.position; // Set the position of the sprite renderer to the node's position
            UnlockNode(); // Call the method to unlock the node
        }
        else
        {
            print("Node cannot be unlocked: " + nodeName); // Debug message to indicate the node cannot be unlocked
        }
    }

    void UnlockNode()
    {
        if (isNotable) // Check if the node is a notable node
        {
            notableNode = GetComponent<NotableNode>(); // Get the NotableNode component attached to the node
            PlayerStats.Instance.IncreaseStatMult("notable", 0, notableNode); // Call the method to increase the player stats with notable effect
            return;
        }

        else 
        {
            // Apply the effects of the node to the player stats
            for (int i = 0; i < effects.Length; i++)
            {
                PlayerStats.Instance.IncreaseStatMult(effects[i], values[i]); // Call the method to increase the player stats
            }
        }

    }
}
