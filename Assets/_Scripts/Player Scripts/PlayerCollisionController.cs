using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    private GameStateController gameStateController;
    [SerializeField] private bool ignoreCollisions = false;

    private void Awake()
    {
        gameStateController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameStateController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Vehicle" && !ignoreCollisions)
        {
            gameStateController.gameOver();
        }
    }
}
