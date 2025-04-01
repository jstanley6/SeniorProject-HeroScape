using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicCanvasScaler : MonoBehaviour
{
    public CanvasScaler canvasScaler;
    public float referenceAspectRatio = 16f / 9f; // Set your default aspect ratio (16:9)

    void Start()
    {
        AdjustCanvas();
    }

    void AdjustCanvas()
    {
        if (canvasScaler == null)
            canvasScaler = GetComponent<CanvasScaler>();

        float currentAspectRatio = (float)Screen.width / Screen.height;

        // Adjust the match mode dynamically based on aspect ratio
        canvasScaler.matchWidthOrHeight = currentAspectRatio >= referenceAspectRatio ? 0 : 1;
    }

    void Update()
    {
        // Check if the resolution changes (useful for resizing the window)
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            AdjustCanvas();
            lastWidth = Screen.width;
            lastHeight = Screen.height;
        }
    }

    private int lastWidth, lastHeight;
}

