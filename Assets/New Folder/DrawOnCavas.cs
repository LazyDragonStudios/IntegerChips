using UnityEngine;
using UnityEngine.UI;

public class DrawOnCavas : MonoBehaviour
{
    public RawImage rawImage;  // The UI RawImage component to display the texture
    private Texture2D texture;
    private Color[] colorArray;
    private bool isDrawing = false;

    public Color drawColor = Color.black;  // The color of the pen
    public float brushSize = 5f;  // Size of the drawing brush

    void Start()
    {
        texture = new Texture2D((int)rawImage.rectTransform.rect.width, (int)rawImage.rectTransform.rect.height);
        colorArray = new Color[texture.width * texture.height];

        // Initialize the texture with white pixels
        for (int i = 0; i < colorArray.Length; i++)
        {
            colorArray[i] = Color.white;
        }

        texture.SetPixels(colorArray);
        texture.Apply();
        rawImage.texture = texture;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))  // Mouse or touch input
        {
            Vector2 localPos = GetLocalMousePosition();
            if (isDrawing || IsPointerInsideCanvas(localPos))
            {
                isDrawing = true;
                Draw(localPos);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
        }
    }

    private Vector2 GetLocalMousePosition()
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, Input.mousePosition, Camera.main, out localPos);
        return localPos;
    }

    private bool IsPointerInsideCanvas(Vector2 localPos)
    {
        RectTransform rectTransform = rawImage.rectTransform;
        return rectTransform.rect.Contains(localPos);
    }

    void Draw(Vector2 localPos)
    {
        // Convert local position to texture space
        Vector2 texPos = new Vector2(localPos.x + rawImage.rectTransform.rect.width / 2, localPos.y + rawImage.rectTransform.rect.height / 2);

        // Get the texture pixel position
        int x = Mathf.FloorToInt(texPos.x);
        int y = Mathf.FloorToInt(texPos.y);

        // Ensure the drawing is within bounds of the texture
        if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
        {
            texture.SetPixel(x, y, drawColor);
            texture.Apply();
        }
    }

    public void ClearDrawing()
    {
        for (int i = 0; i < colorArray.Length; i++)
        {
            colorArray[i] = Color.white;
        }

        texture.SetPixels(colorArray);
        texture.Apply();
    }
}
