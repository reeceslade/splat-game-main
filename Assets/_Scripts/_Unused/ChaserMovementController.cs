using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserMovementController : MonoBehaviour
{
    private Transform playerTranform;
    private float maxOffset = -8f;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        playerTranform = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    private void OnEnable()
    {
        PlayerMovementTracker.OnGameOverCountDown += catchupToPlayer;
        PlayerMovementTracker.OnCountdownReset += disableSelf;
    }

    private void OnDisable()
    {
        PlayerMovementTracker.OnGameOverCountDown -= catchupToPlayer;
        PlayerMovementTracker.OnCountdownReset += disableSelf;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Sin(Time.time) * 0.66f, 0f);
    }

    private void catchupToPlayer(float percentTimeLeft)
    {
        //spriteRenderer.enabled = true;
        //transform.position = new Vector3(transform.position.x, playerTranform.position.y + (maxOffset * percentTimeLeft));
    }

    private void disableSelf(float ignore)
    {
        //spriteRenderer.enabled = false;
    }
}
