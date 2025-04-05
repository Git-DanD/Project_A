using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float gridSize = 1f;

    // Method to convert world position to grid cell position
    public Vector3 GetGridPosition(Vector3 worldPosition)
    {
        // Convert world position to grid position based on grid size
        float x = Mathf.Floor(worldPosition.x / gridSize) * gridSize;
        float y = Mathf.Floor(worldPosition.y / gridSize) * gridSize;

        return new Vector3(x, y, worldPosition.z);
    }
}
