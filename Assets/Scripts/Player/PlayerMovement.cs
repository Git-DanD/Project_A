using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;   // Movement speed (units per second)
    public float gridSize = 1f;    // Grid size (one cell = 1 unit)
    private Vector3 targetPosition; // Target position (next grid cell)
    private bool isMoving = false;  // To prevent multiple movements at the same time

    public Tilemap tilemap;  // Reference to the Tilemap in the scene

    void Start()
    {
        // Snap the player's starting position to the grid
        transform.position = SnapToGrid(transform.position);
        targetPosition = transform.position;  // Set initial target to starting position
    }

    void Update()
    {
        // Get movement direction based on input keys (WASD)
        Vector3 movementDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) movementDirection += Vector3.up;
        if (Input.GetKey(KeyCode.S)) movementDirection += Vector3.down;
        if (Input.GetKey(KeyCode.A)) movementDirection += Vector3.left;
        if (Input.GetKey(KeyCode.D)) movementDirection += Vector3.right;

        // Only attempt to start a new move if the player isn't already moving
        if (movementDirection != Vector3.zero && !isMoving)
        {
            TryMove(movementDirection.normalized); // Normalize direction to maintain consistent speed
        }

        // Move continuously toward target position until it is reached
        if (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void TryMove(Vector3 direction)
    {
        // Get the current position snapped to grid
        Vector3 currentGridPos = SnapToGrid(transform.position);
        // Calculate the target position (next grid cell in the movement direction)
        Vector3 desiredPosition = currentGridPos + direction * gridSize;

        // Handle diagonal movement (ensure that both x and y directions are considered together)
        if (direction.x != 0 && direction.y != 0)
        {
            // For diagonal movement, we want to use the same target cell for both axes
            Vector3 target = SnapToGrid(currentGridPos + direction * gridSize);

            // If the target position is blocked, don't move
            if (IsBlocked(target))
            {
                return; // Prevent diagonal movement if blocked
            }

            // Allow movement to the target position if it's not blocked
            targetPosition = target;
            isMoving = true;
            StartCoroutine(MoveCoroutine());
        }
        else
        {
            // For non-diagonal movement (only x or y axis), we just check the single direction
            if (!IsBlocked(desiredPosition))
            {
                targetPosition = desiredPosition;
                isMoving = true;
                StartCoroutine(MoveCoroutine());
            }
        }
    }

    IEnumerator MoveCoroutine()
    {
        // Wait until the player has reached the target position
        yield return new WaitUntil(() => transform.position == targetPosition);
        isMoving = false;  // Stop moving once the player has reached the target position
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        // Snap position to the nearest grid cell
        float snappedX = Mathf.Round(position.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(position.y / gridSize) * gridSize;
        return new Vector3(snappedX, snappedY, position.z);
    }

    bool IsBlocked(Vector3 worldPosition)
    {
        // Convert world position to tilemap grid position
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

        // Get the tile at the specified position
        TileBase tile = tilemap.GetTile(cellPosition);

        if (tile == null)
            return false; // No tile = no block

        // Check if the tile has a collider (i.e., it's an obstacle like a wall)
        Tile.ColliderType collider = tilemap.GetColliderType(cellPosition);
        return collider != Tile.ColliderType.None;
    }
}
