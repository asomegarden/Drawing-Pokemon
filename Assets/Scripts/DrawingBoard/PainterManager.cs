using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainterManager : MonoBehaviour
{
    public static PainterManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public Painter painter;

    public void EnablePainter()
    {
        painter.gameObject.SetActive(true);
    }

    public void DisablePainter()
    {
        painter.gameObject.SetActive(false);
    }
}
