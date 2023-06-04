using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureLogger : MonoBehaviour
{
    private void OnEnable()
    {
        GestureDetection.OnSwipeUp += SwipeUp;
        GestureDetection.OnSwipeDown += SwipeDown;
        GestureDetection.OnSwipeLeft += SwipeLeft;
        GestureDetection.OnSwipeRight += SwipeRight;
        GestureDetection.OnTap += Tap;
    }

    private void OnDisable()
    {
        GestureDetection.OnSwipeUp -= SwipeUp;
        GestureDetection.OnSwipeDown -= SwipeDown;
        GestureDetection.OnSwipeLeft -= SwipeLeft;
        GestureDetection.OnSwipeRight -= SwipeRight;
        GestureDetection.OnTap -= Tap;
    }

    private void SwipeUp(TouchData td)
    {
        Debug.Log("Swipe up registered!");
        //td.debugLogInfo();
    }

    private void SwipeDown(TouchData td)
    {
        Debug.Log("Swipe down registered!");
        //td.debugLogInfo();
    }

    private void SwipeLeft(TouchData td) {
        Debug.Log("Swipe left registered!");
        //td.debugLogInfo(); 
    }

    private void SwipeRight(TouchData td)
    {
        Debug.Log("Swipe right registered!");
        //td.debugLogInfo();
    }

    private void Tap(TouchData td)
    {
        td.debugLogInfo();
        Debug.Log("Tap registered!");
    }
}
