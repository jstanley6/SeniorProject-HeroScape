using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor.Animations;
using UnityEngine;
using static HexGrid;

public class EditorController : MonoBehaviour
{
    public HexGrid grid;
    public Transform targetHex;
    public Piece selectedPiece;
    public int activelayer = 0;
    public Mouse3D mouse;
    //public List<ItemInGrid[][]> terrain;
    Dictionary<Vector3Int, Piece> terrainPieces = new Dictionary<Vector3Int, Piece>();
    Dictionary<Vector3Int, ItemInGrid> hexContents = new Dictionary<Vector3Int, ItemInGrid>();

    public bool holdingPiece = false;

    public class ItemInGrid
    {
        public Piece parentPiece;
        public ItemInGrid (Piece piece)
        {
            parentPiece = piece;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        activelayer = (int) Math.Round(mouse.transform.position.y * 5);
        if (grid.lastGridObject != null)
        {
            targetHex = grid.lastGridObject.visualTransform;
        }
        if (holdingPiece)
        {
            selectedPiece.transform.position = targetHex.position;
            grid.gridHexXZLayers[activelayer].GetXZ(selectedPiece.transform.position, out int posX, out int posZ);
            selectedPiece.gridPosition = new Vector3Int(posX, activelayer, posZ);
            while (CheckForOverlap(selectedPiece))
            {
                activelayer++;
                if (activelayer >= grid.gridHexXZLayers.Count)
                {
                    grid.AddLayer();
                }
                //selectedPiece.transform.position += new Vector3(0, 0.2f, 0);
                selectedPiece.gridPosition.y++;
            }
        }
    }

    public void ClickedOnPiece(Piece piece)
    {
        if (!holdingPiece)
        {
            holdingPiece = true;
            selectedPiece = piece;
            ChangeBaseMaterial(FindChildrenWithTag(selectedPiece.gameObject, "TileBase"), selectedPiece.selectedMat);
            foreach(Transform child in piece.transform)
            {
                child.gameObject.layer = 0;
                grid.gridHexXZLayers[activelayer].GetXZ(child.transform.position, out int childX, out int childZ);
                hexContents.Remove(new Vector3Int(childX, selectedPiece.gridPosition.y, childZ));
            }
            terrainPieces.Remove(new Vector3Int(selectedPiece.gridPosition.x, selectedPiece.gridPosition.y, selectedPiece.gridPosition.z));
        }
        else if (selectedPiece == piece)
        {
            holdingPiece = false;
            foreach (Transform child in piece.transform)
            {
                child.gameObject.layer = 3;
                grid.gridHexXZLayers[activelayer].GetXZ(child.transform.position, out int childX, out int childZ);
                hexContents.Add(new Vector3Int(childX, selectedPiece.gridPosition.y, childZ), new ItemInGrid(selectedPiece));
            }
            ChangeBaseMaterial(FindChildrenWithTag(selectedPiece.gameObject, "TileBase"), selectedPiece.highlightMat);
            int xPos = 0;
            int yPos = 0;
            grid.gridHexXZLayers[activelayer].GetXZ(selectedPiece.gameObject.transform.position, out xPos, out yPos);
            selectedPiece.gridPosition = new Vector3Int(xPos, activelayer, yPos);
            terrainPieces.Add(new Vector3Int(selectedPiece.gridPosition.x, selectedPiece.gridPosition.y, selectedPiece.gridPosition.z), selectedPiece);
            selectedPiece = null;
        }
    }
    List<GameObject> FindChildrenWithTag(GameObject parent, string tag)
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag(tag))
            {
                children.Add(child.gameObject);
            }
        }
        return children;
    }

    void ChangeBaseMaterial(List<GameObject> tileHexes, Material mat)
    {
        foreach (GameObject tile in tileHexes)
        {
            tile.GetComponent<Renderer>().material = mat;
        }
    }

    bool CheckForOverlap(Piece piece)
    {
        foreach (Transform child in piece.transform)
        {
            grid.gridHexXZLayers[activelayer].GetXZ(child.transform.position, out int childX, out int childZ);
            hexContents.TryGetValue(new Vector3Int(childX, piece.gridPosition.y, childZ), out ItemInGrid item);
            if (item != null)
            {
                return true;
            }
        }
        return false;
    }
}
