using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingHexGrid : MonoBehaviour {


    [SerializeField] private Transform pfHex;

    private GridHexXZ<GridObject> gridHexXZ;
    private GridObject lastGridObject;
    public int width = 30;
    public int height = 30;

    private class GridObject {
        public Transform visualTransform;

        public void Show() {
            visualTransform.Find("Selected").gameObject.SetActive(true);
        }

        public void Hide() {
            visualTransform.Find("Selected").gameObject.SetActive(false);
        }

    }

    private void Awake() {
        float cellSize = 1f;
        gridHexXZ = 
            new GridHexXZ<GridObject>(width, height, cellSize, new Vector3(width/-2, 0, height/-2), (GridHexXZ<GridObject> g, int x, int y) => new GridObject());

        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                Transform visualTransform = Instantiate(pfHex, gridHexXZ.GetWorldPosition(x, z), Quaternion.identity);
                gridHexXZ.GetGridObject(x, z).visualTransform = visualTransform;
                gridHexXZ.GetGridObject(x, z).Hide();
            }
        }

    }

    private void Update() {
        if (lastGridObject != null) {
            lastGridObject.Hide();
        }

        lastGridObject = gridHexXZ.GetGridObject(Mouse3D.GetMouseWorldPosition());

        if (lastGridObject != null) {
            lastGridObject.Show();
        }
        
    }
}