using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HexGrid : MonoBehaviour {


    [SerializeField] private Transform pfHex;

    public List<GridHexXZ<GridObject>> gridHexXZLayers;
    private EditorController editor;
    private float cellSize = 1f;
    public GridObject lastGridObject;
    public int sizeX = 30;
    public int sizeZ = 30;
    Dictionary<Vector3Int, Piece> terrainPieces = new Dictionary<Vector3Int, Piece>();
    //public ItemInGrid[][] gridContentArray;

    public class GridObject {
        public Transform visualTransform;

        public void Show() {
            visualTransform.Find("Selected").gameObject.SetActive(true);
        }

        public void Hide() {
            visualTransform.Find("Selected").gameObject.SetActive(false);
        }

        public void Visible()
        {
            visualTransform.Find("HexBase").gameObject.SetActive(true);
        }
    }

    public class ItemInGrid
    {
        public Piece parentPiece;

    }

    private void Awake() {
        editor = FindObjectOfType<EditorController>();
        gridHexXZLayers = new List<GridHexXZ<GridObject>>();

        AddLayer();
        AddLayer();
    }

    private void AddLayer()
    {
        gridHexXZLayers.Add(
            new GridHexXZ<GridObject>(sizeX, sizeZ, cellSize, new Vector3(sizeX / -2, gridHexXZLayers.Count() / 5, sizeZ / -2), (GridHexXZ<GridObject> g, int x, int y) => new GridObject()));
        //print(gridHexXZLayers.Count());
        for (int x = 0; x < sizeX; x++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                Transform visualTransform = Instantiate(pfHex, gridHexXZLayers.Last<GridHexXZ<GridObject>>().GetWorldPosition(x, z) + new Vector3(0, gridHexXZLayers.Count() / 5f - 0.2f, 0), Quaternion.identity);
                gridHexXZLayers.Last<GridHexXZ<GridObject>>().GetGridObject(x, z).visualTransform = visualTransform;
                gridHexXZLayers.Last<GridHexXZ<GridObject>>().GetGridObject(x, z).Hide();
                if(gridHexXZLayers.Count() == 1)
                {
                    gridHexXZLayers.Last<GridHexXZ<GridObject>>().GetGridObject(x, z).Visible();
                }
            }
        }
    }

    private void Update() {
        if (lastGridObject != null || editor.activelayer != 0) {
            lastGridObject.Hide();
        }
        if (editor.activelayer >= gridHexXZLayers.Count())
        {
            AddLayer();
        }

        lastGridObject = gridHexXZLayers[editor.activelayer].GetGridObject(Mouse3D.GetMouseWorldPosition());

        if (lastGridObject != null && editor.activelayer == 0) {
            lastGridObject.Show();
        }
        
    }
}