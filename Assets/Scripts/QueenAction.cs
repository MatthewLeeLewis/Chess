using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenAction : PieceAction // By making the base class abstract, instances of this class cannot be created, only subclasses.
{
    public event EventHandler OnStopMoving;
    [SerializeField] private LayerMask piecesLayerMask;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        float stoppingDistance = .1f; // Sets the accuracy of the stopping point.
        Vector3 moveDirection = (targetPosition - transform.position).normalized; // Determine what direction it needs to be moving in...

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance) // If the target isn't yet within the accuracy of the stopping point...
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime; // Move it in that direction.
        }
        else
        {
            OnStopMoving?.Invoke(this, EventArgs.Empty);
            ActionComplete();
        }
    }
    public override List<GridPosition> GetValidActionGridPositionList() // Function to test a unit's move distance against valid grid positions and only return a list of valid positions.
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition pieceGridPosition = piece.GetGridPosition();

        for (int x = -8; x <= 8; x++)
        {
            for (int z = -8; z <= 8; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = pieceGridPosition + offsetGridPosition;

                if (!BoardGrid.Instance.IsValidGridPosition(testGridPosition)) // if check to prevent movement out of bounds of grid.
                {
                    continue; // exits current for iteration and moves onto the next one.
                }

                if (Mathf.Abs(x) != Mathf.Abs(z))
                {
                    if (x != 0 && z != 0)
                    {
                        continue; // exits current for iteration and moves onto the next one.
                    }
                }

                if (BoardGrid.Instance.HasAnyPieceOnGridPosition(testGridPosition)) // if check to prevent movement to where another unit already is.
                {
                    Piece targetPiece = BoardGrid.Instance.GetPieceAtGridPosition(testGridPosition);
                    if (piece.IsDark() == targetPiece.IsDark())
                    {
                        continue;
                    }
                }

                Vector3 gridWorldPosition = BoardGrid.Instance.GetWorldPosition(testGridPosition);
                Vector3 pieceWorldPosition = BoardGrid.Instance.GetWorldPosition(pieceGridPosition);

                Vector3 moveDirection = (gridWorldPosition - pieceWorldPosition).normalized;

                float heightDisplacement = 0.6f;
                if (Physics.Raycast(
                    pieceWorldPosition + Vector3.up * heightDisplacement,
                    moveDirection,
                    (Vector3.Distance(pieceWorldPosition, gridWorldPosition)) - 2f,
                    piecesLayerMask))
                {
                    // Blocked by another piece.
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }
        
        return validGridPositionList;
    }

    public override List<GridPosition> GetTheoreticalActionGridPositionList(GridPosition gridPosition) // Function to test a unit's move distance against valid grid positions and only return a list of valid positions.
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition pieceGridPosition = gridPosition;

        for (int x = -8; x <= 8; x++)
        {
            for (int z = -8; z <= 8; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = pieceGridPosition + offsetGridPosition;

                if (!BoardGrid.Instance.IsValidGridPosition(testGridPosition)) // if check to prevent movement out of bounds of grid.
                {
                    continue; // exits current for iteration and moves onto the next one.
                }

                if (Mathf.Abs(x) != Mathf.Abs(z))
                {
                    if (x != 0 && z != 0)
                    {
                        continue; // exits current for iteration and moves onto the next one.
                    }
                }

                if (BoardGrid.Instance.HasAnyPieceOnGridPosition(testGridPosition)) // if check to prevent movement to where another unit already is.
                {
                    Piece targetPiece = BoardGrid.Instance.GetPieceAtGridPosition(testGridPosition);
                    if (piece.IsDark() == targetPiece.IsDark())
                    {
                        continue;
                    }
                }

                Vector3 gridWorldPosition = BoardGrid.Instance.GetWorldPosition(testGridPosition);
                Vector3 pieceWorldPosition = BoardGrid.Instance.GetWorldPosition(pieceGridPosition);

                Vector3 moveDirection = (gridWorldPosition - pieceWorldPosition).normalized;

                float heightDisplacement = 0.6f;
                if (Physics.Raycast(
                    pieceWorldPosition + Vector3.up * heightDisplacement,
                    moveDirection,
                    (Vector3.Distance(pieceWorldPosition, gridWorldPosition)) - 2f,
                    piecesLayerMask))
                {
                    // Blocked by another piece.
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }
        
        return validGridPositionList;
    }
}