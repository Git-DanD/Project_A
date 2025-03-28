using UnityEngine;

public class SwordHandler : MonoBehaviour
{
    public Transform backPosition;   // Sword's back position
    public Transform combatPosition; // Sword's combat position
    public Transform sword;          // The sword itself
    public Transform player;         // Reference to the player

    public float transitionSpeed = 5f;  // Smooth movement speed
    private bool isInCombat = false;    // Combat mode toggle
    private float combatTimer = 0f;     // Timer for auto-return
    public float combatDuration = 5f;   // Time before returning to back position

    private SpriteRenderer swordRenderer; // Reference to the sword's sprite renderer
    private bool swordInFront = false;   // Track sword position (in front or behind)

    void Start()
    {
        isInCombat = false;

        // Initialize sword position and rotation
        sword.position = backPosition.position;
        sword.rotation = Quaternion.Euler(0, 0, 180); // Flipped on back position

        // Get the SpriteRenderer component of the sword for visibility control
        swordRenderer = sword.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Prevent movement calculations on the first frame
        if (Time.timeSinceLevelLoad < 0.1f)
        {
            sword.position = backPosition.position; // Snap sword directly to back position
            return;
        }

        // Enter combat mode when left-clicking
        if (Input.GetMouseButtonDown(0))
        {
            isInCombat = true;
            combatTimer = 0f;  // Reset combat timer
        }

        // If in combat, start counting down the timer
        if (isInCombat)
        {
            combatTimer += Time.deltaTime;
            if (combatTimer >= combatDuration)
            {
                isInCombat = false; // Return to back position after combat duration
            }
        }

        // Move sword smoothly to the correct position
        Transform targetPosition = isInCombat ? combatPosition : backPosition;
        sword.position = Vector3.Lerp(sword.position, targetPosition.position, Time.deltaTime * transitionSpeed);

        // Check if the player is moving up, down, left, or right
        Vector2 playerVelocity = player.GetComponent<Rigidbody2D>().linearVelocity;

        // If moving up (positive Y direction), place the sword behind the player
        if (playerVelocity.y > 0)
        {
            if (!swordInFront)
            {
                sword.position = new Vector3(sword.position.x, sword.position.y, backPosition.position.z - 1); // Behind
                swordInFront = false; // Track that sword is now behind
            }
        }
        // If moving down (negative Y direction), place the sword in front of the player
        else if (playerVelocity.y < 0)
        {
            if (swordInFront)
            {
                sword.position = new Vector3(sword.position.x, sword.position.y, backPosition.position.z + 1); // In front
                swordInFront = true; // Track that sword is now in front
            }
        }

        // Handle sword rotation based on combat state
        if (isInCombat)
        {
            sword.rotation = Quaternion.identity;  // Normal rotation when in combat
        }
        else
        {
            sword.rotation = Quaternion.Euler(0, 0, 180);  // Flipped when on back
        }

        // Handle sword visibility based on player movement
        HandleSwordVisibility(playerVelocity);
    }

    // Function to handle the sword's visibility based on player movement
    void HandleSwordVisibility(Vector2 playerVelocity)
    {
        // Always show sword if in combat mode
        if (isInCombat)
        {
            swordRenderer.enabled = true;
            return;
        }

        // If the sword is in the back position and moving left or right
        if (!isInCombat && sword.position == backPosition.position)
        {
            // If player is moving left or right, hide sword
            if (Mathf.Abs(playerVelocity.x) > 0.1f)
            {
                swordRenderer.enabled = false; // Hide sword
            }
            else
            {
                swordRenderer.enabled = true; // Show sword if stationary
            }
        }
        else
        {
            // If sword is in front or behind (up or down), always show it
            swordRenderer.enabled = true;
        }
    }
}
