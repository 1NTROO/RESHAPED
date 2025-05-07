using UnityEngine;

public class SkillTreeNode : MonoBehaviour
{
    [Header("Node Properties")]
    public string nodeName; // Name of the skill tree node
    public bool isUnlocked; // Indicates if the node is unlocked

    private bool canBeUnlocked; // Indicates if the node can be unlocked


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



    [Header("Node Requirements")]
    [SerializeField] private SkillTreeNode[] previous; // Indicates if the previous node is unlocked

    [Header("Node Connections")]
    [SerializeField] private SkillTreeNode[] connectedNodes; // Array of connected nodes


    void Start()
    {
        // Initialize the node properties
        nodeName = gameObject.name; // Set the node name to the name of the GameObject
        canBeUnlocked = false;
        isUnlocked = false; // Initialize the node as locked
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
        if (previous.Length == 0) // Check if there are no previous nodes
        {
            canBeUnlocked = true; // If so, set this node to unlocked
            return;
        }
        for (int i = 0; i < previous.Length; i++)
        {
            if (previous[i].isUnlocked == true) // Check if a previous node is unlocked
            {
                canBeUnlocked = true; // If so, set this node to be able to be unlocked
                return;
            }
        }
    }

    void TryUnlockNode()
    {
        if (canBeUnlocked) // Check if the node can be unlocked
        {
            isUnlocked = true; // Unlock the node
            SkillTreeManager.Instance.SkillPoints--; // Decrease skill points
            print("Node Unlocked: " + nodeName); // Debug message to indicate the node is unlocked
            UnlockNode(); // Call the method to unlock the node
        }
        else
        {
            print("Node cannot be unlocked: " + nodeName); // Debug message to indicate the node cannot be unlocked
        }
    }

    void UnlockNode()
    {

    }
}
