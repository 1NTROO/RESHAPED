using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Speed of the player movement
    public float maxSpeed = 10f; // Maximum speed of the player
    private Vector2 movementInput; // Store the movement input
    private Rigidbody2D rb; // Reference to the Rigidbody2D component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the player
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal"); // Get horizontal input (A/D or Left/Right arrow keys)
        movementInput.y = Input.GetAxisRaw("Vertical"); // Get vertical input (W/S or Up/Down arrow keys)

        // Normalize the movement vector to ensure consistent speed in all directions
        if (movementInput != Vector2.zero)
        {
            movementInput.Normalize();
        }

        movementInput *= moveSpeed * Time.deltaTime; // Scale the input by move speed and delta time

        rb.AddForce(movementInput, ForceMode2D.Force); // Apply the movement force to the Rigidbody2D

        if (rb.linearVelocity.magnitude > maxSpeed) // Check if the player exceeds the maximum speed
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed; // Clamp the velocity to the maximum speed
        }
    }
}
