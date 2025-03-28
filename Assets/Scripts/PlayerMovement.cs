using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    private Rigidbody2D rb; // Reference to Rigidbody2D
    private Vector2 moveDirection; // Movement direction

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovement();
    }

    // Handle player movement
    void HandleMovement()
    {
        // Get input for movement
        float moveX = Input.GetAxisRaw("Horizontal"); // -1, 0, 1
        float moveY = Input.GetAxisRaw("Vertical");   // -1, 0, 1

        // Diagonal movement is possible as moveX and moveY can be combined
        moveDirection = new Vector2(moveX, moveY).normalized; // Normalized to avoid faster diagonal movement

        // Apply the movement to the Rigidbody2D
        rb.linearVelocity = moveDirection * moveSpeed;
    }
}
