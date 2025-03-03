using UnityEngine;
using System.Collections.Generic;

public class DrawInGameSpace : MonoBehaviour
{
    public Color drawColor = Color.black;  // The color of the line
    public float lineWidth = 0.1f;  // Width of the drawing line
    private List<LineRenderer> lineRenderers = new List<LineRenderer>();  // Store all the LineRenderers for each stroke
    private List<Vector3> linePositions = new List<Vector3>();  // Store the points of the current line

    private bool isDrawing = false;  // Is drawing mode enabled?
    private bool canDraw = false;  // Determines whether the player can draw or not
    private LineRenderer currentLineRenderer;  // The current LineRenderer being drawn on

    void Start()
    {
        // No need to create LineRenderers here, it's done when drawing starts
    }

    void Update()
    {
        if (canDraw && Input.GetMouseButton(0))  // While mouse is held down, and if drawing is allowed
        {
            Vector3 worldPosition = GetMouseWorldPosition();
            if (isDrawing)
            {
                Draw(worldPosition);
            }
            else
            {
                StartDrawing(worldPosition);
            }
        }

        if (Input.GetMouseButtonUp(0))  // When mouse is released
        {
            EndDrawing();
        }
    }

    // Convert mouse position to world space
    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.forward, Vector3.zero);  // Assuming you're drawing on the XY plane (z = 0)
        float distance;
        if (groundPlane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }

    // Start drawing the line
    void StartDrawing(Vector3 startPosition)
    {
        if (!canDraw) return;  // If drawing isn't toggled on, don't start drawing

        linePositions.Clear();  // Clear any previous points when starting a new line
        linePositions.Add(startPosition);  // Add the start position
        currentLineRenderer.positionCount = 1;  // Start with 1 point
        currentLineRenderer.SetPosition(0, startPosition);  // Set the start point
        isDrawing = true;
    }

    // Draw the line as the mouse moves
    void Draw(Vector3 currentPosition)
    {
        // Only draw if the mouse moves significantly
        if (Vector3.Distance(linePositions[linePositions.Count - 1], currentPosition) > 0.1f)
        {
            linePositions.Add(currentPosition);  // Add the new point
            currentLineRenderer.positionCount = linePositions.Count;  // Update the position count
            currentLineRenderer.SetPosition(linePositions.Count - 1, currentPosition);  // Update the last point
        }
    }

    // End the drawing when the mouse is lifted
    void EndDrawing()
    {
        isDrawing = false;  // Finish the current drawing
        linePositions.Clear();  // Clear the line positions to prepare for the next stroke
        CreateNewLineRenderer();  // Prepare for the next stroke
    }
    public void PauseDrawingMode()
    {
        canDraw = false;
    }
    // Create a new LineRenderer for a new stroke
    void CreateNewLineRenderer()
    {
        if (!canDraw) return;  // Only create a new LineRenderer if drawing is toggled on

        // Create a new LineRenderer for each new stroke
        GameObject newLineObject = new GameObject("LineRenderer_" + lineRenderers.Count);
        LineRenderer newLineRenderer = newLineObject.AddComponent<LineRenderer>();
        newLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        newLineRenderer.startColor = drawColor;
        newLineRenderer.endColor = drawColor;
        newLineRenderer.startWidth = lineWidth;
        newLineRenderer.endWidth = lineWidth;
        newLineRenderer.positionCount = 0;  // Start with no points
        lineRenderers.Add(newLineRenderer);  // Add this LineRenderer to the list

        // Set this new LineRenderer as the current one to draw on
        currentLineRenderer = newLineRenderer;
    }

    // Clear all drawings (can be bound to a button)
    public void ClearAllDrawings()
    {
        foreach (LineRenderer lr in lineRenderers)
        {
            Destroy(lr.gameObject);  // Destroy all previous line objects
        }
        lineRenderers.Clear();
    }

    // Toggle the drawing mode on or off
    public void ToggleDrawingMode()
    {
        canDraw = !canDraw;  // Toggle drawing mode on or off
    }
}
