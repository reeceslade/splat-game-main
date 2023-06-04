using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCountDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cointCounterText;
    [SerializeField] private TextMeshProUGUI increaseDisplay;
    [SerializeField] private float increaseDisplayTime = 5f;

    private float hideIncreaseDisplayTime;
    private float startIncreaseDisplayTime;

    private void OnEnable()
    {
        CoinCounter.OnCountIncrease += updateDisplay;
    }

    private void OnDisable()
    {
        CoinCounter.OnCountIncrease -= updateDisplay;
    }

    private void Update()
    {
        if(Time.time > hideIncreaseDisplayTime)
        {
            increaseDisplay.enabled = false;
        }
        else
        {
            float timeSinceDisplayStart = Time.time - startIncreaseDisplayTime;
            float opacity = (float)timeSinceDisplayStart / increaseDisplayTime;
            opacity = 1 - opacity;
            Color displayColor = new Color(1f, 0.813f, 0f, opacity);
            increaseDisplay.color = displayColor;
        }
    }

    private void updateDisplay(CoinIncrease ci)
    {
        cointCounterText.text = ci.totalAmount.ToString();
        //display amount coin count has been increase but hide after x seconds
        if(ci.increaseAmount > 0)
        {
            increaseDisplay.enabled = true;
            increaseDisplay.text = "+" + ci.increaseAmount.ToString();
            hideIncreaseDisplayTime = Time.time + increaseDisplayTime;
            startIncreaseDisplayTime = Time.time;
        }
    }
}
