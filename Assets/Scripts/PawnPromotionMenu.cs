using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnPromotionMenu : MonoBehaviour
{

    public static PawnPromotionMenu Instance { get; private set; }
    [SerializeField] private Button queenButton;
    [SerializeField] private Button rookButton;
    [SerializeField] private Button bishopButton;
    [SerializeField] private Button knightButton;

    [SerializeField] private Transform queenPrefab;
    [SerializeField] private Transform rookPrefab;
    [SerializeField] private Transform bishopPrefab;
    [SerializeField] private Transform knightPrefab;

    [SerializeField] private Transform darkQueenPrefab;
    [SerializeField] private Transform darkRookPrefab;
    [SerializeField] private Transform darkBishopPrefab;
    [SerializeField] private Transform darkKnightPrefab;

    private void Awake()
    {
        if (Instance != null) // This if check ensures that multiple instances of this object do not exist and reports it if they do, and destroys the duplicate.
        {
            Debug.LogError("There's more than one PawnPromotionMenu! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this; // This instantiates the instance.
        this.gameObject.SetActive(false);
    }

    void Start()
    {
        queenButton.onClick.AddListener(() =>
        {
            PromotePawn("Queen");
        });
        rookButton.onClick.AddListener(() =>
        {
            PromotePawn("Rook");
        });
        bishopButton.onClick.AddListener(() =>
        {
            PromotePawn("Bishop");
        });
        knightButton.onClick.AddListener(() =>
        {
            PromotePawn("Knight");
        });
    }

    private void PromotePawn(string pieceType)
    {
        Piece newPiece = null;
        
        switch (pieceType)
        {
            case "Queen": 
                if (TurnSystem.Instance.IsDarkTurn())
                {
                    newPiece = Instantiate(darkQueenPrefab, PieceControlSystem.Instance.GetLastMoved().GetWorldPosition(), Quaternion.identity).GetComponent<Piece>();
                }
                else
                {
                    newPiece = Instantiate(queenPrefab, PieceControlSystem.Instance.GetLastMoved().GetWorldPosition(), Quaternion.identity).GetComponent<Piece>();
                }
                break;
            case "Rook":
                if (TurnSystem.Instance.IsDarkTurn())
                {
                    newPiece = Instantiate(darkRookPrefab, PieceControlSystem.Instance.GetLastMoved().GetWorldPosition(), Quaternion.identity).GetComponent<Piece>();
                }
                else
                {
                    newPiece = Instantiate(rookPrefab, PieceControlSystem.Instance.GetLastMoved().GetWorldPosition(), Quaternion.identity).GetComponent<Piece>();
                }
                break;
            case "Bishop":
                if (TurnSystem.Instance.IsDarkTurn())
                {
                    newPiece = Instantiate(darkBishopPrefab, PieceControlSystem.Instance.GetLastMoved().GetWorldPosition(), Quaternion.identity).GetComponent<Piece>();
                }
                else
                {
                    newPiece = Instantiate(bishopPrefab, PieceControlSystem.Instance.GetLastMoved().GetWorldPosition(), Quaternion.identity).GetComponent<Piece>();
                }
                break;
            case "Knight":
                if (TurnSystem.Instance.IsDarkTurn())
                {
                    newPiece = Instantiate(darkKnightPrefab, PieceControlSystem.Instance.GetLastMoved().GetWorldPosition(), Quaternion.identity).GetComponent<Piece>();
                }
                else
                {
                    newPiece = Instantiate(knightPrefab, PieceControlSystem.Instance.GetLastMoved().GetWorldPosition(), Quaternion.identity).GetComponent<Piece>();
                }
                break;
        }

        PieceControlSystem.Instance.GetLastMoved().Die();
        PieceControlSystem.Instance.SetLastMoved(newPiece);
        PieceControlSystem.Instance.StopPrompt();
        newPiece.GetPieceAction().SetHasMoved();
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }
}
