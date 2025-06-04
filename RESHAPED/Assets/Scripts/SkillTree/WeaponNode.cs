using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponNode : MonoBehaviour
{
    [Header("Node Properties")]
    public string nodeName; // Name of the skill tree node
    public bool isUnlocked; // Indicates if the node is unlocked

    [SerializeField] private bool canBeUnlocked; // Indicates if the node can be unlocked

    [Space(10)]
    [SerializeField] private string mainEffect; // Main effect of the node
    public string MainEffect // Property to get the main effect of the node
    {
        get { return mainEffect; }
        set { this.mainEffect = value; }
    }

    [SerializeField] private float mainEffectValue; // Value of the main effect
    public float MainEffectValue // Property to get the value of the main effect
    {
        get { return mainEffectValue; }
        set { this.mainEffectValue = value; }
    }

    [SerializeField] private string secondaryEffect; // Secondary effect of the node
    public string SecondaryEffect // Property to get the secondary effect of the node
    {
        get { return secondaryEffect; }
        set { this.secondaryEffect = value; }
    }

    [SerializeField] private float secondaryEffectValue; // Value of the secondary effect
    public float SecondaryEffectValue // Property to get the value of the secondary effect
    {
        get { return secondaryEffectValue; }
        set { this.secondaryEffectValue = value; }
    }

    [Header("Node Connections")]
    [SerializeField] private WeaponNode[] connectedNodes; // Array of connected nodes
    [SerializeField] private List<Slider> links; // List of sliders for the node

    [Header("Node SFX")]
    [SerializeField] private AudioClip unlockSkillSound; // Sound to play when a skill is unlocked


    private Image sprite; // Reference to the Image component
    private Color spriteColor; // Color of the sprite
    void Start()
    {
        nodeName = gameObject.name; // Set the node name to the name of the GameObject
        isUnlocked = false; // Initialize the node as locked
        links.Clear();
        GetNodeLinks(); // Get the links of the node

        sprite = GetComponentInChildren<Image>(); // Get the Image component attached to the node
        spriteColor = new Color(85, 255, 0, 255); // Set the color of the sprite renderer
    }

    void Update()
    {

    }

    void OnMouseDown()
    {
        CheckPrevious(); // Check if the node can be unlocked based on previous nodes
        TryUnlockNode(); // Attempt to unlock the node when clicked
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
        if (canBeUnlocked && SkillTreeManager.Instance.WeaponSkillPoints > 0) // Check if the node can be unlocked
        {
            AudioManager.Instance.PlayClip(unlockSkillSound); // Play the unlock skill sound
            isUnlocked = true; // Unlock the node
            SkillTreeManager.Instance.WeaponSkillPoints--; // Decrease skill points
            print("Node Unlocked: " + nodeName); // Debug message to indicate the node is unlocked
            sprite.color = spriteColor; // Set the color of the sprite renderer to its original color
            sprite.transform.position = transform.position; // Set the position of the sprite renderer to the node's position
            GetNodeLinks(); // Get the links of the node
            UpdateNodeLinks(); // Update the links of the node
            UnlockNode(); // Call the method to unlock the node
            SkillTreeManager.Instance.WeaponSkillPointTextUpdate(); // Update the skill point text UI element
        }
        else
        {
            print("Node cannot be unlocked: " + nodeName); // Debug message to indicate the node cannot be unlocked
        }
    }

    void UnlockNode()
    {
        Shoot.Instance.SetWeaponStats(mainEffect, mainEffectValue, secondaryEffect, secondaryEffectValue); // Set the weapon stats based on the node's effects
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
