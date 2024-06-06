using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingAction : PieceAction // By making the base class abstract, instances of this class cannot be created, only subclasses.
{
    public event EventHandler OnStopMoving;
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
                
                //bool valid = true;
                List<Piece> enemyPieceList;
                if (piece.IsDark())
                {
                    enemyPieceList = PieceManager.Instance.GetLightPieceList();
                    /*
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
                    }*/
                }
                else
                {
                    enemyPieceList = PieceManager.Instance.GetDarkPieceList();
                    /*
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
                    }*/
                }
                
                
                /*
                foreach (Piece enemy in enemyPieceList)
                {
                    
                    if (!enemy.GetPieceAction().IsValidKingPosition(testGridPosition))
                    {
                        valid = false;
                        continue;
                    }
                }
                
                if (!valid)
                {
                    continue;
                }*/

                //************************************

                /*
                float heightDisplacement = 0.6f;
                GridPosition eastPosOffset = new GridPosition(1, 0);
                GridPosition eastPos_grid = gridPosition + eastPosOffset;
                Vector3 eastPos = BoardGrid.Instance.GetWorldPosition(eastPos_grid);
                
                Vector3 east = (eastPos - pieceWorldPosition).normalized;
                if (Physics.Raycast(
                        pieceWorldPosition + Vector3.up * heightDisplacement,
                        east,
                        out RaycastHit eastHit,
                        32f,
                        piecesLayerMask))
                {
                    if (eastHit.transform.TryGetComponent<Piece>(out Piece hitPiece)))
                    {
                        if (hitPiece.GetPieceType() != "Rook" || hitPiece.IsDark() != pieceGridPosition.isDark())
                        {
                            continue;
                        }
                    }
                }*/
                
                
                if (BoardGrid.Instance.IsThreatened(testGridPosition, piece.IsDark()))
                {
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
                
                //bool valid = true;
                List<Piece> enemyPieceList;
                if (piece.IsDark())
                {
                    enemyPieceList = PieceManager.Instance.GetLightPieceList();
                    /*
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
                    }*/
                }
                else
                {
                    enemyPieceList = PieceManager.Instance.GetDarkPieceList();
                    /*
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
                    }*/
                }
                
                
                /*
                foreach (Piece enemy in enemyPieceList)
                {
                    
                    if (!enemy.GetPieceAction().IsValidKingPosition(testGridPosition))
                    {
                        valid = false;
                        continue;
                    }
                }
                
                if (!valid)
                {
                    continue;
                }*/

                //************************************

                /*
                float heightDisplacement = 0.6f;
                GridPosition eastPosOffset = new GridPosition(1, 0);
                GridPosition eastPos_grid = gridPosition + eastPosOffset;
                Vector3 eastPos = BoardGrid.Instance.GetWorldPosition(eastPos_grid);
                
                Vector3 east = (eastPos - pieceWorldPosition).normalized;
                if (Physics.Raycast(
                        pieceWorldPosition + Vector3.up * heightDisplacement,
                        east,
                        out RaycastHit eastHit,
                        32f,
                        piecesLayerMask))
                {
                    if (eastHit.transform.TryGetComponent<Piece>(out Piece hitPiece)))
                    {
                        if (hitPiece.GetPieceType() != "Rook" || hitPiece.IsDark() != pieceGridPosition.isDark())
                        {
                            continue;
                        }
                    }
                }*/
                
                
                if (BoardGrid.Instance.IsThreatened(testGridPosition, piece.IsDark()))
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }
        
        return validGridPositionList;
    }

    public override bool IsValidKingPosition(GridPosition gridPosition)
    {
        return (!GetValidActionGridPositionList().Contains(gridPosition));
    }

    private void PieceAction_OnStartMoving(object sender, EventArgs e)
    {
        hasMoved = true;
    }

    public override bool HasMoved()
    {
        return hasMoved;
    }

    public override void SetHasMoved()
    {
        hasMoved = true;
    }
}