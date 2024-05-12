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
    [SerializeField] private bool isDark;

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
        Destroy(this.gameObject);
        OnAnyPieceDead?.Invoke(this, EventArgs.Empty);
    }
}
