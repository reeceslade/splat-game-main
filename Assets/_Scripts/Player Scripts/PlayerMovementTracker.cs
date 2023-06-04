using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTracker : MonoBehaviour
{
    public delegate void PlayerMovementTrackerAction(float timeLeftBeforeGameOver);
    public static event PlayerMovementTrackerAction OnGameOverCountDown;
    public static event PlayerMovementTrackerAction OnCountdownReset;

    //This script will listen for events from the player movement script
    //it will track how far forward the player has moved
    //it will also track how long the player has been staying still for

    private Vector2 previousPos;

    private float timeSincePlayerMovedForward;
    //safe zone time = amount of time player can not moved forward before count down starts
    private float safeZoneTime = 2f;
    //gameOverTime = amount of time after the safeZoneTime has run out where a count down will show and game over
    //when it reaches its value
    private float gameOverTime = 3f;

    private GameStateController gameStateController;
    private ScoreCounter scoreCounter;


    private bool isEnabled = true;


    private void Awake()
    {
        previousPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        gameStateController = gameController.GetComponent<GameStateController>();
        scoreCounter = gameController.GetComponent<ScoreCounter>();
    }

    private void Update()
    {
        //call countdown reset event if the timeSincePlayerMovedForward is now
        if(Time.timeSinceLevelLoad - timeSincePlayerMovedForward < 1f)
        {
            OnCountdownReset?.Invoke(1f);
        }

        if (isEnabled)
        {
            float gameOverTimestamp = timeSincePlayerMovedForward + safeZoneTime + gameOverTime;
            if (Time.timeSinceLevelLoad > timeSincePlayerMovedForward + safeZoneTime && Time.timeSinceLevelLoad < gameOverTimestamp)
            {
                //safe zone time has run out
                //calculate percentage of time left
                float timeLeftBeforeGameOver = calcPercentBetweenTimestamps(timeSincePlayerMovedForward + safeZoneTime, Time.timeSinceLevelLoad, gameOverTimestamp);
                timeLeftBeforeGameOver = 1 - timeLeftBeforeGameOver;
                //Debug.Log(timeLeftBeforeGameOver);
                OnGameOverCountDown?.Invoke(timeLeftBeforeGameOver);
            }
            else
            {
                if (Time.timeSinceLevelLoad > gameOverTimestamp)
                {
                    gameStateController.gameOver();
                }
            }
        }
    }

    private void OnEnable()
    {
        PlayerMovementController.OnValidMovement += trackMovement;
        GameStateController.OnGameOver += toggleState;
        GameStateController.OnPauseGame += toggleState;
        GameStateController.OnResumeGame += toggleState;
    }

    private void OnDisable()
    {
        PlayerMovementController.OnValidMovement -= trackMovement;
        GameStateController.OnGameOver -= toggleState;
        GameStateController.OnPauseGame -= toggleState;
        GameStateController.OnResumeGame -= toggleState;
    }

    private void toggleState(GameStateController.GameState gameState)
    {
        if(gameState == GameStateController.GameState.GameOver || gameState == GameStateController.GameState.Paused)
        {
            isEnabled = false;
        }
        else
        {
            isEnabled = true;
            timeSincePlayerMovedForward = Time.timeSinceLevelLoad;
        }
    }

    private void trackMovement(Vector2 newPos)
    {
        if(newPos.y > previousPos.y)
        {
            int distanceMoved = Mathf.CeilToInt(newPos.y - previousPos.y);
            timeSincePlayerMovedForward = Time.timeSinceLevelLoad;
            scoreCounter.increaseScore(distanceMoved);
        }
        previousPos.y = newPos.y;
    }

    private float calcPercentBetweenTimestamps(float startTime, float currentTime, float endTime)
    {
        float duration = endTime - startTime;
        float timeSinceStart = currentTime - startTime;
        float percentage = (float)(timeSinceStart / duration);
        return percentage;
    }
}
