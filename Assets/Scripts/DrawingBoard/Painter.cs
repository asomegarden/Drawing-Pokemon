using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class Painter : MonoBehaviour
{
    public RawImage drawingCanvas;
    public Color brushColor = Color.black;
    public float brushSize = 1.0f;

    private Texture2D texture;
    private bool isDrawing = false;
    private Vector2 lastMousePos;
    private Vector2 nullVector;

    public GameObject panel;

    void Start()
    {
        texture = new Texture2D((int)drawingCanvas.rectTransform.rect.width, (int)drawingCanvas.rectTransform.rect.height);
        texture.filterMode = FilterMode.Point;
        drawingCanvas.texture = texture;
        nullVector = new Vector2(-1, -1);
        lastMousePos = nullVector;

        ClearCanvas();
    }

    void Update()
    {
        if (!panel.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDrawing = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isDrawing = false;
                lastMousePos = nullVector;
            }
        }
        else
        {
            isDrawing = false; lastMousePos = nullVector;
        }

        if (isDrawing)
        {
            Vector2 mousePos = Input.mousePosition;
            if (lastMousePos != nullVector)
            {
                DrawLine(lastMousePos, mousePos);
            }
            lastMousePos = mousePos;
        }
    }

    void DrawLine(Vector2 start, Vector2 end)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(drawingCanvas.rectTransform, start, null, out Vector2 localStart);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(drawingCanvas.rectTransform, end, null, out Vector2 localEnd);

        int x0 = Mathf.FloorToInt(localStart.x + drawingCanvas.rectTransform.rect.width / 2);
        int y0 = Mathf.FloorToInt(localStart.y + drawingCanvas.rectTransform.rect.height / 2);
        int x1 = Mathf.FloorToInt(localEnd.x + drawingCanvas.rectTransform.rect.width / 2);
        int y1 = Mathf.FloorToInt(localEnd.y + drawingCanvas.rectTransform.rect.height / 2);

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            DrawBrush(x0, y0);

            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy) { err -= dy; x0 += sx; }
            if (e2 < dx) { err += dx; y0 += sy; }
        }

        texture.Apply();
    }
    void DrawBrush(int x, int y)
    {
        for (int i = -Mathf.FloorToInt(brushSize / 2); i <= Mathf.FloorToInt(brushSize / 2); i++)
        {
            for (int j = -Mathf.FloorToInt(brushSize / 2); j <= Mathf.FloorToInt(brushSize / 2); j++)
            {
                if (x + i >= 0 && x + i < texture.width && y + j >= 0 && y + j < texture.height)
                {
                    texture.SetPixel(x + i, y + j, brushColor);
                }
            }
        }
    }
    public void ClearCanvas()
    {
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, Color.white);
            }
        }
        texture.Apply();
    }
}
