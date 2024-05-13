using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PieceAction : MonoBehaviour // By making the base class abstract, instances of this class cannot be created, only subclasses.
{
    protected Piece piece; // Reference to the piece this action is attached to.
    protected bool isActive; // Variable to track if this action is active or not.
    protected Action onActionComplete; // Variable to track if the action is completed
    protected float moveSpeed = 4f;

    public event EventHandler OnStartMoving;
    protected Vector3 targetPosition; // Keeps track of the individual piece's target position.


    protected virtual void Awake()
    {
        piece = GetComponent<Piece>();
        targetPosition = transform.position; // Set the piece's default target position to its default position in the worldspace so it doesn't instantly move.
    }

    public void TakeAction(GridPosition gridPosition, Action onActionComplete) // public method to change its target position and trigger it to move.
    {
        ActionStart(onActionComplete);
        this.targetPosition = BoardGrid.Instance.GetWorldPosition(gridPosition);

        OnStartMoving?.Invoke(this, EventArgs.Empty);
    }

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public abstract bool IsValidKingPosition(GridPosition gridPosition);

    protected void ActionStart(Action onActionComplete) 
    {
        isActive = true;
        this.onActionComplete = onActionComplete;
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();
    }
}