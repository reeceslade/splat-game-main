using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public delegate void PlayerMovementAction(Vector2 newPosition);
    public static event PlayerMovementAction OnValidMovement;

    private Vector3 targetPosition;
    //private Vector2 futurePos;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private float movementSpeed = 1.0f;

    private bool isMovementEnabled = true;
    private Queue<Vector3> futurePositionsQueue = new Queue<Vector3>();

    private void Start()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, targetPosition) < 0.01f && futurePositionsQueue.Count > 0)
        {
            targetPosition = futurePositionsQueue.Dequeue();
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        GestureDetection.OnTap += move;
        GestureDetection.OnSwipeUp += move;
        GestureDetection.OnSwipeDown += move;
        GestureDetection.OnSwipeLeft += move;
        GestureDetection.OnSwipeRight += move;

        GameStateController.OnGameOver += togglePlayerMovement;
        GameStateController.OnPauseGame += togglePlayerMovement;
        GameStateController.OnResumeGame += togglePlayerMovement;
    }

    private void OnDisable()
    {
        GestureDetection.OnTap -= move;
        GestureDetection.OnSwipeUp -= move;
        GestureDetection.OnSwipeDown -= move;
        GestureDetection.OnSwipeLeft -= move;
        GestureDetection.OnSwipeRight -= move;

        GameStateController.OnGameOver -= togglePlayerMovement;
        GameStateController.OnPauseGame -= togglePlayerMovement;
        GameStateController.OnResumeGame -= togglePlayerMovement;
    }

    private void togglePlayerMovement(GameStateController.GameState gameState)
    {
        if(gameState == GameStateController.GameState.GameOver || gameState == GameStateController.GameState.Paused)
        {
            isMovementEnabled = false;
        }
        else
        {
            isMovementEnabled = true;
        }
    }

    private void move(TouchData td)
    {
          if (isMovementEnabled)
          {

            if (td.distance < 1f)
            {
              Vector2 futurePos = targetPosition + new Vector3(0f, 1f);
              futurePos = new Vector2(Mathf.RoundToInt(futurePos.x), Mathf.RoundToInt(futurePos.y));
              if (checkPosIsEmpty(futurePos) && checkPosWithinBounds(futurePos))
              {
                  futurePositionsQueue.Enqueue(futurePos);
                  OnValidMovement?.Invoke(targetPosition);
              }
            }

            else if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
            {
                Vector2 futurePos = targetPosition + new Vector3(td.direction.x, td.direction.y);
                //Debug.Log(td.direction.x);
                //Debug.Log(td.direction.y);
                futurePos = new Vector2(Mathf.RoundToInt(futurePos.x), Mathf.RoundToInt(futurePos.y));
                //Debug.Log(futurePos);
                if (checkPosIsEmpty(futurePos) && checkPosWithinBounds(futurePos))
                {
                    futurePositionsQueue.Enqueue(futurePos);
                    OnValidMovement?.Invoke(targetPosition);
                }
            }
        }
    }

    private bool checkPosIsEmpty(Vector2 newPos)
    {
        if(Physics2D.OverlapCircleAll(newPos + new Vector2(0.5f, 0.5f), 0.1f, playerLayerMask).Length == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool checkPosWithinBounds(Vector2 newPos)
    {
        float maxOffset = (ChunkPoolController.chunkWidth) / 2f;
        if(newPos.x > maxOffset || newPos.x < -maxOffset)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPosition, 0.3f);
    }



}
