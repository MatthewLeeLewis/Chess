using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script governs the necessary functionality held within each piece.
 */

public class Piece : MonoBehaviour
{
    public static event EventHandler OnAnyPieceDead;
    public static event EventHandler OnAnyPieceSpawned;
    [SerializeField] private LayerMask piecesLayerMask;  
    [SerializeField] private int relativePower;

    private enum PieceType 
    {
        Pawn,
        Rook, 
        Bishop,
        Knight,
        King,
        Queen
    };

    [SerializeField] private bool isDark;
    [SerializeField] private Transform testBox;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private PieceType pieceType;

    private GridPosition gridPosition; // Keeps track of the piece's grid position.
    private PieceAction pieceAction; // Variable for the piece's action.

    private void Awake()
    {
        pieceAction = GetComponent<PieceAction>(); // Assigns the correct PieceAction script to the variable.
    }

    private void Start()
    {
        gridPosition = BoardGrid.Instance.GetGridPosition(transform.position); // Identify this piece's starting grid position.
        BoardGrid.Instance.AddPieceAtGridPosition(gridPosition, this); // Place it within the position on the grid object array.

        OnAnyPieceSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        GridPosition newGridPosition = BoardGrid.Instance.GetGridPosition(transform.position); // Identify this piece's new grid position.

        if (newGridPosition != gridPosition) // If the piece has moved to a different grid space
        {
            BoardGrid.Instance.PieceMovedGridPosition(this, gridPosition, newGridPosition); // Update the grid array for the piece's new location.
            gridPosition = newGridPosition; // Update the grid position on this piece.
        }
    }

