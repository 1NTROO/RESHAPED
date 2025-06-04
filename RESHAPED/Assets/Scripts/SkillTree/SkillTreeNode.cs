using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private List<Slider> links; // List of sliders for the node

    [Header("Node SFX")]
    [SerializeField] private AudioClip unlockSkillSound; // Sound to play when a skill is unlocked

    private Image sprite; // Reference to the Image component
    private Color spriteColor; // Color of the sprite


    void Start()
    {
        // Initialize the node properties
        nodeName = gameObject.name; // Set the node name to the name of the GameObject
        // canBeUnlocked = false;
        isUnlocked = false; // Initialize the node as locked
        links.Clear();
        GetNodeLinks(); // Get the links of the node

        sprite = GetComponentInChildren<Image>(); // Get the SpriteRenderer component attached to the node
        spriteColor = new Color(85, 255, 0, 255); // Get the color of the sprite renderer
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
            AudioManager.Instance.PlayClip(unlockSkillSound); // Play the unlock skill sound
            isUnlocked = true; // Unlock the node
            SkillTreeManager.Instance.SkillPoints--; // Decrease skill points
            print("Node Unlocked: " + nodeName); // Debug message to indicate the node is unlocked
            sprite.color = spriteColor; // Set the color of the sprite renderer to its original color
            sprite.transform.position = transform.position; // Set the position of the sprite renderer to the node's position
            GetNodeLinks(); // Get the links of the node
            UpdateNodeLinks(); // Update the links of the node
            UnlockNode(); // Call the method to unlock the node
            SkillTreeManager.Instance.SkillPointTextUpdate(); // Update the skill point text UI element
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

    void GetNodeLinks()
    {
        links.AddRange(transform.parent.GetComponentsInChildren<Slider>()); // Add the sliders of the current node to the list of links
        // for (int i = 0; i < connectedNodes.Length; i++)
        // {
        //     Slider[] temp = connectedNodes[i].transform.parent.GetComponentsInChildren<Slider>(); // Get the sliders of the connected nodes
        //     for (int j = 0; j < temp.Length; j++)
        //     {
        //         if (links.Contains(temp[j])) // Check if each slider is already in the list
        //         {
        //             links.Remove(temp[j]); // Remove those sliders from the list
        //         }
        //     }
        //     links.AddRange(temp); // Add the slider of the connected node to the list of links
        // }
    }

    void UpdateNodeLinks()
    {
        for (int i = 0; i < links.Count; i++)
        {
            if (links[i].value == 0.0f) links[i].value = 1.0f; // Set the value of the slider to 1.0f if the node is unlocked
        }
    }
}
