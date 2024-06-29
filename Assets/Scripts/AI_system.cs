using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AI_system : MonoBehaviour
{
    // Create static instance
    public static AI_system Instance { get; private set; } 

    // Event to destroy Text Boxes for ray detection of hypothetical moves.
    public static event EventHandler TestBoxDestroy;

    // Prefabs for Pawn Promotion
    [SerializeField] private Transform darkQueenPrefab;
    [SerializeField] private Transform queenPrefab;

    // Prefab for the aforementioned testBox.
    [SerializeField] private TestBox testBox;

    // List of float values for gauging the best move to make
    List<float> positionValues;

    // Reference to which actions and grid positions are connected to the best values.
    Dictionary<float, Piece> actionsByValues;
    Dictionary<float, GridPosition> positionsByValues;

    // enumerator for the various states of the AI.
    private enum State
    {
        WaitingForAITurn,
        TakingTurn,
        Action,
        Busy,
        GetOutOfCheck,
        GiveUp
    }

    // The state itself.
    private State state;

    // A timer to create timed delays
    private float timer;

    // The selected piece, its action, and target position
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
        state = State.WaitingForAITurn; // Sets the default state

        // Instantiate lists
        positionValues = new List<float>(); 
        actionsByValues = new Dictionary<float, Piece>();
        positionsByValues = new Dictionary<float, GridPosition>();
    }

    private void Start() 
    {
        // Subscribe to turn changing event and determine if the AI takes the first turn.
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
        // Do nothing if not an Ai turn...
        if (!TurnSystem.Instance.IsAITurn())
        {
            return;
        }

        // Run the state machine
        switch (state)
        {
            case State.WaitingForAITurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if (!CheckManager.Instance.IsInCheck())
                    {
                        List<Piece> pieceList;
                        if (TurnSystem.Instance.IsDarkTurn())
                        {
                            pieceList = PieceManager.Instance.GetDarkPieceList().OrderBy(x => UnityEngine.Random.value).ToList();
                        }
                        else
                        {
                            pieceList = PieceManager.Instance.GetLightPieceList().OrderBy(x => UnityEngine.Random.value).ToList();
                        }
                        CalculateAIAction(pieceList);
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

    private void CalculateAIAction(List<Piece> pieceList)
    {
        // Instantiate a new list for valid positions
        List<GridPosition> validPositions; 

        // Reset lists.
        positionValues.Clear();
        actionsByValues.Clear();
        positionsByValues.Clear();

        // Iterate over every piece of the AI team and calculate the values of its moves.
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
    }

    private bool TryInitiateAction()
    {
        // This method tries to initiate what it has selected as the best action and verifies that the move is legal, returning true if it is and executing the action.
        // Otherwise, it returns false.
        if (selectedPiece.GetPieceType() != "King")
        {
            if (selectedPiece.IsDark())
            {    
                if (selectedPieceAction.IsValidActionGridPosition(targetPosition))
                {
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
                    Instantiate(testBox, BoardGrid.Instance.GetWorldPosition(targetPosition), Quaternion.identity);
                    if (!PieceManager.Instance.GetLightKing().IsThreatened(targetPosition))
                    {
                        selectedPieceAction.TakeAction(targetPosition, ClearBusy);
                        return true;
                    }
                }
            }
            Invoke("TestBoxDestruction", 1f);
        }
        else if (selectedPieceAction.IsValidActionGridPosition(targetPosition))
        {
            if (!BoardGrid.Instance.IsThreatened(targetPosition, selectedPiece.IsDark()))
            {
                selectedPieceAction.TakeAction(targetPosition, ClearBusy);
                return true;
            }
            Invoke("TestBoxDestruction", 1f);
        }
        return false;
    }

    private void ClearBusy()
    {
        // This method emulates ClearBusy from the Piece Control System with some tweaks for AI.
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
        // This method simply lets the turn system know to end the turn.
        TurnSystem.Instance.NextTurn();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        // This method allows the AI to react correctly to the changing of turns.
        if (TurnSystem.Instance.IsAITurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }
    }

    public void TestBoxDestruction()
    {
        // This method was made simply for the purpose of being able to delay the event.
        TestBoxDestroy?.Invoke(this, EventArgs.Empty);
    }

    public void GiveUp()
    {
        // This method is called when checkmate is reached so the AI knows to stop.
        state = State.GiveUp;
    }

    private void SetSelection()
    {
        // This method correctly identifies the selected piece and sets the correct variables.
        selectedPiece = actionsByValues[positionValues[0]];
        selectedPieceAction = selectedPiece.GetPieceAction();
        PieceControlSystem.Instance.SetSelectedPiece(selectedPiece);
        targetPosition = positionsByValues[positionValues[0]];
    }

    private void TryNonKingFigures()
    {
        // This method is to check if any non-king figures can move to end a state of check
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
        // This method is to help the king determine its best move when necessary.
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
