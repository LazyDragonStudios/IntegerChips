using UnityEngine;
using UnityEngine;

public class Chip : MonoBehaviour
{
    public ChipType chipType = ChipType.Red;  // Default type is Red
    public bool isImmobile = false;  // Flag to check if chip should be immobile
    private SpriteRenderer spriteRenderer;
    private Vector3 offset;  // The offset between the chip and mouse position when dragging
    private bool isDragging = false;
    // To store the paired chip when a collision occurs
    private Chip pairedChip = null;

    private Camera mainCamera;  // Reference to the camera to use for raycasting

    private float lastTapTime = 0f;  // The time of the last tap
    private float doubleTapTimeLimit = 0.3f;  // Time window for detecting a double-tap (in seconds)
    private Collider2D chipCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;  // Get the main camera for raycasting
        chipCollider = GetComponent<Collider2D>();  // Reference to the collider
        UpdateChipColor();  // Initialize color based on type
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Detect mouse button press
        {
            // Perform raycast to detect if the mouse click is over this chip
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // If the chip was clicked, check for double-tap
                bool isToggle = HandleDoubleTap();
                if (!isToggle && !isDragging)
                {
                    StartDragging();
                }
            }
        }

        if (isDragging)  // While holding the mouse button down
        {
            DragChip();
        }

        if (Input.GetMouseButtonUp(0))  // Detect mouse release
        {
            EndDragging();
        }
    }

    private void StartDragging()
    {
        if (!isImmobile)
        {
            isDragging = true;
            chipCollider.enabled = false;  // Disable collider while dragging
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);  // Get the mouse position in world space
            mousePosition.z = 0;  // Keep it in 2D space
            offset = transform.position - mousePosition;  // Calculate offset for smooth dragging
        }
    }

    // Drag the chip based on mouse position
    // Drag the chip based on mouse position
    private void DragChip()
    {
        if (!isImmobile)
        {
            isDragging = true;
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);  // Get the mouse position in world space
            mousePosition.z = 0;  // Keep it in 2D space
            transform.position = mousePosition + offset;  // Update chip position

            // If this chip has a paired chip, move it along with this one
            if (pairedChip != null)
            {
                pairedChip.transform.position = transform.position;  // Move the paired chip
            }
        }
    }

    // End dragging the chip
    private void EndDragging()
    {
        isDragging = false;
        GetComponent<Collider2D>().enabled = true;  // Re-enable collision after dropping
    }

    // Handle the logic for double-tapping
    private bool HandleDoubleTap()
    {
        if (Time.time - lastTapTime <= doubleTapTimeLimit)
        {
            // Double-tap detected within the time limit, change color
            ToggleChipColor();
            lastTapTime = Time.time;
            return true;
        }

        lastTapTime = Time.time;
        return false;
    }

    // Toggle the chip's color between red and yellow
    private void ToggleChipColor()
    {
        if (chipType == ChipType.Red)
        {
            chipType = ChipType.Yellow;
        }
        else
        {
            chipType = ChipType.Red;
        }

        UpdateChipColor();  // Update the color after toggling
    }

    // Update the chip's color based on its type
    private void UpdateChipColor()
    {
        if (chipType == ChipType.Red)
        {
            spriteRenderer.color = Color.red;
        }
        else if (chipType == ChipType.Yellow)
        {
            spriteRenderer.color = Color.yellow;
        }
    }
    public void StartDraggingImmediately()
    {
        // Start dragging immediately after the chip is spawned
        isDragging = true;
        GetComponent<Collider2D>().enabled = false;  // Disable collision while dragging
    }
    // Detect collision with other chips and change color
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Can you see this?");
        // Check if the other object is a chip
        Chip otherChip = collision.gameObject.GetComponent<Chip>();
        if (otherChip != null)
        {
            // Check if this chip is red and the other one is yellow, or vice versa
            if ((chipType == ChipType.Red && otherChip.chipType == ChipType.Yellow) ||
                (chipType == ChipType.Yellow && otherChip.chipType == ChipType.Red))
            {
                // Darken both chips' colors
                spriteRenderer.color = DarkenColor(spriteRenderer.color);
                otherChip.spriteRenderer.color = DarkenColor(otherChip.spriteRenderer.color);

                // Link the two chips together
                pairedChip = otherChip;
                otherChip.pairedChip = this;  // Make the other chip aware of this one
            }
        }
    }

    // Darken the color by multiplying each component (R, G, B) by a factor
    private Color DarkenColor(Color originalColor)
    {
        // Factor to darken the color (lower than 1 will darken it)
        float darkenFactor = 0.5f;  // You can adjust this value for more or less darkening

        // Darken each color component
        return new Color(originalColor.r * darkenFactor, originalColor.g * darkenFactor, originalColor.b * darkenFactor);
    }

}

public enum ChipType
{
    Red,
    Yellow
}

