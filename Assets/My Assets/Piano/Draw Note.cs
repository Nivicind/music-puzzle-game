using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawNote : MonoBehaviour
{
    public Tilemap tilemap; // Reference to the Tilemap in your scene
    public Tile bridgeTile; // Tile to use for the bridge
    public float drawInterval = 0.05f; // Time between placing each tile
    public Collider2D drawArea; // Reference to the large box collider
    public GameObject pedestalInterface; // Reference to the Pedestal Interface GameObject

    private bool isDrawing = false; // Whether the bridge is being drawn
    private bool canDraw = true; // Whether a bridge can be drawn
    private Vector3Int currentCellPosition; // The current position of the last drawn tile
    private float nextDrawTime; // Time for the next tile to be drawn

    void Update()
    {
        // Ensure the Pedestal Interface is active before allowing input
        if (pedestalInterface != null && pedestalInterface.activeSelf)
        {
            // Start drawing when "A" key is pressed
            if (Input.GetKeyDown(KeyCode.A) && canDraw)
            {
                StartDrawing();
            }

            // Stop drawing when "A" key is released
            if (Input.GetKeyUp(KeyCode.A))
            {
                StopDrawing();
            }

            // Draw the bridge incrementally while "A" is held
            if (isDrawing)
            {
                DrawBridge();
            }

            // Reset the bridge when "Backspace" is pressed
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                ResetBridge();
            }
        }
    }

    void StartDrawing()
    {
        isDrawing = true;
        canDraw = false;

        // Get the starting cell position on the Tilemap
        Vector3 worldPosition = transform.position;
        currentCellPosition = tilemap.WorldToCell(worldPosition);
        nextDrawTime = Time.time;
    }

    void DrawBridge()
    {
        if (Time.time < nextDrawTime) return;

        // Check if the next tile is within the draw area
        Vector3 worldPosition = tilemap.CellToWorld(currentCellPosition);
        if (drawArea.bounds.Contains(worldPosition))
        {
            // Place the tile at the current cell position
            tilemap.SetTile(currentCellPosition, bridgeTile);

            // Move to the next cell to the right
            currentCellPosition += new Vector3Int(1, 0, 0);

            // Update the next draw time
            nextDrawTime = Time.time + drawInterval;
        }
        else
        {
            // Stop drawing if we exceed the draw area
            StopDrawing();
        }
    }

    void StopDrawing()
    {
        isDrawing = false;
    }

    void ResetBridge()
    {
        // Clear the tilemap and allow drawing again
        tilemap.ClearAllTiles();
        canDraw = true;
    }
}
