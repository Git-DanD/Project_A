using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

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
            if (Input.GetKey(KeyCode.W)) TryMove(Vector3.up);     // Move up
            if (Input.GetKey(KeyCode.S)) TryMove(Vector3.down);   // Move down
            if (Input.GetKey(KeyCode.A)) TryMove(Vector3.left);   // Move left
            if (Input.GetKey(KeyCode.D)) TryMove(Vector3.right);  // Move right
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
        Vector3 target = SnapToGrid(transform.position + direction * gridSize);
        
        // Check if the target position is blocked by an obstacle
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
            // You can check based on tags or properties for the obstacle tiles.
            // For now, let's assume we tag obstacle tiles as "Obstacle" in the Inspector
            if (tilemap.GetColliderType(cellPosition) != Tile.ColliderType.None)
            {
                return true; // Blocked if tile has a collider or other conditions
            }
        }

        return false;  // Tile is not an obstacle or there is no tile
    }
}
