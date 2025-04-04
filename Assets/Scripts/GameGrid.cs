using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    public int width = 10;  // Number of cells in the X direction
    public int height = 10; // Number of cells in the Y direction
    public float gridSize = 1f;  // Size of each grid cell
    public Material gridMaterial; // Material for grid lines
    public Vector3 gridStartPos = new Vector3(-10, -10, 0); // Starting position of the grid

    private void Start()
    {
        DrawGrid();
    }

    void DrawGrid()
    {
        GameObject gridContainer = new GameObject("Grid");
        
        for (int i = 0; i <= width; i++)
        {
            // Vertical lines
            GameObject verticalLine = new GameObject("Vertical Line " + i);
            verticalLine.transform.parent = gridContainer.transform;
            LineRenderer lrVertical = verticalLine.AddComponent<LineRenderer>();
            lrVertical.positionCount = 2;
            lrVertical.SetPosition(0, gridStartPos + new Vector3(i * gridSize, 0, 0));
            lrVertical.SetPosition(1, gridStartPos + new Vector3(i * gridSize, height * gridSize, 0));
            lrVertical.material = gridMaterial;
            lrVertical.startWidth = 0.05f;
            lrVertical.endWidth = 0.05f;

            // Horizontal lines
            GameObject horizontalLine = new GameObject("Horizontal Line " + i);
            horizontalLine.transform.parent = gridContainer.transform;
            LineRenderer lrHorizontal = horizontalLine.AddComponent<LineRenderer>();
            lrHorizontal.positionCount = 2;
            lrHorizontal.SetPosition(0, gridStartPos + new Vector3(0, i * gridSize, 0));
            lrHorizontal.SetPosition(1, gridStartPos + new Vector3(width * gridSize, i * gridSize, 0));
            lrHorizontal.material = gridMaterial;
            lrHorizontal.startWidth = 0.05f;
            lrHorizontal.endWidth = 0.05f;
        }
    }
}
