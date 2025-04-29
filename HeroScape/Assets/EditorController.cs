using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorController : MonoBehaviour
{
    public TestingHexGrid grid;
    public Transform targetHex;
    public Piece selectedPiece;

    private bool holdingPiece = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (grid.lastGridObject != null)
        {
            targetHex = grid.lastGridObject.visualTransform;
        }
        if (holdingPiece)
        {
            selectedPiece.transform.position = targetHex.position;
        }
    }

    public void ClickedOnPiece(Piece piece)
    {
        if (!holdingPiece)
        {
            holdingPiece = true;
            selectedPiece = piece;
        }
        else if (selectedPiece == piece)
        {
            holdingPiece = false;
            selectedPiece = null;
        }
    }
}
