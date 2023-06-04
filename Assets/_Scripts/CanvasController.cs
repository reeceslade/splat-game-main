using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private Canvas inGameCanvas;
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private Canvas pauseCanavas;
    [SerializeField] private GameOverSplatterAnimation splatterAnimation;

    private void OnEnable()
    {
        GameStateController.OnGameOver += switchUI;
        GameStateController.OnPauseGame += switchUI;
        GameStateController.OnResumeGame += switchUI;
    }

    private void OnDisable()
    {
        GameStateController.OnGameOver -= switchUI;
        GameStateController.OnPauseGame -= switchUI;
        GameStateController.OnResumeGame -= switchUI;
    }

    private void Start()
    {
        switchUI(GameStateController.GameState.InGame);
    }

    private void switchUI(GameStateController.GameState gameState)
    {
        if(gameState == GameStateController.GameState.InGame)
        {
            inGameCanvas.enabled = true;
            gameOverCanvas.enabled = false;
            pauseCanavas.enabled = false;
        }
        else if (gameState == GameStateController.GameState.GameOver)
        {
            inGameCanvas.enabled = false;
            gameOverCanvas.enabled = true;
            splatterAnimation.playAnimation();
            pauseCanavas.enabled = false;
        }
        else
        {
            inGameCanvas.enabled = false;
            gameOverCanvas.enabled = false;
            pauseCanavas.enabled = true;
        }
    }
}
