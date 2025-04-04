using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2.5f;  // Movement speed (units per second)
    public float gridSize = 1f;   
    public float chaseRange = 2f; // Cell distance to begin chase
    private Vector3 targetPosition; // Target position for the enemy
    private bool isMoving = false;  // To prevent multiple movements at the same time
    private Transform player; // Reference to the player's transform

    public Tilemap tilemap; // Reference to the Tilemap in the scene

    void Start()
    {
        // Find the player object in the scene by tag
        player = GameObject.FindWithTag("Player").transform;

        // Start at the current position on the grid
        targetPosition = SnapToGrid(transform.position);
    }

    void Update()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If the player is within the chase range, move towards the player
        if (distanceToPlayer <= chaseRange * gridSize) // 2 cells = 64 pixels
        {
            MoveTowardsPlayer();
        }

        // Smooth movement towards the target position
        if (transform.position != targetPosition && !isMoving)
        {
            StartCoroutine(SmoothMoveToTarget(targetPosition));
        }
    }

    // Move towards the player's position (diagonal and straight movement)
    void MoveTowardsPlayer()
    {
        // Get direction towards the player in grid steps (can move diagonally)
        Vector3 direction = player.position - transform.position;

        // Normalize the direction and snap it to the grid
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            direction.x = Mathf.Sign(direction.x) * gridSize;
            direction.y = Mathf.Sign(direction.y) * gridSize * (Mathf.Abs(direction.y) / Mathf.Abs(direction.x)); // Proportional Y movement
        }
        else
        {
            direction.y = Mathf.Sign(direction.y) * gridSize;
            direction.x = Mathf.Sign(direction.x) * gridSize * (Mathf.Abs(direction.x) / Mathf.Abs(direction.y)); // Proportional X movement
        }

        Vector3 potentialTargetPosition = SnapToGrid(transform.position + direction);

        // Check if the target position is blocked (e.g., by a wall)
        if (IsBlocked(potentialTargetPosition))
        {
            // If blocked, stop moving (or implement an alternative behavior like finding a different path)
            return;
        }

        // Set the valid target position
        targetPosition = potentialTargetPosition;
    }

    // Snap to the nearest grid cell
    private Vector3 SnapToGrid(Vector3 position)
    {
        float snappedX = Mathf.Round(position.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(position.y / gridSize) * gridSize;
        return new Vector3(snappedX, snappedY, position.z);
    }

    // Smoothly move to the target position
    IEnumerator SmoothMoveToTarget(Vector3 target)
    {
        isMoving = true;

        // Smoothly move to target position
        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;  // Stop moving once the enemy reaches the target position
    }

    // Checks if the target position is blocked by any obstacle tiles (like a wall)
    bool IsBlocked(Vector3 position)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(position);  // Convert world position to tilemap cell position
        
        // Get the tile at the specified cell position
        TileBase tile = tilemap.GetTile(cellPosition);

        // If a tile exists, check if it is a type of obstacle (e.g., wall)
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
