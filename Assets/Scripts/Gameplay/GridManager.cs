using UnityEngine;

public class GridManager : MonoBehaviour
{
    public float gridSize = 1f; // Size of each grid cell
    public int width = 10; // Number of cells in the X direction
    public int height = 10; // Number of cells in the Y direction

    public Vector3 GridToWorldPosition(int x, int y)
    {
        return new Vector3(x * gridSize, y * gridSize, 0);
    }

    public Vector3 SnapToGrid(Vector3 worldPosition)
    {
        float snappedX = Mathf.Round(worldPosition.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(worldPosition.y / gridSize) * gridSize;
        return new Vector3(snappedX, snappedY, worldPosition.z);
    }
}
