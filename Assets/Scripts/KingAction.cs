using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingAction : PieceAction // By making the base class abstract, instances of this class cannot be created, only subclasses.
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

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = pieceGridPosition + offsetGridPosition;

                if (!BoardGrid.Instance.IsValidGridPosition(testGridPosition)) // if check to prevent movement out of bounds of grid.
                {
                    continue; // exits current for iteration and moves onto the next one.
                }

                if (BoardGrid.Instance.HasAnyPieceOnGridPosition(testGridPosition)) // if check to prevent movement to where another unit already is.
                {
                    Piece targetPiece = BoardGrid.Instance.GetPieceAtGridPosition(testGridPosition);
                    if (piece.IsDark() == targetPiece.IsDark())
                    {
                        continue;
                    }
                }

                bool valid = true;
                //List<Piece> enemyPieceList;
                if (piece.IsDark())
                {
                    //enemyPieceList = PieceManager.Instance.GetLightPieceList();
                    foreach (Piece enemy in PieceManager.Instance.GetLightPieceList())
                    {
                        foreach (GridPosition pos in enemy.GetPieceAction().GetValidActionGridPositionList())
                        {
                            if (pos.x == testGridPosition.x && pos.z == testGridPosition.z)
                            {
                                valid = false;
                                continue;
                            }
                        }
                    }
                }
                else
                {
                    //enemyPieceList = PieceManager.Instance.GetDarkPieceList();
                    foreach (Piece enemy in PieceManager.Instance.GetDarkPieceList())
                    {
                        foreach (GridPosition pos in enemy.GetPieceAction().GetValidActionGridPositionList())
                        {
                            if (pos.x == testGridPosition.x && pos.z == testGridPosition.z)
                            {
                                valid = false;
                                continue;
                            }
                        }
                    }
                }
                
                
                /*
                foreach (Piece enemy in enemyPieceList)
                {
                    if (enemy.GetPieceAction().GetValidActionGridPositionList().Contains(testGridPosition))
                    {
                        valid = false;
                        continue;
                    }
                }
                */

                if (!valid)
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }
        
        return validGridPositionList;
    }
}