using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_system : MonoBehaviour
{
    public static event EventHandler TestBoxDestroy;
    [SerializeField] private Transform darkQueenPrefab;
    [SerializeField] private Transform queenPrefab;
    [SerializeField] private TestBox testBox;
    private enum State
    {
        WaitingForAITurn,
        TakingTurn,
        Action,
        Busy,
    }

    private State state;
    private float timer;
    Piece selectedPiece;
    PieceAction selectedPieceAction;
    GridPosition targetPosition;

    private void Awake()
    {
        state = State.WaitingForAITurn;
    }

    private void Start() 
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        if (TurnSystem.Instance.IsAITurn())
        {
            state = State.TakingTurn;
        }
        else
        {
            state = State.WaitingForAITurn;
        }
    }

    private void Update()
    {
        if (!TurnSystem.Instance.IsAITurn())
        {
            return;
        }

        switch (state)
        {
            case State.WaitingForAITurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    CalculateAIAction();
                    state = State.Action;
                    timer = 1f;
                }
                break;
            case State.Action:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if (!TryInitiateAction())
                    {
                        timer = 1f;
                        state = State.TakingTurn;
                    }
                    else
                    {
                        state = State.Busy;
                    }
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void CalculateAIAction()
    {
        List<Piece> pieceList;
        List<GridPosition> validPositions;

        if (TurnSystem.Instance.IsDarkTurn())
        {
            pieceList = PieceManager.Instance.GetDarkPieceList();
        }
        else
        {
            pieceList = PieceManager.Instance.GetLightPieceList();
        }
        
        selectedPiece = pieceList[UnityEngine.Random.Range(0, pieceList.Count - 1)];
        selectedPieceAction = selectedPiece.GetPieceAction();
        PieceControlSystem.Instance.SetSelectedPiece(selectedPiece);

        validPositions = selectedPieceAction.GetValidActionGridPositionList();
        if (validPositions.Count > 0)
        {
            targetPosition = validPositions[UnityEngine.Random.Range(0, validPositions.Count - 1)];
        }
        else
        {
            CalculateAIAction();
        }
        

        //selectedPiece.GetPieceAction().TakeAction(targetPosition, ClearBusy);
    }

    private bool TryInitiateAction()
    {
        //selectedPiece.GetPieceAction().TakeAction(targetPosition, ClearBusy);

        if (selectedPiece.GetPieceType() != "King")
        {
            if (selectedPiece.IsDark())
            {
                    
                if (selectedPieceAction.IsValidActionGridPosition(targetPosition))
                {
                    //Transform testBox = selectedPiece.GetBox();
                    Instantiate(testBox, BoardGrid.Instance.GetWorldPosition(targetPosition), Quaternion.identity);
                        
                    if (!PieceManager.Instance.GetDarkKing().IsThreatened(targetPosition))
                    {
                        selectedPieceAction.TakeAction(targetPosition, ClearBusy);
                        return true;
                    }
                }
            }
            else
            {
                if (selectedPieceAction.IsValidActionGridPosition(targetPosition))
                {
                    //Transform testBox = selectedPiece.GetBox();
                    // Instantiate(testBox, BoardGrid.Instance.GetWorldPosition(mouseGridPosition), Quaternion.identity);
                    Instantiate(testBox, BoardGrid.Instance.GetWorldPosition(targetPosition), Quaternion.identity);
                    if (!PieceManager.Instance.GetLightKing().IsThreatened(targetPosition))
                    {
                        selectedPieceAction.TakeAction(targetPosition, ClearBusy);
                        return true;
                    }
                }
            }
            Invoke("TestBoxDestruction", 1f);
            //TestBoxDestroy?.Invoke(this, EventArgs.Empty);
        }
        else if (selectedPieceAction.IsValidActionGridPosition(targetPosition))
        {
            if (!BoardGrid.Instance.IsThreatened(targetPosition, selectedPiece.IsDark()))
            {
                selectedPieceAction.TakeAction(targetPosition, ClearBusy);
                return true;
            }
            Invoke("TestBoxDestruction", 1f);
            //TestBoxDestroy?.Invoke(this, EventArgs.Empty);
        }
        return false;
    }

    private void ClearBusy()
    {
        GridPosition gridPosition = selectedPiece.GetGridPosition();
        List<Piece> pieceList = BoardGrid.Instance.GetPieceListAtGridPosition(gridPosition);

        for (int i = 0; i <pieceList.Count; i++)
        {
            if (pieceList[i] != selectedPiece)
            {
                pieceList[i].Die();
            }
        }

        PieceControlSystem.Instance.SetLastMoved(selectedPiece);

        int pawnPromotionGridZ = 7;
        if (TurnSystem.Instance.IsDarkTurn())
        {
            pawnPromotionGridZ = 0;
        }

        if (selectedPiece.GetPieceType() == "Pawn" && selectedPiece.GetGridPosition().z == pawnPromotionGridZ)
        {
            Piece newPiece = null;

            if (TurnSystem.Instance.IsDarkTurn())
            {
                newPiece = Instantiate(darkQueenPrefab, selectedPiece.GetWorldPosition(), Quaternion.identity).GetComponent<Piece>();
            }
            else
            {
                newPiece = Instantiate(queenPrefab, selectedPiece.GetWorldPosition(), Quaternion.identity).GetComponent<Piece>();
            }

            selectedPiece.Die();
            PieceControlSystem.Instance.SetLastMoved(newPiece);
            newPiece.GetPieceAction().SetHasMoved();
        }

        if (TurnSystem.Instance.IsDarkTurn())
        {
            PieceControlSystem.Instance.SetSelectedPiece(PieceManager.Instance.GetLightKing());
        }
        else
        {
            PieceControlSystem.Instance.SetSelectedPiece(PieceManager.Instance.GetDarkKing());
        }

        TestBoxDestruction();

        Invoke("NextTurn", 0.1f);
        timer = 0.5f;
        state = State.WaitingForAITurn;
    }

    private void NextTurn()
    {

        TurnSystem.Instance.NextTurn();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.IsAITurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }
    }

    public void TestBoxDestruction()
    {
        TestBoxDestroy?.Invoke(this, EventArgs.Empty);
    }
}
