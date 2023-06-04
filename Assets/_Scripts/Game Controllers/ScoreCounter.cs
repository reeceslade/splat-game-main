using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public delegate void ScoreCounterAction(int score, int highscore);
    public static event ScoreCounterAction OnScoreChange;

    public int score { get; private set; }
    public int highscore { get; private set; }

    private void OnEnable()
    {
        GameStateController.OnGameOver += updateHighscore;
    }

    private void OnDisable()
    {
        GameStateController.OnGameOver -= updateHighscore;  
    }

    private void Awake()
    {
        highscore = getHighscore();
    }

    public int getHighscore()
    {
        int highscore = 0;
        if(PlayerPrefs.HasKey("highscore"))
        {
            highscore = PlayerPrefs.GetInt("highscore");
        }
        return highscore;
    }

    public void updateHighscore(GameStateController.GameState gameState)
    {
        if(score > highscore)
        {
            PlayerPrefs.SetInt("highscore", score);
        }
    }

    public void increaseScore(int increase)
    {
        score += increase;
        OnScoreChange?.Invoke(score, highscore);
    }


}
