using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GridManager gridManager; // Reference to the GridManager

    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        targetPosition = transform.position; // Set initial target to player's starting position
    }

    void Update()
    {
        if (!isMoving)
        {
            Vector3 direction = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) direction = Vector3.up;
            if (Input.GetKey(KeyCode.S)) direction = Vector3.down;
            if (Input.GetKey(KeyCode.A)) direction = Vector3.left;
            if (Input.GetKey(KeyCode.D)) direction = Vector3.right;

            if (direction != Vector3.zero)
            {
                TryMove(direction);
            }
        }

        if (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    // Try to move to the new target position
    void TryMove(Vector3 direction)
    {
        Vector3 newTargetPosition = gridManager.SnapToGrid(transform.position + direction);

        // Only move if the target position is different
        if (newTargetPosition != transform.position)
        {
            targetPosition = newTargetPosition;
            isMoving = true;
            StartCoroutine(StopMoving());
        }
    }

    // Coroutine to stop the player from moving once they reach the target position
    System.Collections.IEnumerator StopMoving()
    {
        yield return new WaitUntil(() => transform.position == targetPosition);
        isMoving = false;
    }
}
