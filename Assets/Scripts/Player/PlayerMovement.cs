using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class PlayerGridMovement : MonoBehaviour
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
        // Only allow movement if the player is not currently moving
        if (!isMoving)
        {
            // Get the movement direction from input
            Vector3 movementDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) movementDirection += Vector3.up;     // Move up
            if (Input.GetKey(KeyCode.S)) movementDirection += Vector3.down;   // Move down
            if (Input.GetKey(KeyCode.A)) movementDirection += Vector3.left;   // Move left
            if (Input.GetKey(KeyCode.D)) movementDirection += Vector3.right;  // Move right

            // If there is any movement direction input, try to move
            if (movementDirection != Vector3.zero)
            {
                TryMove(movementDirection.normalized);  // Normalize to maintain consistent speed
            }
        }

        // Smooth movement towards the target position
        if (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    // Tries to move in the given direction if the position is not blocked
    void TryMove(Vector3 direction)
    {
        // Snap to grid for the target position
        Vector3 target = SnapToGrid(transform.position + direction * gridSize);

        // Check if movement is diagonal (both x and y directions)
        bool isDiagonal = direction.x != 0 && direction.y != 0;

        if (isDiagonal)
        {
            // Check if the horizontal (left/right) and vertical (up/down) directions are both clear
            Vector3 horizontalTarget = SnapToGrid(transform.position + new Vector3(direction.x, 0, 0) * gridSize);
            Vector3 verticalTarget = SnapToGrid(transform.position + new Vector3(0, direction.y, 0) * gridSize);

            // If either direction is blocked, don't allow diagonal movement
            if (IsBlocked(horizontalTarget) || IsBlocked(verticalTarget))
            {
                return;
            }
        }

        // If the target position is not blocked, proceed with the movement
        if (!IsBlocked(target))
        {
            targetPosition = target;
            isMoving = true;  // Player is now moving
            StartCoroutine(StopMoving());  // Call StopMoving coroutine after movement
        }
    }

    // Coroutine to stop movement after reaching the target position
    IEnumerator StopMoving()
    {
        // Wait until the player has reached the target position
        yield return new WaitUntil(() => transform.position == targetPosition);
        isMoving = false;  // Stop moving once the player has reached the target position
    }

    // Ensures the player is always centered on the grid (snaps to nearest grid cell)
    private Vector3 SnapToGrid(Vector3 position)
    {
        float snappedX = Mathf.Round(position.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(position.y / gridSize) * gridSize;
        return new Vector3(snappedX, snappedY, position.z);
    }

    // Checks if the target position is blocked by any obstacle tiles
    bool IsBlocked(Vector3 position)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(position);  // Convert world position to tilemap cell position
        
        // Get the tile at the specified cell position
        TileBase tile = tilemap.GetTile(cellPosition);

        // If a tile exists, check if it is a type of obstacle
        if (tile != null)
        {
            // Check if the tile has a collider (indicating it's an obstacle)
            if (tilemap.GetColliderType(cellPosition) != Tile.ColliderType.None)
            {
                return true; // Blocked if tile has a collider
            }
        }

        return false;  // Tile is not an obstacle or there is no tile
    }
}
