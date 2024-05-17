using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI checkText;
    private void Awake()
    {
        checkText.enabled = false;
    }
    void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        PieceControlSystem.Instance.OnBusyChanged += PieceControlSystem_OnBusyChanged;
        PieceControlSystem.Instance.OnSelectedPieceChanged += PieceControlSystem_OnSelectedPieceChanged;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.IsDarkTurn())
        {
            turnText.text = "Dark Turn";
        }
        else
        {
            turnText.text = "Light Turn";
        }
    }

    private void PieceControlSystem_OnBusyChanged(object sender, EventArgs e)
    {
        CheckCheckText();
    }
    private void PieceControlSystem_OnSelectedPieceChanged(object sender, EventArgs e)
    {
        CheckCheckText();
    }

    public void CheckCheckText()
    {
        if (TurnSystem.Instance.IsDarkTurn())
        {
            GridPosition gridPosition = PieceManager.Instance.GetDarkKing().GetGridPosition();
            if (PieceManager.Instance.GetDarkKing().IsThreatened(gridPosition))
            {
                checkText.enabled = true;
            }
            else
            {
                checkText.enabled = false;
            }
        }
        else
        {
            GridPosition gridPosition = PieceManager.Instance.GetLightKing().GetGridPosition();
            if (PieceManager.Instance.GetLightKing().IsThreatened(gridPosition))
            {
                checkText.enabled = true;
            }
            else
            {
                checkText.enabled = false;
            }
        }
    }
}
