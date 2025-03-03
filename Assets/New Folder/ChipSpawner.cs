using UnityEngine;
using UnityEngine.EventSystems;

public class ChipSpawner : MonoBehaviour, IPointerDownHandler
{
    public GameObject chipPrefab;  // The chip prefab to spawn
    private GameObject currentChip;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;  // Get the main camera
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Convert the screen position to world position when the pointer is clicked
        Vector3 spawnPosition = mainCamera.ScreenToWorldPoint(eventData.position);
        spawnPosition.z = 0;  // Set z to 0 to keep it in 2D space
        SpawnChip(spawnPosition);
    }

    void SpawnChip(Vector3 spawnPosition)
    {
        // Instantiate the chip and set it in the world space
        currentChip = Instantiate(chipPrefab, spawnPosition, Quaternion.identity);

        // Set the chip to be above the UI (this ensures it renders in front)
        currentChip.layer = LayerMask.NameToLayer("Default");  // Ensure it's on the Default layer or the appropriate one above UI

        Chip chipScript = currentChip.GetComponent<Chip>();
        chipScript.StartDraggingImmediately();  // This triggers dragging behavior immediately
    }
}
