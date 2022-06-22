using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Text cherryText;
    public Text highscoreText;

    private void Start()
    {
        PlayerData data = GetData();
        if(data != null)
        {
            cherryText.text = data.cherries.ToString();
            highscoreText.text = data.highscore.ToString();
        }
        else
        {
            cherryText.text = "0";
            highscoreText.text = "0";
        }
    }
    public PlayerData GetData()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null) return data;
        return null;
    }
    public void Begin()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
