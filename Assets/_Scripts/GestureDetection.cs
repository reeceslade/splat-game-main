using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GestureDetection : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;

    private float touchStartTimestamp;
    private float touchDuration;

    public delegate void TouchDetectorAction(TouchData td);
    public static event TouchDetectorAction OnSwipeUp;
    public static event TouchDetectorAction OnSwipeDown;
    public static event TouchDetectorAction OnSwipeLeft;
    public static event TouchDetectorAction OnSwipeRight;
    public static event TouchDetectorAction OnTap;

    [SerializeField] private float swipeDeadzone = 3f;


    private void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                startPos = touch.position;
                touchStartTimestamp = Time.time;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endPos = touch.position;
                touchDuration = Time.time - touchStartTimestamp;
                analyseTouchEvent();
            }
        }
    }

    private void analyseTouchEvent()
    {
        TouchData td = new TouchData(startPos, endPos, touchDuration);


        if (EventSystem.current.IsPointerOverGameObject(0)) { return; }

        if (td.distance < swipeDeadzone)
        {
            td.setDirection(Vector2.zero);
            OnTap?.Invoke(td);
        }
        else
        {
            float xDiff = endPos.x - startPos.x;
            float yDiff = endPos.y - startPos.y;

            float normalisedXDiff = xDiff;
            if (normalisedXDiff < 0)
            {
                normalisedXDiff *= -1f;
            }
            float normalisedYDiff = yDiff;
            if (normalisedYDiff < 0)
            {
                normalisedYDiff *= -1f;
            }

            //if y axis swipe (up or down)
            if (normalisedYDiff > normalisedXDiff)
            {
                if (yDiff > 0)
                {
                    td.debugLogInfo();
                    td.setDirection(Vector2.up);
                    OnSwipeUp?.Invoke(td);
                }
                else
                {
                    td.setDirection(Vector2.down);
                    OnSwipeDown?.Invoke(td);
                }
            }
            //if x axis swipe (left or right)
            else
            {
                if (xDiff > 0)
                {
                    td.setDirection(Vector2.right);
                    OnSwipeRight?.Invoke(td);
                }
                else
                {
                    td.setDirection(Vector2.left);
                    OnSwipeLeft?.Invoke(td);
                }
            }
        }

    }
}

public class TouchData
{
    public Vector2 startPos { get; private set; }
    public Vector2 endPos { get; private set; }

    public float duration { get; private set; }
    public float distance { get; private set; }

    public Vector2 direction { get; private set; }


    public TouchData(Vector2 startPos, Vector2 endPos, float duration)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        this.duration = duration;
        distance = Vector2.Distance(startPos, endPos);
    }

    public void setDirection(Vector2 direction) { this.direction = direction; }

    public void debugLogInfo()
    {
        Debug.Log("=== Debug Touch Data ===");
        Debug.Log("start pos: " + startPos);
        Debug.Log("end pos: " + endPos);
        Debug.Log("duration: " + duration);
        Debug.Log("distance: " + distance);
        Debug.Log("direction: " + direction.ToString());
    }
}
