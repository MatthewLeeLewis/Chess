using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public static PieceManager Instance { get; private set; }
    private Piece focusedPiece;

    private List<Piece> pieceList;
    private List<Piece> lightPieceList;
    private List<Piece> capturedLightList;
    private List<Piece> darkPieceList;
    private List<Piece> capturedDarkList;

    [SerializeField] Piece lightKing;
    [SerializeField] Piece darkKing;

    private void Awake()
    {
        if (Instance != null) // This if check ensures that multiple instances of this object do not exist and reports it if they do, and destroys the duplicate.
        {
            Debug.LogError("There's more than one PieceManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this; // This instantiates the instance.

        pieceList = new List<Piece>();
        lightPieceList = new List<Piece>();
        darkPieceList = new List<Piece>();
        focusedPiece = PieceControlSystem.Instance.GetSelectedPiece();
        capturedLightList = new List<Piece>();
        capturedDarkList = new List<Piece>();
    }

    private void Start()
    {
        Piece.OnAnyPieceSpawned += Piece_OnAnyPieceSpawned;
        Piece.OnAnyPieceDead += Piece_OnAnyPieceDead;
    }

    private void Piece_OnAnyPieceSpawned(object sender, EventArgs e)
    {
        Piece piece = sender as Piece;

        pieceList.Add(piece);

        if (piece.IsDark())
        {
            darkPieceList.Add(piece);
        } else
        {
            lightPieceList.Add(piece);
        }
    }

    private void Piece_OnAnyPieceDead(object sender, EventArgs e)
    {
        Piece piece = sender as Piece;

        pieceList.Remove(piece);
        
        if (piece.IsDark())
        {
            darkPieceList.Remove(piece);
            capturedDarkList.Add(piece);
        } else
        {
            lightPieceList.Remove(piece);
            capturedLightList.Add(piece);
        }
    }

    public List<Piece> GetPieceList()
    {
        return pieceList;
    }
    public List<Piece> GetLightPieceList()
    {
        return lightPieceList;
    }
    public List<Piece> GetDarkPieceList()
    {
        return darkPieceList;
    }

    public void SetFocusedPiece(Piece piece)
    {
        focusedPiece = piece;
    }

    public Piece GetFocusedPiece()
    {
        return focusedPiece;
    }

    public Piece GetLightKing()
    {
        return lightKing;
    }

    public Piece GetDarkKing()
    {
        return darkKing;
    }
}
