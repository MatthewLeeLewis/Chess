using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * This script governs which pieces are selected and the execution of actions for those pieces.
 */

public class PieceControlSystem : MonoBehaviour
{
    public static PieceControlSystem Instance { get; private set; } // This ensures that the instance of this object can be gotten publicly but cannot be set publicly.
    public event EventHandler OnSelectedPieceChanged; // This is an event that allows subscribers to react to a selected piece being changed.
    public event EventHandler<bool> OnBusyChanged; 
    public event EventHandler OnActionStarted;
    public static event EventHandler TestBoxDestroy;
    [SerializeField] private Transform testBox_king;
    private GridPosition kingTestPos;


    [SerializeField] private Piece selectedPiece; // This tracks which piece is selected.
    [SerializeField] private LayerMask piecesLayerMask; // This is the pieces layer variable.

    private PieceAction selectedPieceAction;
    private bool isBusy; // Variable to track whether the control system is currently doing something or not.

    private void Awake() 
    {
        if (Instance != null) // This if check ensures that multiple instances of this object do not exist and reports it if they do, and destroys the duplicate.
        {
            Debug.LogError("There's more than one PieceControlSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this; // This instantiates the instance.
    }

    private void Start()
    {
        SetSelectedPiece(selectedPiece);
    }

    private void Update()
    {
        if (isBusy)
        {
            return;
        }

        /*
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        */

        /*
        if (EventSystem.current.IsPointerOverGameObject()) // Ensures the mouse cursor is not over a button...
        {
            return;
        }
        */

        if (TryHandlePieceSelection())
        {
            return; // Try to select a piece and end this event if one is selected.
        }

        if (selectedPiece.IsDark() == TurnSystem.Instance.IsDarkTurn())
        {
            HandleSelectedAction();
        }
    }

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = BoardGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (selectedPiece.GetPieceType() != "King")
            {
                if (selectedPiece.IsDark())
                {
                    if (selectedPieceAction.IsValidActionGridPosition(mouseGridPosition))
                    {
                        Transform testBox = selectedPiece.GetBox();
                        Instantiate(testBox, BoardGrid.Instance.GetWorldPosition(mouseGridPosition), Quaternion.identity);
                        if (!PieceManager.Instance.GetDarkKing().IsThreatened(mouseGridPosition))
                        {
                            SetBusy();
                            selectedPieceAction.TakeAction(mouseGridPosition, ClearBusy);
                            OnActionStarted?.Invoke(this, EventArgs.Empty);
                            TurnSystem.Instance.NextTurn();
                        }
                    }
                }
                else
                {
                    if (selectedPieceAction.IsValidActionGridPosition(mouseGridPosition))
                    {
                        Transform testBox = selectedPiece.GetBox();
                        Instantiate(testBox, BoardGrid.Instance.GetWorldPosition(mouseGridPosition), Quaternion.identity);
                        if (!PieceManager.Instance.GetLightKing().IsThreatened(mouseGridPosition))
                        {
                            SetBusy();
                            selectedPieceAction.TakeAction(mouseGridPosition, ClearBusy);
                            OnActionStarted?.Invoke(this, EventArgs.Empty);
                            TurnSystem.Instance.NextTurn();
                        }
                    }
                }
                TestBoxDestroy?.Invoke(this, EventArgs.Empty);
            }
            else if (selectedPieceAction.IsValidActionGridPosition(mouseGridPosition))
            {
                if (selectedPieceAction.IsValidActionGridPosition(mouseGridPosition))
                {
                    Instantiate(testBox_king, BoardGrid.Instance.GetWorldPosition(mouseGridPosition), Quaternion.identity);
                  
                    kingTestPos = mouseGridPosition;

                    Invoke("TestKingBox", 1f);
                    TestBoxDestroy?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    private void TestKingBox()
    {
        TestBox kingBoxTest = testBox_king.GetComponent<TestBox>();
        if (!kingBoxTest.IsThreatened(kingTestPos))
        {
            SetBusy();
            selectedPieceAction.TakeAction(kingTestPos, ClearBusy);
            OnActionStarted?.Invoke(this, EventArgs.Empty);
            TurnSystem.Instance.NextTurn();
        }
    }
    
    private void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);

        GridPosition gridPosition = selectedPiece.GetGridPosition();
        List<Piece> pieceList = BoardGrid.Instance.GetPieceListAtGridPosition(gridPosition);

        for (int i = 0; i <pieceList.Count; i++)
        {
            if (pieceList[i] != selectedPiece)
            {
                pieceList[i].Die();
            }
        }
    }

    private bool TryHandlePieceSelection() // Custom method to try to select a piece, and return whether it was successful or not.
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Instantiate a laser to determine mouse position.
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, piecesLayerMask)) // If it hits a piece (specifically an object on the pieces layer)...
            {
                if (raycastHit.transform.TryGetComponent<Piece>(out Piece piece)) // Check if the hit object is a piece...
                {
                    if (piece == selectedPiece)
                    {
                        // Piece is already selected
                        return false;
                    }

                    if (piece.IsDark() != TurnSystem.Instance.IsDarkTurn())
                    {
                        // Piece is an enemy
                        return false;
                    }
                    SetSelectedPiece(piece); // Select the hit piece
                    return true;
                }
            }
        }
        
        return false;
    }

    private void SetSelectedPiece(Piece piece) // Simple method to change the selected piece and trigger the event for if a selected piece was changed.
    {
        if (selectedPiece != null)
        {
            selectedPiece.EnableCollider();
        }
        
        selectedPiece = piece;
        SetSelectedAction(piece.GetPieceAction());
        selectedPiece.DeactivateCollider();
        OnSelectedPieceChanged?.Invoke(this, EventArgs.Empty); 
    }

    public void SetSelectedAction(PieceAction pieceAction)
    {
        selectedPieceAction = pieceAction; 
    }

    public Piece GetSelectedPiece() // Simple public method to return the currently selected piece.
    {
        return selectedPiece;
    }

    public PieceAction GetSelectedAction()
    {
        return selectedPieceAction;
    }

}
