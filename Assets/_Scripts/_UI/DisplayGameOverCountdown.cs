using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayGameOverCountdown : MonoBehaviour
{
    [SerializeField] private RectTransform barRectTransform;
    [SerializeField] private RectTransform barBackgroundRectTransform;
    [SerializeField] private TextMeshProUGUI warningMessage;

    private void Awake()
    {
        warningMessage.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        PlayerMovementTracker.OnGameOverCountDown += updateProgressBar;
        PlayerMovementTracker.OnCountdownReset += hideProgressBar;
    }

    private void OnDisable()
    {
        PlayerMovementTracker.OnGameOverCountDown -= updateProgressBar;
        PlayerMovementTracker.OnCountdownReset -= hideProgressBar;
    }

    private void hideProgressBar(float timeLeft)
    {
        barRectTransform.gameObject.SetActive(false);
        warningMessage.gameObject.SetActive(false);
        barBackgroundRectTransform.gameObject.SetActive(false);
    }

    private void updateProgressBar(float timeLeftBeforeGameOver)
    {
        if(!warningMessage.gameObject.activeSelf) {
            warningMessage.gameObject.SetActive(true);
        }
        barRectTransform.gameObject.SetActive(true);
        barBackgroundRectTransform.gameObject.SetActive(true);

        barRectTransform.localScale = Vector3.Lerp(barRectTransform.localScale, new Vector3(timeLeftBeforeGameOver, 1f), 1f);
    }
}
