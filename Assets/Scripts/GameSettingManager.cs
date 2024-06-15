using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingManager : MonoBehaviour
{
    public static GameSettingManager Instance { get; private set; }
    private bool isPvP = false;
    private bool playerIsDark = false;

    private void Awake()
    {
        if (Instance != null) // This if check ensures that multiple instances of this object do not exist and reports it if they do, and destroys the duplicate.
        {
            Debug.LogError("There's more than one CastlingMenu! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this; // This instantiates the instance.
    }

    public bool IsPvP()
    {
        return isPvP;
    }

    public bool PlayerIsDark()
    {
        return playerIsDark;
    }

    public void SetPlayerIsDark(bool isDark)
    {
        playerIsDark = isDark;
    }

    public void SetIsPVP(bool isPVP)
    {
        isPvP = isPVP;
    }
}
