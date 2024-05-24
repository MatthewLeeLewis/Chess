using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastlingMenu : MonoBehaviour
{
    public static CastlingMenu Instance { get; private set; }
    [SerializeField] Piece castlingRook;
    [SerializeField] private Button castleButton;
    [SerializeField] private Button selectButton;

    private void Awake()
    {
        if (Instance != null) // This if check ensures that multiple instances of this object do not exist and reports it if they do, and destroys the duplicate.
        {
            Debug.LogError("There's more than one CastlingMenu! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this; // This instantiates the instance.
        this.gameObject.SetActive(false);
    }

    private void Start()
    {
        castleButton.onClick.AddListener(() =>
        {
            Castle();
        });
        selectButton.onClick.AddListener(() =>
        {
            PieceControlSystem.Instance.SetSelectedPiece(castlingRook);
            PieceControlSystem.Instance.StopPrompt();
            this.gameObject.SetActive(false);
        });
    }

    public void SetRook(Piece piece)
    {
        castlingRook = piece;
    }

    private void Castle()
    {
        int zPos = 0;
        if (TurnSystem.Instance.IsDarkTurn())
        {
            zPos = 14;
        }

        if (PieceControlSystem.Instance.GetSelectedPiece().GetGridPosition().x > castlingRook.GetGridPosition().x)
        {
            PieceControlSystem.Instance.GetSelectedPiece().transform.position = new Vector3(4f, 0f, zPos);
            castlingRook.transform.position = new Vector3(6f, 0f, zPos);
        }
        else
        {
            PieceControlSystem.Instance.GetSelectedPiece().transform.position = new Vector3(12f, 0f, zPos);
            castlingRook.transform.position = new Vector3(10f, 0f, zPos);
        }

        Invoke("VerifyCastling", 0.1f);
    }

    private void VerifyCastling()
    {
        if (PieceControlSystem.Instance.GetSelectedPiece().IsThreatened(PieceControlSystem.Instance.GetSelectedPiece().GetGridPosition()))
        {
            float zPos = 0f;
            if (TurnSystem.Instance.IsDarkTurn())
            {
                zPos = 14f;
            }

            if (PieceControlSystem.Instance.GetSelectedPiece().GetGridPosition().x > castlingRook.GetGridPosition().x)
            {
                PieceControlSystem.Instance.GetSelectedPiece().transform.position = new Vector3(8f, 0f, zPos);
                castlingRook.transform.position = new Vector3(14f, 0f, zPos);
            }
            else
            {
                PieceControlSystem.Instance.GetSelectedPiece().transform.position = new Vector3(8f, 0f, zPos);
                castlingRook.transform.position = new Vector3(0f, 0f, zPos);
            }
            PieceControlSystem.Instance.StopPrompt();
            this.gameObject.SetActive(false);
        }
        else
        {
            TurnSystem.Instance.NextTurn();
            castlingRook.GetPieceAction().SetHasMoved();
            PieceControlSystem.Instance.GetSelectedPiece().GetPieceAction().SetHasMoved();
            PieceControlSystem.Instance.StopPrompt();
            this.gameObject.SetActive(false);
        }
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }
}
