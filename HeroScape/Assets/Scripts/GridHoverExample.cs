using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHoverExample : MonoBehaviour
{
    public GridControllerExample controller;

    private void Start()
    {
        controller = FindObjectOfType<GridControllerExample>();
        foreach (MeshRenderer render in GetComponentsInChildren<MeshRenderer>())
        {
            render.enabled = false;
        }
    }

    void OnMouseExit()
    {
        if(controller.placing)
        foreach (MeshRenderer render in GetComponentsInChildren<MeshRenderer>())
        {
            render.enabled = false;
        }
    }

    private void OnMouseEnter()
    {
        if (controller.placing)
        foreach (MeshRenderer render in GetComponentsInChildren<MeshRenderer>())
        {
            render.enabled = true;
        }
    }
}