    public PieceAction GetPieceAction() // Function to access the piece's action through the piece.
    {
        return pieceAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public bool IsDark() 
    {
        return isDark;
    }

    public Vector3 GetWorldPosition() 
    {
        return transform.position;
    }

    public void Die()
    {
        OnAnyPieceDead?.Invoke(this, EventArgs.Empty);
    }

    public void DeactivateCollider()
    {
        boxCollider.enabled = false;
    }

    public void EnableCollider()
    {
        boxCollider.enabled = true;
    }

    public Transform GetBox()
    {
        return testBox;
    }

    public string GetPieceType()
    {
        return pieceType.ToString();
    }

    public bool IsThreatened(GridPosition testGridPosition)
    {
        Vector3 pieceWorldPosition = GetWorldPosition();

        float heightDisplacement = 0.6f;
        GridPosition nwPosOffset = new GridPosition(-1, 1);
        GridPosition nwPos_grid = gridPosition + nwPosOffset;

        if (BoardGrid.Instance.IsValidGridPosition(nwPos_grid))
        {
             Vector3 nwPos = BoardGrid.Instance.GetWorldPosition(nwPos_grid);

            Vector3 northwest = (nwPos - pieceWorldPosition).normalized;
            if (Physics.Raycast(
                    pieceWorldPosition + Vector3.up * heightDisplacement,
                    northwest,
                    out RaycastHit nwHit,
                    32f,
                    piecesLayerMask))
            {
                if (nwHit.transform.TryGetComponent<Piece>(out Piece piece)) // Check if the hit object is a piece...
                {
                    if (piece.GetPieceType() == "Pawn")
                    {
                        if (piece.GetGridPosition() == nwPos_grid)
                        {
                            if (piece.IsDark() && isDark == false)
                            {
                                if (piece.GetGridPosition() != testGridPosition)
                                {
                                    Debug.DrawLine(pieceWorldPosition + Vector3.up * heightDisplacement, nwHit.point, Color.white, 5f, true);
                                    return true;
                                }
                            }
                        }
                    }
                    else if (piece.GetPieceType() == "Bishop" || piece.GetPieceType() == "Queen")
                    {
                        if (piece.IsDark() != isDark)
                        {
                            if (piece.GetGridPosition() != testGridPosition)
                            {
                                Debug.DrawLine(pieceWorldPosition + Vector3.up * heightDisplacement, nwHit.point, Color.white, 5f, true);
                                return true;
                            }
                        }
                    }
                    else if (piece.GetPieceType() == "King")
                    {
                        if (piece.GetGridPosition() == nwPos_grid)
                        {
                            if (piece.GetGridPosition() != testGridPosition)
                            {
                                Debug.DrawLine(pieceWorldPosition + Vector3.up * heightDisplacement, nwHit.point, Color.white, 5f, true);
                                return true;
                            }
                        }
                    }
                }
            }
        }
       

        GridPosition nePosOffset = new GridPosition(1, 1);
        GridPosition nePos_grid = gridPosition + nePosOffset;

        if (BoardGrid.Instance.IsValidGridPosition(nePos_grid))
        {
            Vector3 nePos = BoardGrid.Instance.GetWorldPosition(nePos_grid);

            Vector3 northeast = (nePos - pieceWorldPosition).normalized;
            if (Physics.Raycast(
                    pieceWorldPosition + Vector3.up * heightDisplacement,
                    northeast,
                    out RaycastHit neHit,
                    32f,
                    piecesLayerMask))
            {
                if (neHit.transform.TryGetComponent<Piece>(out Piece piece)) // Check if the hit object is a piece...
                {
                    if (piece.GetPieceType() == "Pawn")
                    {
                        if (piece.GetGridPosition() == nePos_grid)
                        {
                            if (piece.IsDark() && isDark == false)
                            {
                                if (piece.GetGridPosition() != testGridPosition)
                                {
                                    //Debug.DrawLine(pieceWorldPosition + Vector3.up * heightDisplacement, neHit.point, Color.white, 5f, true);
                                    return true;
                                }
                            }
                        }
                    }
                    else if (piece.GetPieceType() == "Bishop" || piece.GetPieceType() == "Queen")
                    {
                        if (piece.IsDark() != isDark)
                        {
                            if (piece.GetGridPosition() != testGridPosition)
                            {
                                //Debug.DrawLine(pieceWorldPosition + Vector3.up * heightDisplacement, neHit.point, Color.white, 5f, true);
                                return true;
                            }
                        }
                    }
                    else if (piece.GetPieceType() == "King")
                    {
                        if (piece.GetGridPosition() == nePos_grid)
                        {
                            if (piece.GetGridPosition() != testGridPosition)
                            {
                                //Debug.DrawLine(pieceWorldPosition + Vector3.up * heightDisplacement, neHit.point, Color.white, 5f, true);
                                return true;
                            }
                        }
                    }
                }
            }
        }
        

        GridPosition sePosOffset = new GridPosition(1, -1);
        GridPosition sePos_grid = gridPosition + sePosOffset;

        if (BoardGrid.Instance.IsValidGridPosition(sePos_grid))
        {
            Vector3 sePos = BoardGrid.Instance.GetWorldPosition(sePos_grid);

            Vector3 southeast = (sePos - pieceWorldPosition).normalized;
            if (Physics.Raycast(
                    pieceWorldPosition + Vector3.up * heightDisplacement,
                    southeast,
                    out RaycastHit seHit,
                    32f,
                    piecesLayerMask))
            {
                if (seHit.transform.TryGetComponent<Piece>(out Piece piece)) // Check if the hit object is a piece...
                {
                    if (piece.GetPieceType() == "Pawn")
                    {
                        if (piece.GetGridPosition() == sePos_grid)
                        {
                            if (!piece.IsDark() && isDark == true)
                            {
                                if (piece.GetGridPosition() != testGridPosition)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    else if (piece.GetPieceType() == "Bishop" || piece.GetPieceType() == "Queen")
                    {
                        if (piece.IsDark() != isDark)
                        {
                            if (piece.GetGridPosition() != testGridPosition)
                            {
                                return true;
                            }
                        }
                    }
                    else if (piece.GetPieceType() == "King")
                    {
                        if (piece.GetGridPosition() == sePos_grid)
                        {
                            if (piece.GetGridPosition() != testGridPosition)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        

        GridPosition swPosOffset = new GridPosition(-1, -1);
        GridPosition swPos_grid = gridPosition + swPosOffset;

        if (BoardGrid.Instance.IsValidGridPosition(swPos_grid))
        {
            Vector3 swPos = BoardGrid.Instance.GetWorldPosition(swPos_grid);

            Vector3 southwest = (swPos - pieceWorldPosition).normalized;
            if (Physics.Raycast(
                    pieceWorldPosition + Vector3.up * heightDisplacement,
                    southwest,
                    out RaycastHit swHit,
                    32f,
                    piecesLayerMask))
            {
                if (swHit.transform.TryGetComponent<Piece>(out Piece piece)) // Check if the hit object is a piece...
                {
                    if (piece.GetPieceType() == "Pawn")
                    {
                        if (piece.GetGridPosition() == swPos_grid)
                        {
                            if (!piece.IsDark() && isDark == true)
                            {
                                if (piece.GetGridPosition() != testGridPosition)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    else if (piece.GetPieceType() == "Bishop" || piece.GetPieceType() == "Queen")
                    {
                        if (piece.IsDark() != isDark)
                        {
                            if (piece.GetGridPosition() != testGridPosition)
                            {
                                return true;
                            }
                        }
                    }
                    else if (piece.GetPieceType() == "King")
                    {
                        if (piece.GetGridPosition() == swPos_grid)
                        {
                            if (piece.GetGridPosition() != testGridPosition)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        

        GridPosition southPosOffset = new GridPosition(0, -1);
        GridPosition southPos_grid = gridPosition + southPosOffset;

        if (BoardGrid.Instance.IsValidGridPosition(southPos_grid))
        {
            Vector3 southPos = BoardGrid.Instance.GetWorldPosition(southPos_grid);

            Vector3 south = (southPos - pieceWorldPosition).normalized;
            if (Physics.Raycast(
                    pieceWorldPosition + Vector3.up * heightDisplacement,
                    south,
                    out RaycastHit southHit,
                    32f,
                    piecesLayerMask))
            {
                if (southHit.transform.TryGetComponent<Piece>(out Piece piece)) // Check if the hit object is a piece...
                {
                    if (piece.GetPieceType() == "Rook" || piece.GetPieceType() == "Queen")
                    {
                        if (piece.IsDark() != isDark)
                        {
                            if (piece.GetGridPosition() != testGridPosition)
                            {
                                return true;
                            }
                        }
                    }
                    else if (piece.GetPieceType() == "King")
                    {
                        if (piece.GetGridPosition() == southPos_grid)
                        {
                            if (piece.GetGridPosition() != testGridPosition)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        

        GridPosition northPosOffset = new GridPosition(0, 1);
        GridPosition northPos_grid = gridPosition + northPosOffset;

        if (BoardGrid.Instance.IsValidGridPosition(northPos_grid))
        {
            Vector3 northPos = BoardGrid.Instance.GetWorldPosition(northPos_grid);

            Vector3 north = (northPos - pieceWorldPosition).normalized;
            if (Physics.Raycast(
                    pieceWorldPosition + Vector3.up * heightDisplacement,
                    north,
                    out RaycastHit northHit,
                    32f,
                    piecesLayerMask))
            {
                if (northHit.transform.TryGetComponent<Piece>(out Piece piece)) // Check if the hit object is a piece...
                {
                    if (piece.GetPieceType() == "Rook" || piece.GetPieceType() == "Queen")
                    {
                        if (piece.IsDark() != isDark)
                        {
                            if (piece.GetGridPosition() != testGridPosition)
                            {
                                return true;
                            }
                        }
                    }
                    else if (piece.GetPieceType() == "King")
                    {
                        if (piece.GetGridPosition() == northPos_grid)
                        {
                            if (piece.GetGridPosition() != testGridPosition)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        
        GridPosition eastPosOffset = new GridPosition(1, 0);
        GridPosition eastPos_grid = gridPosition + eastPosOffset;

        if (BoardGrid.Instance.IsValidGridPosition(eastPos_grid))
        {
            Vector3 eastPos = BoardGrid.Instance.GetWorldPosition(eastPos_grid);

            Vector3 east = (eastPos - pieceWorldPosition).normalized;
            if (Physics.Raycast(
                    pieceWorldPosition + Vector3.up * heightDisplacement,
                    east,
                    out RaycastHit eastHit,
                    32f,
                    piecesLayerMask))
            {
                if (eastHit.transform.TryGetComponent<Piece>(out Piece piece)) // Check if the hit object is a piece...
                {
                    if (piece.GetPieceType() == "Rook" || piece.GetPieceType() == "Queen")
                    {
                        if (piece.IsDark() != isDark)
                        {
                            if (piece.GetGridPosition() != testGridPosition)
                            {
                                return true;
                            }
                        }
                    }
                    else if (piece.GetPieceType() == "King")
                    {
                        if (piece.GetGridPosition() == eastPos_grid)
                        {
                            if (piece.GetGridPosition() != testGridPosition)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        

        GridPosition westPosOffset = new GridPosition(-1, 0);
        GridPosition westPos_grid = gridPosition + westPosOffset;

        if (BoardGrid.Instance.IsValidGridPosition(westPos_grid))
        {
            Vector3 westPos = BoardGrid.Instance.GetWorldPosition(westPos_grid);

            Vector3 west = (westPos - pieceWorldPosition).normalized;
            if (Physics.Raycast(
                    pieceWorldPosition + Vector3.up * heightDisplacement,
                    west,
                    out RaycastHit westHit,
                    32f,
                    piecesLayerMask))
            {
                if (westHit.transform.TryGetComponent<Piece>(out Piece piece)) // Check if the hit object is a piece...
                {
                    if (piece.GetPieceType() == "Rook" || piece.GetPieceType() == "Queen")
                    {
                        if (piece.IsDark() != isDark)
                        {
                            if (piece.GetGridPosition() != testGridPosition)
                            {
                                return true;
                            }
                        }
                    }
                    else if (piece.GetPieceType() == "King")
                    {
                        if (piece.GetGridPosition() == westPos_grid)
                        {
                            if (piece.GetGridPosition() != testGridPosition)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        
        
        GridPosition knightThreat1 = new GridPosition(-1, 2);
        GridPosition knightThreat2 = new GridPosition(-1, -2);
        GridPosition knightThreat3 = new GridPosition(1, 2);
        GridPosition knightThreat4 = new GridPosition(1, -2);
        GridPosition knightThreat5 = new GridPosition(2, 1);
        GridPosition knightThreat6 = new GridPosition(2, -1);
        GridPosition knightThreat7 = new GridPosition(-2, 1);
        GridPosition knightThreat8 = new GridPosition(-2, -1);
        GridPosition knightThreatTest1 = gridPosition + knightThreat1;
        GridPosition knightThreatTest2 = gridPosition + knightThreat2;
        GridPosition knightThreatTest3 = gridPosition + knightThreat3;
        GridPosition knightThreatTest4 = gridPosition + knightThreat4;
        GridPosition knightThreatTest5 = gridPosition + knightThreat5;
        GridPosition knightThreatTest6 = gridPosition + knightThreat6;
        GridPosition knightThreatTest7 = gridPosition + knightThreat7;
        GridPosition knightThreatTest8 = gridPosition + knightThreat8;

        if (BoardGrid.Instance.IsValidGridPosition(knightThreatTest1))
        {
            if (BoardGrid.Instance.HasAnyPieceOnGridPosition(knightThreatTest1))
            {
                Piece testPiece = BoardGrid.Instance.GetPieceAtGridPosition(knightThreatTest1);
                if (testPiece.GetPieceType() == "Knight" && testPiece.IsDark() != isDark)
                {
                    if (testPiece.GetGridPosition() != testGridPosition)
                    {
                        return true;
                    }
                }
            }
        }
        
        if (BoardGrid.Instance.IsValidGridPosition(knightThreatTest2))
        {
            if (BoardGrid.Instance.HasAnyPieceOnGridPosition(knightThreatTest2))
            {
                Piece testPiece = BoardGrid.Instance.GetPieceAtGridPosition(knightThreatTest2);
                if (testPiece.GetPieceType() == "Knight" && testPiece.IsDark() != isDark)
                {
                    if (testPiece.GetGridPosition() != testGridPosition)
                    {
                        return true;
                    }
                }
            }
        }
        
        if (BoardGrid.Instance.IsValidGridPosition(knightThreatTest3))
        {
            if (BoardGrid.Instance.HasAnyPieceOnGridPosition(knightThreatTest3))
            {
                Piece testPiece = BoardGrid.Instance.GetPieceAtGridPosition(knightThreatTest3);
                if (testPiece.GetPieceType() == "Knight" && testPiece.IsDark() != isDark)
                {
                    if (testPiece.GetGridPosition() != testGridPosition)
                    {
                        return true;
                    }
                }
            }
        }
        
        if (BoardGrid.Instance.IsValidGridPosition(knightThreatTest4))
        {
            if (BoardGrid.Instance.HasAnyPieceOnGridPosition(knightThreatTest4))
            {
                Piece testPiece = BoardGrid.Instance.GetPieceAtGridPosition(knightThreatTest4);
                if (testPiece.GetPieceType() == "Knight" && testPiece.IsDark() != isDark)
                {
                    if (testPiece.GetGridPosition() != testGridPosition)
                    {
                        return true;
                    }
                }
            }
        }

        if (BoardGrid.Instance.IsValidGridPosition(knightThreatTest5))
        {
            if (BoardGrid.Instance.HasAnyPieceOnGridPosition(knightThreatTest5))
            {
                Piece testPiece = BoardGrid.Instance.GetPieceAtGridPosition(knightThreatTest5);
                if (testPiece.GetPieceType() == "Knight" && testPiece.IsDark() != isDark)
                {
                    if (testPiece.GetGridPosition() != testGridPosition)
                    {
                        return true;
                    }
                }
            }
        }

        if (BoardGrid.Instance.IsValidGridPosition(knightThreatTest6))
        {
            if (BoardGrid.Instance.HasAnyPieceOnGridPosition(knightThreatTest6))
            {
                Piece testPiece = BoardGrid.Instance.GetPieceAtGridPosition(knightThreatTest6);
                if (testPiece.GetPieceType() == "Knight" && testPiece.IsDark() != isDark)
                {
                    if (testPiece.GetGridPosition() != testGridPosition)
                    {
                        return true;
                    }
                }
            }
        }

        if (BoardGrid.Instance.IsValidGridPosition(knightThreatTest7))
        {
            if (BoardGrid.Instance.HasAnyPieceOnGridPosition(knightThreatTest7))
            {
                Piece testPiece = BoardGrid.Instance.GetPieceAtGridPosition(knightThreatTest7);
                if (testPiece.GetPieceType() == "Knight" && testPiece.IsDark() != isDark)
                {
                    if (testPiece.GetGridPosition() != testGridPosition)
                    {
                        return true;
                    }
                }
            }
        }

        if (BoardGrid.Instance.IsValidGridPosition(knightThreatTest8))
        {
            if (BoardGrid.Instance.HasAnyPieceOnGridPosition(knightThreatTest8))
            {
                Piece testPiece = BoardGrid.Instance.GetPieceAtGridPosition(knightThreatTest8);
                if (testPiece.GetPieceType() == "Knight" && testPiece.IsDark() != isDark)
                {
                    if (testPiece.GetGridPosition() != testGridPosition)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }   

    public float GetRelativePower()
    {
        return (float) relativePower;
    }
}
