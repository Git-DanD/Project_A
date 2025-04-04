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
    // Get raw direction toward the player
    Vector3 direction = player.position - transform.position;

    // Determine dominant direction and normalize to grid size
    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
    {
        direction.x = Mathf.Sign(direction.x) * gridSize;
        direction.y = Mathf.Sign(direction.y) * gridSize * (Mathf.Abs(direction.y) / Mathf.Abs(direction.x));
    }
    else
    {
        direction.y = Mathf.Sign(direction.y) * gridSize;
        direction.x = Mathf.Sign(direction.x) * gridSize * (Mathf.Abs(direction.x) / Mathf.Abs(direction.y));
    }

    Vector3 snappedDirection = new Vector3(
        Mathf.Round(direction.x / gridSize) * gridSize,
        Mathf.Round(direction.y / gridSize) * gridSize,
        0
    );

    Vector3 potentialTargetPosition = SnapToGrid(transform.position + snappedDirection);

    bool isDiagonal = snappedDirection.x != 0 && snappedDirection.y != 0;

    if (isDiagonal)
    {
        Vector3 horizontal = SnapToGrid(transform.position + new Vector3(snappedDirection.x, 0, 0));
        Vector3 vertical = SnapToGrid(transform.position + new Vector3(0, snappedDirection.y, 0));

        if (IsBlocked(horizontal) || IsBlocked(vertical))
        {
            return; // Cancel diagonal move if either axis is blocked
        }
    }

    // Block full movement if target is obstructed
    if (IsBlocked(potentialTargetPosition))
    {
        return;
    }

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
