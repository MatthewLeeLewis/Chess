using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAction : PieceAction // By making the base class abstract, instances of this class cannot be created, only subclasses.
{
    public event EventHandler OnStopMoving;
    public static event EventHandler TestBoxDestroy;
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
                bool valid = false;
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = pieceGridPosition + offsetGridPosition;

                if (!BoardGrid.Instance.IsValidGridPosition(testGridPosition)) // if check to prevent movement out of bounds of grid.
                {
                    continue; // exits current for iteration and moves onto the next one.
                }

                if (Mathf.Abs(x) == 2 && Mathf.Abs(z) == 1)
                {
                    valid = true;
                }

                if (Mathf.Abs(x) == 1 && Mathf.Abs(z) == 2)
                {
                    valid = true;
                }

                if (!valid)
                {
                    continue;
                }

                if (BoardGrid.Instance.HasAnyPieceOnGridPosition(testGridPosition)) // if check to prevent movement to where another unit already is.
                {
                    Piece targetPiece = BoardGrid.Instance.GetPieceAtGridPosition(testGridPosition);
                    if (piece.IsDark() == targetPiece.IsDark())
                    {
                        continue;
                    }
                }

                /*
                Vector3 gridWorldPosition = BoardGrid.Instance.GetWorldPosition(testGridPosition);
                bool validToKing = true;
                piece.DeactivateCollider();
                Transform testBox = piece.GetBox();
                Instantiate(testBox, gridWorldPosition, Quaternion.identity);

                Piece king;
                List<Piece> enemyPieceList;

                if (piece.IsDark())
                {
                    king = PieceManager.Instance.GetDarkKing();
                    enemyPieceList = PieceManager.Instance.GetLightPieceList();
                }
                else
                {
                    king = PieceManager.Instance.GetLightKing();
                    enemyPieceList = PieceManager.Instance.GetDarkPieceList();
                }
                GridPosition kingPos = king.GetGridPosition();

                foreach (Piece enemy in enemyPieceList)
                {
                    if (!enemy.GetPieceAction().IsValidKingPosition(kingPos))
                    {
                        TestBoxDestroy?.Invoke(this, EventArgs.Empty);
                        piece.EnableCollider();
                        validToKing = false;
                        continue;
                    }
                }

                if (!validToKing)
                {
                    TestBoxDestroy?.Invoke(this, EventArgs.Empty);
                    piece.EnableCollider();
                    continue;
                }

                piece.EnableCollider();
                TestBoxDestroy?.Invoke(this, EventArgs.Empty);*/
                
                validGridPositionList.Add(testGridPosition);
            }
        }
        TestBoxDestroy?.Invoke(this, EventArgs.Empty);
        return validGridPositionList;
    }

    public override bool IsValidKingPosition(GridPosition gridPosition)
    {
        return (!GetValidActionGridPositionList().Contains(gridPosition));
    }
}