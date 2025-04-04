using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControllerExample : MonoBehaviour
{
    public bool placing = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && placing)
            placing = false;
        else if (Input.GetMouseButtonDown(0) && !placing)
            placing = true;
    }
}
