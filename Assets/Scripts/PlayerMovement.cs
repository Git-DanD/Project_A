using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movement;
    public float speed = 5f; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Get the Rigidbody2D component
    }

    void Update()
    {
        // Get movement input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Prevent diagonal movement (optional)
        if (movement.x != 0) movement.y = 0;
    }

    void FixedUpdate()
    {
        // Move the player
        rb.linearVelocity = movement * speed;
    }
}
