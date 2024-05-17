using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnAction : PieceAction // By making the base class abstract, instances of this class cannot be created, only subclasses.
{
    public event EventHandler OnStopMoving;
    public static event EventHandler TestBoxDestroy;
    [SerializeField] private LayerMask piecesLayerMask;
    private bool hasMoved = false;
    private PieceAction pieceAction;

    protected override void Awake()
    {
        base.Awake();
        pieceAction = this;
    }
    private void Start()
    {
        pieceAction.OnStartMoving += PieceAction_OnStartMoving;
    }
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
            for (int z = -2; z <= 2; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = pieceGridPosition + offsetGridPosition;

                if (!BoardGrid.Instance.IsValidGridPosition(testGridPosition)) // if check to prevent movement out of bounds of grid.
                {
                    continue; // exits current for iteration and moves onto the next one.
                }

                if (x == 0)
                {
                    if (BoardGrid.Instance.HasAnyPieceOnGridPosition(testGridPosition)) // if check to prevent movement to where another unit already is.
                    {
                        continue; // exits current for iteration and moves onto the next one.
                    }
                }

                if (z > 1 || z < -1)
                {
                    if (hasMoved)
                    {
                        continue;
                    }
                    if (x != 0)
                    {
                        continue;
                    }
                }

                if (z > 0)
                {
                    if (piece.IsDark())
                    {
                        continue;
                    }
                }

                if (z < 1)
                {
                    if (!piece.IsDark())
                    {
                        continue;
                    }
                }

                if (x != 0)
                {
                    if (!BoardGrid.Instance.HasAnyPieceOnGridPosition(testGridPosition)) // if check to prevent movement to where another unit already is.
                    {
                        continue; // exits current for iteration and moves onto the next one.
                    }
                    if (z == 0)
                    {
                        continue;
                    }
                    else
                    {
                        Piece targetPiece = BoardGrid.Instance.GetPieceAtGridPosition(testGridPosition);
                        if (piece.IsDark() == targetPiece.IsDark())
                        {
                            continue;
                        }
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

                /*

                bool valid = true;
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
                        valid = false;
                        continue;
                    }
                }

                if (!valid)
                {
                    TestBoxDestroy?.Invoke(this, EventArgs.Empty);
                    piece.EnableCollider();
                    continue;
                }
                
                piece.EnableCollider();
                TestBoxDestroy?.Invoke(this, EventArgs.Empty);
                */
                validGridPositionList.Add(testGridPosition);
            }
        }
        TestBoxDestroy?.Invoke(this, EventArgs.Empty);
        return validGridPositionList;
    }

    private void PieceAction_OnStartMoving(object sender, EventArgs e)
    {
        hasMoved = true;
        TestBoxDestroy?.Invoke(this, EventArgs.Empty);
    }

    public override bool IsValidKingPosition(GridPosition gridPosition)
    {
        GridPosition pieceGridPosition = piece.GetGridPosition();
        if (piece.IsDark())
        {
            if (gridPosition.x - pieceGridPosition.x == 1 || pieceGridPosition.x - gridPosition.x == 1)
            {
                if (pieceGridPosition.z - gridPosition.z == 1)
                {
                    Debug.Log("Dark Pawn stopped King from moving");
                    return false;
                }
            }
        }
        else
        {
            if (gridPosition.x - pieceGridPosition.x == 1 || pieceGridPosition.x - gridPosition.x == 1)
            {
                if (gridPosition.z - pieceGridPosition.z == 1)
                {
                    Debug.Log("Light Pawn stopped King from moving");
                    return false;
                }
            }
        }
        return true;
    }
}