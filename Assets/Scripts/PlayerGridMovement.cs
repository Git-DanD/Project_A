using UnityEngine;
using System.Collections;

public class PlayerGridMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed (not used for movement steps)
    public float gridSize = 1f / 3f; // 32 pixels per step (PPU 96)
    
    private Vector3 targetPosition; 
    private bool isMoving = false;

    void Start()
    {
        targetPosition = transform.position; // Start on grid
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
        float moveDuration = gridSize / moveSpeed; // Duration per step

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Ensure exact position
        isMoving = false;
    }
}
