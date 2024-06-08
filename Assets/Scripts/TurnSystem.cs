using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }
    public event EventHandler OnTurnChanged;

    private int turnNumber = 1;
    private bool isDarkTurn = false;
    [SerializeField] private bool lightIsAI = false;
    [SerializeField] private bool darkIsAI = false;
    
    private void Awake() 
    {
        if (Instance != null) // This if check ensures that multiple instances of this object do not exist and reports it if they do, and destroys the duplicate.
        {
            Debug.LogError("There's more than one TurnSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this; // This instantiates the instance.
    }

    public void NextTurn()
    {
        turnNumber++;
        isDarkTurn = !isDarkTurn;

        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public bool IsDarkTurn()
    {
        return isDarkTurn;
    }

    public bool IsAITurn()
    {
        if (isDarkTurn)
        {
            return darkIsAI;
        }
        else
        {
            return lightIsAI;
        }
    }

    public bool EitherIsAI()
    {
        if (darkIsAI || lightIsAI)
        {
            return true;
        }
        return false;
    }

    public bool IsDarkAI()
    {
        return darkIsAI;
    }

    public bool IsLightAI()
    {
        return lightIsAI;
    }
}