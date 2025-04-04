using UnityEngine;
using System.Collections;

public class PlayerGridMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Controls movement speed per step
    public float gridSize = 1f / 3f; // 32 pixels per step (PPU 96)

    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        // Snap the player's starting position to the grid
        transform.position = SnapToGrid(transform.position);
        targetPosition = transform.position;
    }

    void Update()
    {
        if (!isMoving)
        {
            if (Input.GetKey(KeyCode.W)) StartCoroutine(Move(Vector3.up));
            if (Input.GetKey(KeyCode.S)) StartCoroutine(Move(Vector3.down));
            if (Input.GetKey(KeyCode.A)) StartCoroutine(Move(Vector3.left));
            if (Input.GetKey(KeyCode.D)) StartCoroutine(Move(Vector3.right));
        }
    }

    IEnumerator Move(Vector3 direction)
    {
        isMoving = true;
        targetPosition = transform.position + direction * gridSize;

        float elapsedTime = 0f;
        float moveDuration = gridSize / moveSpeed; // Time to complete a move

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = SnapToGrid(targetPosition); // Snap to grid after moving
        isMoving = false;
    }

    // Ensures the player is always centered on the grid
    private Vector3 SnapToGrid(Vector3 position)
    {
        float snappedX = Mathf.Round(position.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(position.y / gridSize) * gridSize;
        return new Vector3(snappedX, snappedY, position.z);
    }
}
