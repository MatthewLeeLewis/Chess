using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGrid : MonoBehaviour
{
    public static BoardGrid Instance { get; private set; } // Create a static instance of the board

    [SerializeField] private Transform gridDebugObjectPrefab; // Initialize Serialized Field for Debug Prefab.
    private GridSystem gridSystem; // Initialize a grid system object to work with.
    [SerializeField] private LayerMask piecesLayerMask;


    private void Awake()
    {
        if (Instance != null) // This if check ensures that multiple instances of this object do not exist and reports it if they do, and destroys the duplicate.
        {
            Debug.LogError("There's more than one BoardGrid! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this; // This instantiates the instance.

        gridSystem = new GridSystem(8, 8, 2f); // Creates a standard chessboard.
        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab); // Create the prefab Debug objects into the grid system.
    }

    public void AddPieceAtGridPosition(GridPosition gridPosition, Piece piece) // Place a piece at a specific grid position
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddPiece(piece);
    }

    public List<Piece> GetPieceListAtGridPosition(GridPosition gridPosition) // Return what piece is at a specific grid position
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetPieceList();
    }

    public void RemovePieceAtGridPosition(GridPosition gridPosition, Piece piece) // Remove pieces from a location.
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemovePiece(piece);
    }

    public void PieceMovedGridPosition(Piece piece, GridPosition fromGridPosition, GridPosition toGridPosition) // Called to move piece to new locations in the grid.
    {
        RemovePieceAtGridPosition(fromGridPosition, piece); // Clears piece from old position
        AddPieceAtGridPosition(toGridPosition, piece); // Sets piece in new position
    }

    // Following function inputs the coordinates of a world position to return the grid position.
    public GridPosition GetGridPosition(Vector3 worldPosition) 
    {
        return gridSystem.GetGridPosition(worldPosition);
    }

    // Same as above but vice-versa.
    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    // Returns whether the position is valid.
    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();
    public int GetHeight() => gridSystem.GetHeight();

    public bool HasAnyPieceOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyPiece();
    }

    public Piece GetPieceAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetPiece();
    }

    public bool IsThreatened(GridPosition testGridPosition, bool isDark)
    {
        Vector3 pieceWorldPosition = GetWorldPosition(testGridPosition);
        GridPosition gridPosition = testGridPosition;

        float heightDisplacement = 0.6f;
        GridPosition nwPosOffset = new GridPosition(-1, 1);
        GridPosition nwPos_grid = gridPosition + nwPosOffset;

        if (IsValidGridPosition(nwPos_grid))
        {
            Vector3 nwPos = GetWorldPosition(nwPos_grid);

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

        if (IsValidGridPosition(nePos_grid))
        {
            Vector3 nePos = GetWorldPosition(nePos_grid);

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
                                    Debug.DrawLine(pieceWorldPosition + Vector3.up * heightDisplacement, neHit.point, Color.white, 5f, true);
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
                                Debug.DrawLine(pieceWorldPosition + Vector3.up * heightDisplacement, neHit.point, Color.white, 5f, true);
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
                                Debug.DrawLine(pieceWorldPosition + Vector3.up * heightDisplacement, neHit.point, Color.white, 5f, true);
                                return true;
                            }
                        }
                    }
                }
            }
        }


        GridPosition sePosOffset = new GridPosition(1, -1);
        GridPosition sePos_grid = gridPosition + sePosOffset;

        if (IsValidGridPosition(sePos_grid))
        {
            Vector3 sePos = GetWorldPosition(sePos_grid);

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

        if (IsValidGridPosition(swPos_grid))
        {
            Vector3 swPos = GetWorldPosition(swPos_grid);

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

        if (IsValidGridPosition(southPos_grid))
        {
            Vector3 southPos = GetWorldPosition(southPos_grid);

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

        if (IsValidGridPosition(northPos_grid))
        {
            Vector3 northPos = GetWorldPosition(northPos_grid);

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

        if (IsValidGridPosition(eastPos_grid))
        {
            Vector3 eastPos = GetWorldPosition(eastPos_grid);

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

        if (IsValidGridPosition(westPos_grid))
        {
            Vector3 westPos = GetWorldPosition(westPos_grid);

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

        if (IsValidGridPosition(knightThreatTest1))
        {
            if (HasAnyPieceOnGridPosition(knightThreatTest1))
            {
                Piece testPiece = GetPieceAtGridPosition(knightThreatTest1);
                if (testPiece.GetPieceType() == "Knight" && testPiece.IsDark() != isDark)
                {
                    if (testPiece.GetGridPosition() != testGridPosition)
                    {
                        return true;
                    }
                }
            }
        }

        if (IsValidGridPosition(knightThreatTest2))
        {
            if (HasAnyPieceOnGridPosition(knightThreatTest2))
            {
                Piece testPiece = GetPieceAtGridPosition(knightThreatTest2);
                if (testPiece.GetPieceType() == "Knight" && testPiece.IsDark() != isDark)
                {
                    if (testPiece.GetGridPosition() != testGridPosition)
                    {
                        return true;
                    }
                }
            }
        }

        if (IsValidGridPosition(knightThreatTest3))
        {
            if (HasAnyPieceOnGridPosition(knightThreatTest3))
            {
                Piece testPiece = GetPieceAtGridPosition(knightThreatTest3);
                if (testPiece.GetPieceType() == "Knight" && testPiece.IsDark() != isDark)
                {
                    if (testPiece.GetGridPosition() != testGridPosition)
                    {
                        return true;
                    }
                }
            }
        }

        if (IsValidGridPosition(knightThreatTest4))
        {
            if (HasAnyPieceOnGridPosition(knightThreatTest4))
            {
                Piece testPiece = GetPieceAtGridPosition(knightThreatTest4);
                if (testPiece.GetPieceType() == "Knight" && testPiece.IsDark() != isDark)
                {
                    if (testPiece.GetGridPosition() != testGridPosition)
                    {
                        return true;
                    }
                }
            }
        }

        if (IsValidGridPosition(knightThreatTest5))
        {
            if (HasAnyPieceOnGridPosition(knightThreatTest5))
            {
                Piece testPiece = GetPieceAtGridPosition(knightThreatTest5);
                if (testPiece.GetPieceType() == "Knight" && testPiece.IsDark() != isDark)
                {
                    if (testPiece.GetGridPosition() != testGridPosition)
                    {
                        return true;
                    }
                }
            }
        }

        if (IsValidGridPosition(knightThreatTest6))
        {
            if (HasAnyPieceOnGridPosition(knightThreatTest6))
            {
                Piece testPiece = GetPieceAtGridPosition(knightThreatTest6);
                if (testPiece.GetPieceType() == "Knight" && testPiece.IsDark() != isDark)
                {
                    if (testPiece.GetGridPosition() != testGridPosition)
                    {
                        return true;
                    }
                }
            }
        }

        if (IsValidGridPosition(knightThreatTest7))
        {
            if (HasAnyPieceOnGridPosition(knightThreatTest7))
            {
                Piece testPiece = GetPieceAtGridPosition(knightThreatTest7);
                if (testPiece.GetPieceType() == "Knight" && testPiece.IsDark() != isDark)
                {
                    if (testPiece.GetGridPosition() != testGridPosition)
                    {
                        return true;
                    }
                }
            }
        }

        if (IsValidGridPosition(knightThreatTest8))
        {
            if (HasAnyPieceOnGridPosition(knightThreatTest8))
            {
                Piece testPiece = GetPieceAtGridPosition(knightThreatTest8);
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

    public float GetPositionValue(GridPosition targetPosition)
    {
        Piece targetPiece;
        Piece selectedPiece = PieceControlSystem.Instance.GetSelectedPiece();

        float discouragementValue = 0f;
        if (selectedPiece.GetPieceType() == "King" && !UICanvas.Instance.IsInCheck())
        {
            discouragementValue = 900f;
        }

        GridPosition targetPosCoord = targetPosition;
        GridPosition currentGridPosCoord = selectedPiece.GetGridPosition();

        if (!TurnSystem.Instance.IsDarkTurn())
        {
            switch (targetPosition.z)
            {
                case 0:
                    {
                        targetPosCoord.z = 7;
                        break;
                    }
                case 1:
                    {
                        targetPosCoord.z = 6;
                        break;
                    }
                case 2:
                    {
                        targetPosCoord.z = 5;
                        break;
                    }
                case 3:
                    {
                        targetPosCoord.z = 4;
                        break;
                    }
                case 4:
                    {
                        targetPosCoord.z = 3;
                        break;
                    }
                case 5:
                    {
                        targetPosCoord.z = 2;
                        break;
                    }
                case 6:
                    {
                        targetPosCoord.z = 1;
                        break;
                    }
                case 7:
                    {
                        targetPosCoord.z = 0;
                        break;
                    }
            }
            switch (targetPosition.x)
            {
                case 0:
                    {
                        targetPosCoord.x = 7;
                        break;
                    }
                case 1:
                    {
                        targetPosCoord.x = 6;
                        break;
                    }
                case 2:
                    {
                        targetPosCoord.x = 5;
                        break;
                    }
                case 3:
                    {
                        targetPosCoord.x = 4;
                        break;
                    }
                case 4:
                    {
                        targetPosCoord.x = 3;
                        break;
                    }
                case 5:
                    {
                        targetPosCoord.x = 2;
                        break;
                    }
                case 6:
                    {
                        targetPosCoord.x = 1;
                        break;
                    }
                case 7:
                    {
                        targetPosCoord.x = 0;
                        break;
                    }
            }
            switch (selectedPiece.GetGridPosition().z)
            {
                case 0:
                    {
                        currentGridPosCoord.z = 7;
                        break;
                    }
                case 1:
                    {
                        currentGridPosCoord.z = 6;
                        break;
                    }
                case 2:
                    {
                        currentGridPosCoord.z = 5;
                        break;
                    }
                case 3:
                    {
                        currentGridPosCoord.z = 4;
                        break;
                    }
                case 4:
                    {
                        currentGridPosCoord.z = 3;
                        break;
                    }
                case 5:
                    {
                        currentGridPosCoord.z = 2;
                        break;
                    }
                case 6:
                    {
                        currentGridPosCoord.z = 1;
                        break;
                    }
                case 7:
                    {
                        currentGridPosCoord.z = 0;
                        break;
                    }
            }
            switch (selectedPiece.GetGridPosition().x)
            {
                case 0:
                    {
                        currentGridPosCoord.x = 7;
                        break;
                    }
                case 1:
                    {
                        currentGridPosCoord.x = 6;
                        break;
                    }
                case 2:
                    {
                        currentGridPosCoord.x = 5;
                        break;
                    }
                case 3:
                    {
                        currentGridPosCoord.x = 4;
                        break;
                    }
                case 4:
                    {
                        currentGridPosCoord.x = 3;
                        break;
                    }
                case 5:
                    {
                        currentGridPosCoord.x = 2;
                        break;
                    }
                case 6:
                    {
                        currentGridPosCoord.x = 1;
                        break;
                    }
                case 7:
                    {
                        currentGridPosCoord.x = 0;
                        break;
                    }
            }
        }

        float targetPieceValue = 0f;
        if (HasAnyPieceOnGridPosition(targetPosition))
        {
            targetPiece = GetPieceAtGridPosition(targetPosition);
            targetPieceValue = targetPiece.GetComponent<PieceValue>().GetPower(targetPosition);
        }

        float currentValue = selectedPiece.GetComponent<PieceValue>().GetPower(currentGridPosCoord);
        if (selectedPiece.IsThreatened(selectedPiece.GetGridPosition()))
        {
            currentValue -= selectedPiece.GetRelativePower();
        }

        float targetValue = selectedPiece.GetComponent<PieceValue>().GetPower(targetPosCoord);
        if (IsThreatened(targetPosition, TurnSystem.Instance.IsDarkTurn()))
        {
            targetValue -= selectedPiece.GetRelativePower();
        }

        float vulnerabilityValue = 0f;
        List<Piece> allyPieceList;
        if (TurnSystem.Instance.IsDarkTurn())
        {
            allyPieceList = PieceManager.Instance.GetDarkPieceList();
        }
        else
        {
            allyPieceList = PieceManager.Instance.GetLightPieceList();
        }
        foreach (Piece piece in allyPieceList)
        {
            if (piece != selectedPiece && piece.IsThreatened(targetPosition))
            {
                vulnerabilityValue += piece.GetRelativePower();
            }
        }

        float threatValue = 0f;
        if (selectedPiece.GetPieceType() != "King" && !IsThreatened(targetPosition, TurnSystem.Instance.IsDarkTurn()))
        {
            List<GridPosition> newActionGridPositionList;
            
            newActionGridPositionList = selectedPiece.GetPieceAction().GetTheoreticalActionGridPositionList(targetPosition);
            
            foreach (GridPosition gridPosition in newActionGridPositionList)
            {
                if (HasAnyPieceOnGridPosition(gridPosition))
                {
                    Piece testPiece = GetPieceAtGridPosition(gridPosition);
                    if (testPiece.IsDark() != selectedPiece.IsDark())
                    {
                        threatValue += (testPiece.GetRelativePower() / 10);
                    }
                }
            }
        }

        return (threatValue + targetPieceValue + targetValue - currentValue - vulnerabilityValue - discouragementValue); 
    }
        
}
