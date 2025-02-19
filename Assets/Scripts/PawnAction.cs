using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnAction : PieceAction // By making the base class abstract, instances of this class cannot be created, only subclasses.
{
    public event EventHandler OnStopMoving;
    [SerializeField] private LayerMask piecesLayerMask;
    private bool hasMoved = false;
    private PieceAction pieceAction;

    private bool enPassant = false;
    [SerializeField] private bool enPass = false;
    List<GridPosition> validGridPositionList = new List<GridPosition>();

    private bool movingTwo = false;
    [SerializeField] private bool justMovedTwo = false;

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
        validGridPositionList.Clear();

        GridPosition pieceGridPosition = piece.GetGridPosition();

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -2; z <= 2; z++)
            {
                enPassant = false;
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = pieceGridPosition + offsetGridPosition;

                Vector3 gridWorldPosition = BoardGrid.Instance.GetWorldPosition(testGridPosition);
                Vector3 pieceWorldPosition = BoardGrid.Instance.GetWorldPosition(pieceGridPosition);

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
                    movingTwo = true;
                    if (hasMoved)
                    {
                        movingTwo = false;
                        continue;
                    }
                    if (x != 0)
                    {
                        movingTwo = false;
                        continue;
                    }
                    else
                    {
                        movingTwo = true;
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
                    if (z == 0)
                        {
                            enPassant = false;
                            continue;
                        }

                    if (!BoardGrid.Instance.HasAnyPieceOnGridPosition(testGridPosition)) // if check to prevent movement to where another unit already is.
                    {
                        if (PieceControlSystem.Instance.GetLastMoved() == null)
                        {
                            enPassant = false;
                            continue;
                        }
                        else if (PieceControlSystem.Instance.GetLastMoved().GetGridPosition().x != (pieceGridPosition.x + 1) && PieceControlSystem.Instance.GetLastMoved().GetGridPosition().x != (pieceGridPosition.x - 1))
                        {
                            enPassant = false;
                            continue;
                        }
                        else if (PieceControlSystem.Instance.GetLastMoved().GetGridPosition().z != pieceGridPosition.z)
                        {
                            enPassant = false;
                            continue;
                        }
                        else if ((PieceControlSystem.Instance.GetLastMoved().GetPieceType() != "Pawn") || PieceControlSystem.Instance.GetLastMoved().IsDark() == piece.IsDark())
                        {
                            enPassant = false;
                            continue;
                        }
                        else if (!PieceControlSystem.Instance.GetLastMoved().GetPieceAction().JustMovedTwo())
                        {  
                            enPassant = false;
                            continue;
                        }
                        else
                        {
                            enPassant = true;
                        }
                    }
                    else
                    {
                        Piece targetPiece = BoardGrid.Instance.GetPieceAtGridPosition(testGridPosition);
                        if (piece.IsDark() == targetPiece.IsDark())
                        {
                            enPassant = false;
                            continue;
                        }
                    }
                }

                

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
                if (movingTwo)
                {
                    justMovedTwo = true;
                }
                else
                {
                    justMovedTwo = false;
                }
                if (enPassant)
                {
                    enPass = true;
                }
                else
                {
                    enPass = false;
                }
            }
        }
        return validGridPositionList;
    }

    public override List<GridPosition> GetTheoreticalActionGridPositionList(GridPosition gridPosition) // Function to test a unit's move distance against valid grid positions and only return a list of valid positions.
    {
        List<GridPosition> theoreticalGridPositionList = new List<GridPosition>();

        GridPosition pieceGridPosition = gridPosition;

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -2; z <= 2; z++)
            {
                enPassant = false;
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = pieceGridPosition + offsetGridPosition;

                Vector3 gridWorldPosition = BoardGrid.Instance.GetWorldPosition(testGridPosition);
                Vector3 pieceWorldPosition = BoardGrid.Instance.GetWorldPosition(pieceGridPosition);

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
                    movingTwo = true;
                    if (hasMoved)
                    {
                        movingTwo = false;
                        continue;
                    }
                    if (x != 0)
                    {
                        movingTwo = false;
                        continue;
                    }
                    else
                    {
                        movingTwo = true;
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
                    if (z == 0)
                        {
                            enPassant = false;
                            continue;
                        }

                    if (!BoardGrid.Instance.HasAnyPieceOnGridPosition(testGridPosition)) // if check to prevent movement to where another unit already is.
                    {
                        if (PieceControlSystem.Instance.GetLastMoved() == null)
                        {
                            enPassant = false;
                            continue;
                        }
                        if (PieceControlSystem.Instance.GetLastMoved().GetGridPosition().x != (pieceGridPosition.x + 1) && PieceControlSystem.Instance.GetLastMoved().GetGridPosition().x != (pieceGridPosition.x - 1))
                        {
                            enPassant = false;
                            continue;
                        }
                        if (PieceControlSystem.Instance.GetLastMoved().GetGridPosition().z != pieceGridPosition.z)
                        {
                            enPassant = false;
                            continue;
                        }
                        if ((PieceControlSystem.Instance.GetLastMoved().GetPieceType() != "Pawn") || PieceControlSystem.Instance.GetLastMoved().IsDark() == piece.IsDark())
                        {
                            enPassant = false;
                            continue;
                        }
                        else if (!PieceControlSystem.Instance.GetLastMoved().GetPieceAction().JustMovedTwo())
                        {
                            enPassant = false;
                            continue;
                        }
                        else
                        {
                            enPassant = true;
                        }
                    }
                    else
                    {
                        Piece targetPiece = BoardGrid.Instance.GetPieceAtGridPosition(testGridPosition);
                        if (piece.IsDark() == targetPiece.IsDark())
                        {
                            enPassant = false;
                            continue;
                        }
                    }
                }

                

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

                theoreticalGridPositionList.Add(testGridPosition);
                if (movingTwo)
                {
                    justMovedTwo = true;
                }
                else
                {
                    justMovedTwo = false;
                }
                if (enPassant)
                {
                    enPass = true;
                }
                else
                {
                    enPass = false;
                }
            }
        }
        return theoreticalGridPositionList;
    }

    private void PieceAction_OnStartMoving(object sender, EventArgs e)
    {
        hasMoved = true;
        if (enPass)
        {
            PieceControlSystem.Instance.GetLastMoved().Die();
        }
    }

    public override bool JustMovedTwo()
    {
        return justMovedTwo;
    }
}