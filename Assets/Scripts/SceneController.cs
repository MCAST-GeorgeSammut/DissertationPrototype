using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] public GameModeOption gameModeOption;
    public enum GameModeOption
    {
        Static,
        Dynamic
    };

    public void PlayGame()
    {
        if (gameModeOption == GameModeOption.Static)
        {
            PlayerPrefs.SetInt("DynamicModeEnabled",0);
        }

        else if(gameModeOption == GameModeOption.Dynamic)
        {
            PlayerPrefs.SetInt("DynamicModeEnabled", 1);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    } 
}
