using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICanvas : MonoBehaviour
{
    public static UICanvas Instance { get; private set; } // This ensures that the instance of this object can be gotten publicly but cannot be set publicly.
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI checkText;
    private bool isInCheck = false;

    private void Awake()
    {
        if (Instance != null) // This if check ensures that multiple instances of this object do not exist and reports it if they do, and destroys the duplicate.
        {
            Debug.LogError("There's more than one UICanvas! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this; // This instantiates the instance.
        checkText.enabled = false;
    }
    void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.IsDarkTurn())
        {
            turnText.text = "Dark Turn";
        
            GridPosition gridPosition = PieceManager.Instance.GetDarkKing().GetGridPosition();
            if (PieceManager.Instance.GetDarkKing().IsThreatened(gridPosition))
            {
                isInCheck = true;
                checkText.enabled = true;
                if (CheckCheckmate(true))
                {
                    checkText.text = "CHECKMATE!";
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
            turnText.text = "Light Turn";
        
            GridPosition gridPosition = PieceManager.Instance.GetLightKing().GetGridPosition();
            if (PieceManager.Instance.GetLightKing().IsThreatened(gridPosition))
            {
                isInCheck = true;
                checkText.enabled = true;
                if (CheckCheckmate(false))
                {
                    checkText.text = "CHECKMATE!";
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
                        /*
                        Vector2 testPosVector = new Vector2(gridPosition.x, gridPosition.z);
                        Vector2 kingPosVector = new Vector2(kingPos.x, kingPos.z);
                        Vector2 threatPosVector = new Vector2(threatPos.x, threatPos.z);*/

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
                        //Vector2 testPosVector = new Vector2(gridPosition.x, gridPosition.z);
                        //Vector2 kingPosVector = new Vector2(kingPos.x, kingPos.z);
                        //Vector2 threatPosVector = new Vector2(threatPos.x, threatPos.z);

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
