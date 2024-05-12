using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script creates an object to keep track of data and objects within each grid position of a grid generated using GridSystem.
 */

public class GridObject // No Monobehavior for constructor
{
    private GridSystem gridSystem; // Variable to keep track of which grid system this object is part of,
    private GridPosition gridPosition; // Variable to keep track of which space in the grid system this object governs.
    private List<Piece> pieceList; // Variable to keep track of what piece is in this position of the grid.

    public GridObject(GridSystem gridSystem, GridPosition gridPosition) // Basic public constructor for the object
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        pieceList = new List<Piece>();
    }

    public override string ToString() // Override the ToString to display data about the grid position of the object and piece in it. 
    {
        string pieceString = "";
        foreach (Piece piece in pieceList)
        {
            pieceString += piece + "\n";
        }
        return gridPosition.ToString() + "\n" + pieceString;
    }

    public void AddPiece(Piece piece) // Set a piece to this location.
    {
        pieceList.Add(piece);
    }

    public void RemovePiece(Piece piece) // Remove the piece from this location.
    {
        pieceList.Remove(piece);
    }

    public List<Piece> GetPieceList() // Return piece within this location.
    {
        return pieceList;
    }

    public bool HasAnyPiece() // See if there is a piece here.
    {
        return pieceList.Count > 0;
    }

    public Piece GetPiece() // Returns a piece if there is a piece, and otherwise returns null.
    {
        if (HasAnyPiece())
        {
            return pieceList[0];
        } else {
            return null;
        }
    }
}