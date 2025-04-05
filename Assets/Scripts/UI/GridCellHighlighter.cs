using UnityEngine;

public class GridCellHighlighter : MonoBehaviour
{
    public GameObject highlightPrefab;  // Reference to the highlight prefab
    private GameObject currentHighlight;  // Current highlight object
    private GridManager gridManager;  // Reference to GridManager
    private Vector3 lastGridPos = Vector3.zero;  // To keep track of the last grid position

    void Start()
    {
        // Find the GridManager in the scene
        gridManager = Object.FindFirstObjectByType<GridManager>();

        // Ensure the highlight prefab is hidden initially
        highlightPrefab.SetActive(false);
    }

    void Update()
    {
        // Get the mouse position in world space
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;  // Ensure the z-axis is zero for 2D grid

        // Apply the offset of 0.5 units to align with the Tilemap grid
        mouseWorldPos.x += 0.5f;
        mouseWorldPos.y += 0.5f;

        // Use GridManager to convert the world position to grid position
        Vector3 gridPos = gridManager.GetGridPosition(mouseWorldPos);

        // Calculate the center of the grid cell with the same 0.5 units offset
        Vector3 gridCenterPos = new Vector3(
            Mathf.Floor(gridPos.x / gridManager.gridSize) * gridManager.gridSize + gridManager.gridSize / 2f,
            Mathf.Floor(gridPos.y / gridManager.gridSize) * gridManager.gridSize + gridManager.gridSize / 2f,
            0);

        // Move the highlight prefab back by 0.5 units
        gridCenterPos -= new Vector3(0.5f, 0.5f, 0);

        // If the grid position has changed, update the highlight
        if (gridCenterPos != lastGridPos)
        {
            // If there's no current highlight, create one
            if (currentHighlight == null)
            {
                currentHighlight = Instantiate(highlightPrefab, gridCenterPos, Quaternion.identity);
                currentHighlight.SetActive(true); // Enable it when needed
            }
            else
            {
                // Update the position of the highlight to the current grid cell
                currentHighlight.transform.position = gridCenterPos;
            }

            // Update the last grid position
            lastGridPos = gridCenterPos;
        }
    }
}
