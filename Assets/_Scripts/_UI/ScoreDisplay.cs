using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    //this script will listen for increases in the player's score and will update the UI to display the current score, their highscore & indicate which is higher

    [SerializeField] private TextMeshProUGUI scoreDisplay;
    [SerializeField] private TextMeshProUGUI highscoreDisplay;
    [SerializeField] private bool displayTextBefore;
 
    [SerializeField] private Color beatenScoreColour = Color.yellow;

    private void OnEnable()
    {
        ScoreCounter.OnScoreChange += displayScores;
    }

    private void OnDisable()
    {
        ScoreCounter.OnScoreChange -= displayScores;
    }

    private void displayScores(int score, int highscore)
    {
        string scoreDisplayText = score.ToString();
        string highscoreDisplayText = highscore.ToString();
        if (displayTextBefore)
        {
            scoreDisplayText = "Score: " + scoreDisplayText;
            highscoreDisplayText = "Highscore: " + highscoreDisplayText;
        }

        if(scoreDisplay != null)
        {
            scoreDisplay.text = scoreDisplayText;
        }
        if (highscoreDisplay != null)
        {
            highscoreDisplay.text = highscoreDisplayText;
        }

        //if highscore has been beaten set score text colour to beaten colour (default yellow)
        //otherwise set it to default colour
        if(score > highscore)
        {
            scoreDisplay.color = beatenScoreColour;
        }
    }
}
