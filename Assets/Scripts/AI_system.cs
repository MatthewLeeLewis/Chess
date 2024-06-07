using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AI_system : MonoBehaviour
{
    public static AI_system Instance { get; private set; } // This ensures that the instance of this object can be gotten publicly but cannot be set publicly.
    public static event EventHandler TestBoxDestroy;
    [SerializeField] private Transform darkQueenPrefab;
    [SerializeField] private Transform queenPrefab;
    [SerializeField] private TestBox testBox;

    List<float> positionValues;
    Dictionary<float, Piece> actionsByValues;
    Dictionary<float, GridPosition> positionsByValues;

    private enum State
    {
        WaitingForAITurn,
        TakingTurn,
        Action,
        Busy,
        GetOutOfCheck,
        GiveUp
    }

    private State state;
    private float timer;
    Piece selectedPiece;
    PieceAction selectedPieceAction;
    GridPosition targetPosition;

    private void Awake()
    {
        if (Instance != null) // This if check ensures that multiple instances of this object do not exist and reports it if they do, and destroys the duplicate.
        {
            Debug.LogError("There's more than one AI_system! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this; // This instantiates the instance.
        state = State.WaitingForAITurn;

        positionValues = new List<float>(); 
        actionsByValues = new Dictionary<float, Piece>();
        positionsByValues = new Dictionary<float, GridPosition>();
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
                    if (!UICanvas.Instance.IsInCheck())
                    {
                        CalculateAIAction();
                        state = State.Action;
                        timer = 1f;
                    }
                    else
                    {
                        TryNonKingFigures();
                        state = State.GetOutOfCheck;
                        timer = 1f;
                    }
                }
                break;
            case State.GetOutOfCheck:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if (positionValues.Count > 0)
                    {
                        state = State.Action;
                        timer = 1f;
                    }
                    else
                    {
                        CalculateKingMove();
                        state = State.Action;
                        timer = 1f;
                    }
                }
                break;
            case State.Action:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if (positionValues.Count > 1)
                    {
                        positionValues.Sort();
                        positionValues.Reverse();
                        SetSelection();
                    }
                    else if (positionValues.Count > 0)
                    {
                        SetSelection();
                    }
                    else
                    {
                        timer = 1f;
                        state = State.TakingTurn;
                    }

                    if (!TryInitiateAction())
                    {
                        if (positionValues.Count > 1)
                        {
                            positionValues.RemoveAt(0);
                            positionValues.Sort();
                            positionValues.Reverse();
                            SetSelection();
                        }
                        else if (positionValues.Count > 0)
                        {
                            positionValues.RemoveAt(0);
                        }
                        else
                        {
                            timer = 1f;
                            state = State.TakingTurn;
                        }

                        for (int i = 0; i < positionValues.Count; i ++)
                        {
                            if (!TryInitiateAction())
                            {
                                if (positionValues.Count > 1)
                                {
                                    positionValues.RemoveAt(0);
                                    positionValues.Sort();
                                    positionValues.Reverse();
                                    SetSelection();
                                }
                                else if (positionValues.Count > 0)
                                {
                                    positionValues.RemoveAt(0);
                                }
                                else
                                {
                                    timer = 1f;
                                    state = State.TakingTurn;
                                }
                            }
                            else
                            {
                                state = State.Busy;
                            }
                        }
                    }
                    else
                    {
                        state = State.Busy;
                    }
                }
                break;
            case State.Busy:
                break;
            case State.GiveUp:
                break;
        }
    }

    private void CalculateAIAction()
    {
        List<Piece> pieceList;
        List<GridPosition> validPositions; 

        positionValues.Clear();
        actionsByValues.Clear();
        positionsByValues.Clear();

        if (TurnSystem.Instance.IsDarkTurn())
        {
            pieceList = PieceManager.Instance.GetDarkPieceList().OrderBy(x => UnityEngine.Random.value).ToList();
        }
        else
        {
            pieceList = PieceManager.Instance.GetLightPieceList().OrderBy(x => UnityEngine.Random.value).ToList();
        }
        
        /*
        selectedPiece = pieceList[UnityEngine.Random.Range(0, pieceList.Count - 1)];
        selectedPieceAction = selectedPiece.GetPieceAction();
        PieceControlSystem.Instance.SetSelectedPiece(selectedPiece);
        */
        for (int i = 0; i < pieceList.Count; i++)
        {
            selectedPiece = pieceList[i];
            selectedPieceAction = selectedPiece.GetPieceAction();
            PieceControlSystem.Instance.SetSelectedPiece(selectedPiece);

            validPositions = selectedPieceAction.GetValidActionGridPositionList();
            
            
            for (int j = 0; j < validPositions.Count; j++)
            {
                
                float positionValue = BoardGrid.Instance.GetPositionValue(validPositions[j]);
                if (!positionValues.Contains(positionValue))
                {
                    positionValues.Add(positionValue);
                    positionsByValues.Add(positionValue, validPositions[j]);
                    actionsByValues.Add(positionValue, selectedPiece);
                }
            }
        }

        /*
        if (validPositions.Count > 0)
        {
            targetPosition = validPositions[UnityEngine.Random.Range(0, validPositions.Count - 1)];
        }
        else
        {
            CalculateAIAction();
        }*/
        

        //selectedPiece.GetPieceAction().TakeAction(targetPosition, ClearBusy);
    }

    private bool TryInitiateAction()
    {
        //selectedPiece.GetPieceAction().TakeAction(targetPosition, ClearBusy);
        Debug.Log("The selected piece is " + selectedPiece.GetPieceType());
        Debug.Log("It is moving to " + targetPosition.ToString());

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

    public void GiveUp()
    {
        state = State.GiveUp;
    }

    private void SetSelection()
    {
        selectedPiece = actionsByValues[positionValues[0]];
        selectedPieceAction = selectedPiece.GetPieceAction();
        PieceControlSystem.Instance.SetSelectedPiece(selectedPiece);
        targetPosition = positionsByValues[positionValues[0]];
    }

    private void TryNonKingFigures()
    {
        Piece threatPiece = PieceControlSystem.Instance.GetLastMoved();
        GridPosition threatPos = threatPiece.GetGridPosition();
        GridPosition kingPos;
        List<GridPosition> validPositions = new List<GridPosition>(); 
        
        if (TurnSystem.Instance.IsDarkTurn())
        {
            kingPos = PieceManager.Instance.GetDarkKing().GetGridPosition();
        }
        else
        {
            kingPos = PieceManager.Instance.GetLightKing().GetGridPosition();
        }
        
        List<Piece> pieceList;
        
        positionValues.Clear();
        actionsByValues.Clear();
        positionsByValues.Clear();

        if (TurnSystem.Instance.IsDarkTurn())
        {
            pieceList = PieceManager.Instance.GetDarkPieceList().OrderBy(x => UnityEngine.Random.value).ToList();
        }
        else
        {
            pieceList = PieceManager.Instance.GetLightPieceList().OrderBy(x => UnityEngine.Random.value).ToList();
        }

        for (int i = 0; i < pieceList.Count; i++)
        {
            if (pieceList[i].GetPieceType() == "King")
            {
                continue;
            }
            selectedPiece = pieceList[i];
            selectedPieceAction = selectedPiece.GetPieceAction();
            PieceControlSystem.Instance.SetSelectedPiece(selectedPiece);

            if (validPositions.Count > 0)
            {
                validPositions.Clear();
            }
            validPositions = selectedPieceAction.GetValidActionGridPositionList();
            
            
            for (int j = 0; j < validPositions.Count; j++)
            {
                
                float positionValue = BoardGrid.Instance.GetPositionValue(validPositions[j]);
                if (!positionValues.Contains(positionValue))
                {
                    if (validPositions[j] == threatPos)
                    {
                        positionValues.Add(positionValue);
                        positionsByValues.Add(positionValue, validPositions[j]);
                        actionsByValues.Add(positionValue, selectedPiece);
                    }
                    if (threatPiece.GetPieceType() == "Bishop" || threatPiece.GetPieceType() == "Queen")
                    {
                        if (validPositions[j].z - validPositions[j].x == kingPos.z - kingPos.x)
                        {
                            if (kingPos.x > threatPos.x && kingPos.z > threatPos.z)
                            {
                                if (validPositions[j].x > threatPos.x && validPositions[j].x < kingPos.x)
                                {
                                    if (validPositions[j].z > threatPos.z && validPositions[j].z < kingPos.z)
                                    {
                                        positionValues.Add(positionValue);
                                        positionsByValues.Add(positionValue, validPositions[j]);
                                        actionsByValues.Add(positionValue, selectedPiece);
                                        continue;
                                    }
                                }
                            }
                            else if (kingPos.x < threatPos.x && kingPos.z < threatPos.z)
                            {
                                if (validPositions[j].x < threatPos.x && validPositions[j].x > kingPos.x)
                                {
                                    if (validPositions[j].z < threatPos.z && validPositions[j].z > kingPos.z)
                                    {
                                        positionValues.Add(positionValue);
                                        positionsByValues.Add(positionValue, validPositions[j]);
                                        actionsByValues.Add(positionValue, selectedPiece);
                                        continue;
                                    }
                                }
                            }
                            else if (kingPos.x < threatPos.x && kingPos.z > threatPos.z)
                            {
                                if (validPositions[j].x < threatPos.x && validPositions[j].x > kingPos.x)
                                {
                                    if (validPositions[j].z > threatPos.z && validPositions[j].z < kingPos.z)
                                    {
                                        positionValues.Add(positionValue);
                                        positionsByValues.Add(positionValue, validPositions[j]);
                                        actionsByValues.Add(positionValue, selectedPiece);
                                        continue;
                                    }
                                }
                            }
                            else if (kingPos.x > threatPos.x && kingPos.z < threatPos.z)
                            {
                                if (validPositions[j].x > threatPos.x && validPositions[j].x < kingPos.x)
                                {
                                    if (validPositions[j].z < threatPos.z && validPositions[j].z > kingPos.z)
                                    {
                                        positionValues.Add(positionValue);
                                        positionsByValues.Add(positionValue, validPositions[j]);
                                        actionsByValues.Add(positionValue, selectedPiece);
                                        continue;
                                    }
                                }
                            }
                        }
                        else if (validPositions[j].z + validPositions[j].x == kingPos.z + kingPos.x) 
                        {
                            if (kingPos.x > threatPos.x && kingPos.z > threatPos.z)
                            {
                                if (validPositions[j].x > threatPos.x && validPositions[j].x < kingPos.x)
                                {
                                    if (validPositions[j].z > threatPos.z && validPositions[j].z < kingPos.z)
                                    {
                                        positionValues.Add(positionValue);
                                        positionsByValues.Add(positionValue, validPositions[j]);
                                        actionsByValues.Add(positionValue, selectedPiece);
                                        continue;
                                    }
                                }
                            }
                            if (kingPos.x < threatPos.x && kingPos.z < threatPos.z)
                            {
                                if (validPositions[j].x < threatPos.x && validPositions[j].x > kingPos.x)
                                {
                                    if (validPositions[j].z < threatPos.z && validPositions[j].z > kingPos.z)
                                    {
                                        positionValues.Add(positionValue);
                                        positionsByValues.Add(positionValue, validPositions[j]);
                                        actionsByValues.Add(positionValue, selectedPiece);
                                        continue;
                                    }
                                }
                            }
                            if (kingPos.x < threatPos.x && kingPos.z > threatPos.z)
                            {
                                if (validPositions[j].x < threatPos.x && validPositions[j].x > kingPos.x)
                                {
                                    if (validPositions[j].z > threatPos.z && validPositions[j].z < kingPos.z)
                                    {
                                        positionValues.Add(positionValue);
                                        positionsByValues.Add(positionValue, validPositions[j]);
                                        actionsByValues.Add(positionValue, selectedPiece);
                                        continue;
                                    }
                                }
                            }
                            if (kingPos.x > threatPos.x && kingPos.z < threatPos.z)
                            {
                                if (validPositions[j].x > threatPos.x && validPositions[j].x < kingPos.x)
                                {
                                    if (validPositions[j].z < threatPos.z && validPositions[j].z > kingPos.z)
                                    {
                                        positionValues.Add(positionValue);
                                        positionsByValues.Add(positionValue, validPositions[j]);
                                        actionsByValues.Add(positionValue, selectedPiece);
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                    if (threatPiece.GetPieceType() == "Rook" || threatPiece.GetPieceType() == "Queen")
                    {
                        if (kingPos.x == threatPos.x)
                        {
                            if (kingPos.z > threatPos.z)
                            {
                                if (validPositions[j].x == kingPos.x)
                                {
                                    if (validPositions[j].z > threatPos.z && validPositions[j].z < kingPos.z)
                                    {
                                        positionValues.Add(positionValue);
                                        positionsByValues.Add(positionValue, validPositions[j]);
                                        actionsByValues.Add(positionValue, selectedPiece);
                                        continue;
                                    }
                                }
                            }
                            if (kingPos.z < threatPos.z)
                            {
                                if (validPositions[j].x == kingPos.x)
                                {
                                    if (validPositions[j].z < threatPos.z && validPositions[j].z > kingPos.z)
                                    {
                                        positionValues.Add(positionValue);
                                        positionsByValues.Add(positionValue, validPositions[j]);
                                        actionsByValues.Add(positionValue, selectedPiece);
                                        continue;
                                    }
                                }
                            }
                        }
                        if (kingPos.z == threatPos.z)
                        {
                            if (kingPos.x > threatPos.x)
                            {
                                if (validPositions[j].z == kingPos.z)
                                {
                                    if (validPositions[j].x > threatPos.x && validPositions[j].x < kingPos.x)
                                    {
                                        positionValues.Add(positionValue);
                                        positionsByValues.Add(positionValue, validPositions[j]);
                                        actionsByValues.Add(positionValue, selectedPiece);
                                        continue;
                                    }
                                }
                            }
                            if (kingPos.x < threatPos.x)
                            {
                                if (validPositions[j].z == kingPos.z)
                                {
                                    if (validPositions[j].x < threatPos.x && validPositions[j].x > kingPos.x)
                                    {
                                        positionValues.Add(positionValue);
                                        positionsByValues.Add(positionValue, validPositions[j]);
                                        actionsByValues.Add(positionValue, selectedPiece);
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

    }

    private void CalculateKingMove()
    {
        GridPosition threatPos = PieceControlSystem.Instance.GetLastMoved().GetGridPosition();
        Piece kingPiece;
        List<GridPosition> validPositions; 

        positionValues.Clear();
        actionsByValues.Clear();
        positionsByValues.Clear();

        if (TurnSystem.Instance.IsDarkTurn())
        {
            kingPiece = PieceManager.Instance.GetDarkKing();
        }
        else
        {
            kingPiece = PieceManager.Instance.GetLightKing();
        }

        selectedPiece = kingPiece;
        selectedPieceAction = selectedPiece.GetPieceAction();
        PieceControlSystem.Instance.SetSelectedPiece(selectedPiece);

        validPositions = selectedPieceAction.GetValidActionGridPositionList();
            
        for (int j = 0; j < validPositions.Count; j++)
        {  
            float positionValue = UnityEngine.Random.Range(1f,50f);
            if (validPositions[j] == threatPos && BoardGrid.Instance.IsThreatened(validPositions[j], TurnSystem.Instance.IsDarkTurn()))
            {
                positionValue = 100f;
            }
            if (!positionValues.Contains(positionValue) && !BoardGrid.Instance.IsThreatened(validPositions[j], TurnSystem.Instance.IsDarkTurn()))
            {
                positionValues.Add(positionValue);
                positionsByValues.Add(positionValue, validPositions[j]);
                actionsByValues.Add(positionValue, selectedPiece);
            }
        }
    }
}
