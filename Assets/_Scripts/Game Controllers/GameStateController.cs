using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateController : MonoBehaviour
{
    public enum GameState { InGame, Paused, GameOver}

    public delegate void GameStateAction(GameState gameState);
    public static event GameStateAction OnGameOver;
    public static event GameStateAction OnPauseGame;
    public static event GameStateAction OnResumeGame;

    [SerializeField] private GameObject player;

    public void pauseGame()
    {
        //do pause game state stuff
        OnPauseGame?.Invoke(GameState.Paused);
    }

    public void resumeGame()
    {
        OnResumeGame?.Invoke(GameState.InGame);
    }

    public void gameOver()
    {
        //do game over state stuff

        Destroy(player.GetComponent<PlayerMovementController>());
        OnGameOver?.Invoke(GameState.GameOver);
    }

    public void reloadScene()
    {
        Debug.Log("Scene reloaded.");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex, LoadSceneMode.Single);
    }

    public void backtoMenu()
    {
      SceneManager.LoadScene("MenuScene");
    }
}
