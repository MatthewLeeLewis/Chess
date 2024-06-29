using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// This script controls the user interface of the game itself as well as handling check/checkmate detection and functionality.

public class CheckManager : MonoBehaviour
{
    public static CheckManager Instance { get; private set; } // This ensures that the instance of this object can be gotten publicly but cannot be set publicly.
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI checkText;
    private bool isInCheck = false;
    [SerializeField] private Button backToMenuBtn;

    

    private void Awake()
    {
        if (Instance != null) // This if check ensures that multiple instances of this object do not exist and reports it if they do, and destroys the duplicate.
        {
            Debug.LogError("There's more than one CheckManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this; // This instantiates the instance.
        checkText.enabled = false;

        backToMenuBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync("MainMenu");
        });
    }
    void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        if (TurnSystem.Instance.EitherIsAI() && TurnSystem.Instance.IsDarkAI())
        {
            turnText.text = "Your Turn";
        }
        else if (TurnSystem.Instance.EitherIsAI() && !TurnSystem.Instance.IsDarkAI())
        {
            turnText.text = "";
        }
        else
        {
            turnText.text = "Light Turn";
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.IsDarkTurn())
        {
            if (TurnSystem.Instance.EitherIsAI() && !TurnSystem.Instance.IsDarkAI())
            {
                turnText.text = "Your Turn";
            }
            else if (TurnSystem.Instance.EitherIsAI() && TurnSystem.Instance.IsDarkAI())
            {
                turnText.text = "";
            }
            else
            {
                turnText.text = "Dark Turn";
            }
        
            GridPosition gridPosition = PieceManager.Instance.GetDarkKing().GetGridPosition();
            if (PieceManager.Instance.GetDarkKing().IsThreatened(gridPosition))
            {
                isInCheck = true;
                checkText.enabled = true;
                if (TurnSystem.Instance.EitherIsAI() && !TurnSystem.Instance.IsDarkAI())
                {
                    checkText.text = "You're in Check!";
                }
                else if (TurnSystem.Instance.EitherIsAI() && TurnSystem.Instance.IsDarkAI())
                {
                    checkText.text = "";
                }
                else
                {
                    checkText.text = "Dark is in Check!";
                }
                if (CheckCheckmate(true))
                {
                    if (TurnSystem.Instance.EitherIsAI() && !TurnSystem.Instance.IsDarkAI())
                    {
                        checkText.text = "CHECKMATE! You Lose...";
                    }
                    else if (TurnSystem.Instance.EitherIsAI() && TurnSystem.Instance.IsDarkAI())
                    {
                        checkText.text = "CHECKMATE! You Win!!!";
                    }
                    else 
                    {
                        checkText.text = "CHECKMATE! Light Wins!!!";
                    }
                    
                    AI_system.Instance.GiveUp();
                }
                Invoke("EnableThreatCollider", 1f);

            }
            else
            {
                isInCheck = false;
                checkText.enabled = false;
            }
        }
        else
        {
            if (TurnSystem.Instance.EitherIsAI() && TurnSystem.Instance.IsDarkAI())
            {
                turnText.text = "Your Turn";
            }
            else if (TurnSystem.Instance.EitherIsAI() && !TurnSystem.Instance.IsDarkAI())
            {
                turnText.text = "";
            }
            else
            {
                turnText.text = "Light Turn";
            }
        
            GridPosition gridPosition = PieceManager.Instance.GetLightKing().GetGridPosition();
            if (PieceManager.Instance.GetLightKing().IsThreatened(gridPosition))
            {
                isInCheck = true;
                checkText.enabled = true;
                if (TurnSystem.Instance.EitherIsAI() && TurnSystem.Instance.IsDarkAI())
                {
                    checkText.text = "You're in Check!";
                }
                else if (TurnSystem.Instance.EitherIsAI() && !TurnSystem.Instance.IsDarkAI())
                {
                    checkText.text = "";
                }
                else
                {
                    checkText.text = "Light is in Check!";
                }
                if (CheckCheckmate(false))
                {
                    if (TurnSystem.Instance.EitherIsAI() && TurnSystem.Instance.IsDarkAI())
                    {
                        checkText.text = "CHECKMATE! You Lose...";
                    }
                    else if (TurnSystem.Instance.EitherIsAI() && !TurnSystem.Instance.IsDarkAI())
                    {
                        checkText.text = "CHECKMATE! You Win!!!";
                    }
                    else 
                    {
                        checkText.text = "CHECKMATE! Dark Wins!!!";
                    }
                    AI_system.Instance.GiveUp();
                }
                Invoke("EnableThreatCollider", 1f);
            }
            else
            {
                isInCheck = false;
                checkText.enabled = false;
            }
        }
    }

    // The following function checks the entire team's moves to verify checkmate.
    private bool CheckCheckmate(bool isDarkCheck)
    {
        if (isDarkCheck)
        {
            GridPosition threatPos = PieceControlSystem.Instance.GetLastMoved().GetGridPosition();
            GridPosition kingPos = PieceManager.Instance.GetDarkKing().GetGridPosition();
            if (PieceManager.Instance.GetDarkKing().GetPieceAction().GetValidActionGridPositionList().Count > 0)
            {
                return false;
            }
            else
            {
                PieceControlSystem.Instance.GetLastMoved().DeactivateCollider();
            }
            for(int i = 0; i < PieceManager.Instance.GetDarkPieceList().Count; i++)
            {
                if (PieceManager.Instance.GetLightPieceList()[i].GetPieceType() == "King")
                {
                    continue;
                }
                if (PieceManager.Instance.GetDarkPieceList()[i].GetPieceAction().GetValidActionGridPositionList().Contains(threatPos))
                {
                    return false;
                }
                if (PieceControlSystem.Instance.GetLastMoved().GetPieceType() == "Rook" || PieceControlSystem.Instance.GetLastMoved().GetPieceType() == "Queen")
                {
                    foreach (GridPosition gridPosition in (PieceManager.Instance.GetDarkPieceList()[i].GetPieceAction().GetValidActionGridPositionList()))
                    {
                        if (kingPos.x == threatPos.x)
                        {
                            if (kingPos.z > threatPos.z)
                            {
                                if (gridPosition.x == kingPos.x)
                                {
                                    if (gridPosition.z > threatPos.z && gridPosition.z < kingPos.z)
                                    {
                                        return false;
                                    }
                                }
                            }
                            if (kingPos.z < threatPos.z)
                            {
                                if (gridPosition.x == kingPos.x)
                                {
                                    if (gridPosition.z < threatPos.z && gridPosition.z > kingPos.z)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                        if (kingPos.z == threatPos.z)
                        {
                            if (kingPos.x > threatPos.x)
                            {
                                if (gridPosition.z == kingPos.z)
                                {
                                    if (gridPosition.x > threatPos.x && gridPosition.x < kingPos.x)
                                    {
                                        return false;
                                    }
                                }
                            }
                            if (kingPos.x < threatPos.x)
                            {
                                if (gridPosition.z == kingPos.z)
                                {
                                    if (gridPosition.x < threatPos.x && gridPosition.x > kingPos.x)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                if (PieceControlSystem.Instance.GetLastMoved().GetPieceType() == "Bishop" || PieceControlSystem.Instance.GetLastMoved().GetPieceType() == "Queen")
                {
                    foreach (GridPosition gridPosition in (PieceManager.Instance.GetDarkPieceList()[i].GetPieceAction().GetValidActionGridPositionList()))
                    {
                        if (gridPosition.z - gridPosition.x == kingPos.z - kingPos.x)
                        {
                            if (kingPos.x > threatPos.x && kingPos.z > threatPos.z)
                            {
                                if (gridPosition.x > threatPos.x && gridPosition.x < kingPos.x)
                                {
                                    if (gridPosition.z > threatPos.z && gridPosition.z < kingPos.z)
                                    {
                                        
                                        return false;
                                    }
                                }
                            }
                            if (kingPos.x < threatPos.x && kingPos.z < threatPos.z)
                            {
                                if (gridPosition.x < threatPos.x && gridPosition.x > kingPos.x)
                                {
                                    if (gridPosition.z < threatPos.z && gridPosition.z > kingPos.z)
                                    {
                                        
                                        return false;
                                    }
                                }
                            }
                            if (kingPos.x < threatPos.x && kingPos.z > threatPos.z)
                            {
                                if (gridPosition.x < threatPos.x && gridPosition.x > kingPos.x)
                                {
                                    if (gridPosition.z > threatPos.z && gridPosition.z < kingPos.z)
                                    {
                                        
                                        return false;
                                    }
                                }
                            }
                            if (kingPos.x > threatPos.x && kingPos.z < threatPos.z)
                            {
                                if (gridPosition.x > threatPos.x && gridPosition.x < kingPos.x)
                                {
                                    if (gridPosition.z < threatPos.z && gridPosition.z > kingPos.z)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                        else if (gridPosition.z + gridPosition.x == kingPos.z + kingPos.x) 
                        {
                            if (kingPos.x > threatPos.x && kingPos.z > threatPos.z)
                            {
                                if (gridPosition.x > threatPos.x && gridPosition.x < kingPos.x)
                                {
                                    if (gridPosition.z > threatPos.z && gridPosition.z < kingPos.z)
                                    {
                                        return false;
                                    }
                                }
                            }
                            if (kingPos.x < threatPos.x && kingPos.z < threatPos.z)
                            {
                                if (gridPosition.x < threatPos.x && gridPosition.x > kingPos.x)
                                {
                                    if (gridPosition.z < threatPos.z && gridPosition.z > kingPos.z)
                                    {
                                        return false;
                                    }
                                }
                            }
                            if (kingPos.x < threatPos.x && kingPos.z > threatPos.z)
                            {
                                if (gridPosition.x < threatPos.x && gridPosition.x > kingPos.x)
                                {
                                    if (gridPosition.z > threatPos.z && gridPosition.z < kingPos.z)
                                    {
                                        return false;
                                    }
                                }
                            }
                            if (kingPos.x > threatPos.x && kingPos.z < threatPos.z)
                            {
                                if (gridPosition.x > threatPos.x && gridPosition.x < kingPos.x)
                                {
                                    if (gridPosition.z < threatPos.z && gridPosition.z > kingPos.z)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                        
                    }
                }
            }
        }
        else
        {
            GridPosition threatPos = PieceControlSystem.Instance.GetLastMoved().GetGridPosition();
            GridPosition kingPos = PieceManager.Instance.GetLightKing().GetGridPosition();
            if (PieceManager.Instance.GetLightKing().GetPieceAction().GetValidActionGridPositionList().Count > 0)
            {
                return false;
            }
            else
            {
                PieceControlSystem.Instance.GetLastMoved().DeactivateCollider();
            }
            for(int i = 0; i < PieceManager.Instance.GetLightPieceList().Count; i++)
            {
                if (PieceManager.Instance.GetLightPieceList()[i].GetPieceType() == "King")
                {
                    continue;
                }
                if (PieceManager.Instance.GetLightPieceList()[i].GetPieceAction().GetValidActionGridPositionList().Contains(threatPos))
                {
                    return false;
                }
                if (PieceControlSystem.Instance.GetLastMoved().GetPieceType() == "Rook" || PieceControlSystem.Instance.GetLastMoved().GetPieceType() == "Queen")
                {
                    foreach (GridPosition gridPosition in (PieceManager.Instance.GetLightPieceList()[i].GetPieceAction().GetValidActionGridPositionList()))
                    {
                        if (kingPos.x == threatPos.x)
                        {
                            if (kingPos.z > threatPos.z)
                            {
                                if (gridPosition.x == kingPos.x)
                                {
                                    if (gridPosition.z > threatPos.z && gridPosition.z < kingPos.z)
                                    {
                                        return false;
                                    }
                                }
                            }
                            if (kingPos.z < threatPos.z)
                            {
                                if (gridPosition.x == kingPos.x)
                                {
                                    if (gridPosition.z < threatPos.z && gridPosition.z > kingPos.z)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                        if (kingPos.z == threatPos.z)
                        {
                            if (kingPos.x > threatPos.x)
                            {
                                if (gridPosition.z == kingPos.z)
                                {
                                    if (gridPosition.x > threatPos.x && gridPosition.x < kingPos.x)
                                    {
                                        return false;
                                    }
                                }
                            }
                            if (kingPos.x < threatPos.x)
                            {
                                if (gridPosition.z == kingPos.z)
                                {
                                    if (gridPosition.x < threatPos.x && gridPosition.x > kingPos.x)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                if (PieceControlSystem.Instance.GetLastMoved().GetPieceType() == "Bishop" || PieceControlSystem.Instance.GetLastMoved().GetPieceType() == "Queen")
                {
                    foreach (GridPosition gridPosition in (PieceManager.Instance.GetLightPieceList()[i].GetPieceAction().GetValidActionGridPositionList()))
                    {
                        if (gridPosition.z - gridPosition.x == kingPos.z - kingPos.x)
                        {
                            if (kingPos.x > threatPos.x && kingPos.z > threatPos.z)
                            {
                                if (gridPosition.x > threatPos.x && gridPosition.x < kingPos.x)
                                {
                                    if (gridPosition.z > threatPos.z && gridPosition.z < kingPos.z)
                                    {
                                        return false;
                                    }
                                }
                            }
                            if (kingPos.x < threatPos.x && kingPos.z < threatPos.z)
                            {
                                if (gridPosition.x < threatPos.x && gridPosition.x > kingPos.x)
                                {
                                    if (gridPosition.z < threatPos.z && gridPosition.z > kingPos.z)
                                    {
                                        return false;
                                    }
                                }
                            }
                            if (kingPos.x < threatPos.x && kingPos.z > threatPos.z)
                            {
                                if (gridPosition.x < threatPos.x && gridPosition.x > kingPos.x)
                                {
                                    if (gridPosition.z > threatPos.z && gridPosition.z < kingPos.z)
                                    {
                                        return false;
                                    }
                                }
                            }
                            if (kingPos.x > threatPos.x && kingPos.z < threatPos.z)
                            {
                                if (gridPosition.x > threatPos.x && gridPosition.x < kingPos.x)
                                {
                                    if (gridPosition.z < threatPos.z && gridPosition.z > kingPos.z)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                        else if (gridPosition.z + gridPosition.x == kingPos.z + kingPos.x) 
                        {
                            if (kingPos.x > threatPos.x && kingPos.z > threatPos.z)
                            {
                                if (gridPosition.x > threatPos.x && gridPosition.x < kingPos.x)
                                {
                                    if (gridPosition.z > threatPos.z && gridPosition.z < kingPos.z)
                                    {
                                        return false;
                                    }
                                }
                            }
                            if (kingPos.x < threatPos.x && kingPos.z < threatPos.z)
                            {
                                if (gridPosition.x < threatPos.x && gridPosition.x > kingPos.x)
                                {
                                    if (gridPosition.z < threatPos.z && gridPosition.z > kingPos.z)
                                    {
                                        return false;
                                    }
                                }
                            }
                            if (kingPos.x < threatPos.x && kingPos.z > threatPos.z)
                            {
                                if (gridPosition.x < threatPos.x && gridPosition.x > kingPos.x)
                                {
                                    if (gridPosition.z > threatPos.z && gridPosition.z < kingPos.z)
                                    {
                                        return false;
                                    }
                                }
                            }
                            if (kingPos.x > threatPos.x && kingPos.z < threatPos.z)
                            {
                                if (gridPosition.x > threatPos.x && gridPosition.x < kingPos.x)
                                {
                                    if (gridPosition.z < threatPos.z && gridPosition.z > kingPos.z)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return true;
    }

    private void EnableThreatCollider()
    {
        PieceControlSystem.Instance.GetLastMoved().EnableCollider();
    }

    public bool IsInCheck()
    {
        return isInCheck;
    }
}
