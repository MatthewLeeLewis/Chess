using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGrid : MonoBehaviour
{
    public static BoardGrid Instance { get; private set; } // Create a static instance of the board

    [SerializeField] private Transform gridDebugObjectPrefab; // Initialize Serialized Field for Debug Prefab.
    private GridSystem gridSystem; // Initialize a grid system object to work with.

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
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab); // Create the prefab Debug objects into the grid system.
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
}
