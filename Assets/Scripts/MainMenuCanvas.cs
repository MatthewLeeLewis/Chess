using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour
{
    [SerializeField] Button playBtn;
    [SerializeField] Button quitBtn;
    [SerializeField] TMP_Dropdown modeSelector;
    [SerializeField] TMP_Dropdown teamSelector;
    [SerializeField] GameObject settingsObject;

    private void Awake()
    {
        playBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync("ChessScene");
            SceneManager.MoveGameObjectToScene(settingsObject, SceneManager.GetSceneByName("ChessScene"));
        });
        quitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        modeSelector.onValueChanged.AddListener(delegate {
            if (modeSelector.value == 0)
            {
                GameSettingManager.Instance.SetIsPVP(false);
                teamSelector.interactable = true;
            }
            else
            {
                GameSettingManager.Instance.SetIsPVP(true);
                teamSelector.interactable = false;
            }
        });
        teamSelector.onValueChanged.AddListener(delegate {
            if (teamSelector.value == 0)
            {
                GameSettingManager.Instance.SetPlayerIsDark(false);
            }
            else
            {
                GameSettingManager.Instance.SetPlayerIsDark(true);
            }
        });
    }
}
