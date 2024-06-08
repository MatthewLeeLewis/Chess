using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectedVisual : MonoBehaviour
{
    [SerializeField] private Piece piece;
    private MeshRenderer meshRenderer;
    
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        PieceControlSystem.Instance.OnSelectedPieceChanged += PieceControlSystem_OnSelectedPieceChanged;
        UpdateVisual();
    }

    private void PieceControlSystem_OnSelectedPieceChanged(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual() // Calculate the visual based on which unit is selected.
    {
        if (PieceControlSystem.Instance.GetSelectedPiece() == piece) // If the unit this is attached to matches the one selected by the unit action system...
        {
            if (piece.IsDark() && !TurnSystem.Instance.IsDarkAI())
            {
                meshRenderer.enabled = true; // Make it invisible.
            }
            else if (!piece.IsDark() && !TurnSystem.Instance.IsLightAI())
            {
                meshRenderer.enabled = true; // Make it invisible.
            }
            else
            {
                meshRenderer.enabled = false;
            }
        }
        else // Otherwise..
        {
            meshRenderer.enabled = false; // Make it invisible.
        }
    }
}
