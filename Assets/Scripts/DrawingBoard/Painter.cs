using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using Unity.Mathematics;

public class Painter : MonoBehaviour
{
    public RawImage drawingCanvas;
    public Color brushColor = Color.black;
    public float brushSize = 1.0f;

    private Texture2D texture;
    private bool isDrawing = false;
    private bool isFilling = false;
    private Vector2 lastMousePos;
    private Vector2 nullVector;

    private float maxBrushSize=5.0f;
    private float minBrushSize=1.0f;

    public GameObject panel;
    public UnityEngine.UI.Button[] buttons;
    public UnityEngine.UI.Button[] colorButtons;
    public UnityEngine.UI.Slider sizeSlider;
    public UnityEngine.UI.Slider[] colorSlider;

    private int[][] colorPreset;

    void Start()
    {
        texture = new Texture2D((int)drawingCanvas.rectTransform.rect.width, (int)drawingCanvas.rectTransform.rect.height);
        texture.filterMode = FilterMode.Point;
        drawingCanvas.texture = texture;
        nullVector = new Vector2(-1, -1);
        lastMousePos = nullVector;

        ClearCanvas();

        buttons[0].onClick.AddListener(OnEraserClick);
        buttons[1].onClick.AddListener(OnPenClick);
        buttons[2].onClick.AddListener(OnPaintClick);
        buttons[3].onClick.AddListener(SaveDrawingToTexture);
        buttons[4].onClick.AddListener(LoadDrawing);
        buttons[5].onClick.AddListener(OnFinishClick);
        #region colprPreset
        for (int i = 0; i < colorButtons.Length; i++)
        {
            colorButtons[i].onClick.AddListener(OnColorClick);
        }
        colorPreset = new int[16][];
        colorPreset[0] = new int[] { 0, 0, 0 };
        colorPreset[1] = new int[] { 127, 127, 127 };
        colorPreset[2] = new int[] { 237, 28, 36 };
        colorPreset[3] = new int[] { 255, 127, 39 };
        colorPreset[4] = new int[] { 255, 242, 0 };
        colorPreset[5] = new int[] { 34, 177, 76 };
        colorPreset[6] = new int[] { 0, 162, 232 };
        colorPreset[7] = new int[] { 63, 72, 204 };
        colorPreset[8] = new int[] { 255, 255, 255 };
        colorPreset[9] = new int[] { 185, 122, 87 };
        colorPreset[10] = new int[] { 255, 174, 201 };
        colorPreset[11] = new int[] { 255, 201, 14 };
        colorPreset[12] = new int[] { 239, 228, 176 };
        colorPreset[13] = new int[] { 181, 230, 29 };
        colorPreset[14] = new int[] { 112, 146, 190 };
        colorPreset[15] = new int[] { 163, 73, 164 };
        #endregion
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
            brushSize =  (maxBrushSize-minBrushSize)*sizeSlider.value + minBrushSize;
            sizeSlider.value = (brushSize - minBrushSize) / (maxBrushSize - minBrushSize);
            SetColor();
        }

        if (isDrawing)
        {
            Vector2 mousePos = Input.mousePosition;
            if (lastMousePos != nullVector)
            {
                DrawLine(lastMousePos, mousePos);
                FillColor();
            }
            lastMousePos = mousePos;
        }
    }

    void DrawLine(Vector2 start, Vector2 end)
    {
        if (isFilling) return;
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
    void FillColor()
    {
        if (!isFilling) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Rect rect = drawingCanvas.rectTransform.rect;
        if (rect.Contains(mousePos))
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(drawingCanvas.rectTransform, mousePos, null, out localPoint);
            int x = (int)mousePos.x;
            int y = (int)mousePos.y;
            FloodFill(x, y, texture.GetPixel(x+80, y+80), brushColor);
            texture.Apply();
        }
    }
    void FloodFill(int startX, int startY, Color targetColor, Color fillColor)
    {
        int width = texture.width;
        int height = texture.height;
        int[,] visited = new int[width, height];

        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        stack.Push(new Vector2Int(startX+80, startY+80));

        while (stack.Count > 0)
        {
            Vector2Int currentPosition = stack.Pop();
            int x = currentPosition.x;
            int y = currentPosition.y;

            if (x < 0 || x >= width || y < 0 || y >= height || visited[x, y] == 1 || texture.GetPixel(x, y) != targetColor)
                continue;

            texture.SetPixel(x, y, fillColor);
            visited[x, y] = 1;

            stack.Push(new Vector2Int(x - 1, y));
            stack.Push(new Vector2Int(x + 1, y));
            stack.Push(new Vector2Int(x, y - 1));
            stack.Push(new Vector2Int(x, y + 1));
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
    private void OnEraserClick()
    {
        isFilling = false;
        brushColor = Color.white;
        brushSize = maxBrushSize;
        colorSlider[0].value = brushColor.r;
        colorSlider[1].value = brushColor.g;
        colorSlider[2].value = brushColor.b;
        sizeSlider.value = (brushSize - minBrushSize) / (maxBrushSize - minBrushSize);
    }
    private void OnPenClick()
    {
        isFilling = false;
        brushSize = minBrushSize;
        brushColor = Color.black;
        colorSlider[0].value = brushColor.r;
        colorSlider[1].value = brushColor.g;
        colorSlider[2].value = brushColor.b;
        sizeSlider.value = (brushSize - minBrushSize) / (maxBrushSize - minBrushSize);
    }
    private void OnColorClick()
    {
        int i = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        brushColor.r = (colorPreset[i][0] / 255.0f);
        brushColor.g = (colorPreset[i][1] / 255.0f);
        brushColor.b = (colorPreset[i][2] / 255.0f);
        colorSlider[0].value = brushColor.r;
        colorSlider[1].value = brushColor.g;
        colorSlider[2].value = brushColor.b;
    }
    private void OnPaintClick()
    {
        isFilling = true;
    }
    void SetColor()
    {
        brushColor.r = colorSlider[0].value;
        brushColor.g = colorSlider[1].value;
        brushColor.b = colorSlider[2].value;
    }

    void SaveDrawingToTexture()
    {
        byte[] bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes("drawing.png", bytes);
    }
    private void LoadDrawing()
    {
        byte[] bytes = System.IO.File.ReadAllBytes("drawing.png");
        texture.LoadImage(bytes);
    }
    void OnFinishClick()
    {
        //완성된 이미지. texture2D형식에서 변환하여 전달
        Texture2D image = texture;
    }
}
