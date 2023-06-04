using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private bool onlyFollowFwdMovements = false;

    [SerializeField] private float lerpTime;
    [SerializeField] public Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private bool followX = false;
    [SerializeField] private bool followY = true;

    private Transform playerTransform;
    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        Vector3 playerPos = playerTransform.position;
        Vector3 targetPos = Vector3.zero;
        if (followX)
        {
            targetPos = new Vector3(playerPos.x, targetPos.y);
        }
        if (followY)
        {
            if ((onlyFollowFwdMovements && playerPos.y > transform.position.y) || !onlyFollowFwdMovements)
            {
                targetPos = new Vector3(targetPos.x, playerPos.y);
            }
        }

        targetPos += offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpTime * Time.deltaTime);
    }
}
