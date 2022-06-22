using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text cherryText;
    public int score = 0;

    private void Start()
    {
        UpdateScore();
    }
    public void IncrementScore()
    {
        score++;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = score.ToString();
        cherryText.text = GetComponent<World>().Cherries.ToString();
    }
}
