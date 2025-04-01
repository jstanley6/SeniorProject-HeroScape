using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISafeAreaAdjuster : MonoBehaviour
{
    private RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;
        Vector2 anchorMin = new Vector2(safeArea.xMin / Screen.width, safeArea.yMin / Screen.height);
        Vector2 anchorMax = new Vector2(safeArea.xMax / Screen.width, safeArea.yMax / Screen.height);

        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
    }

    void Update()
    {
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            ApplySafeArea();
            lastWidth = Screen.width;
            lastHeight = Screen.height;
        }
    }

    private int lastWidth, lastHeight;
}

