
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CoinDisplay : MonoBehaviour
{
    private GameObject coinDisplay;
    private TextMeshProUGUI coinBalance;

    void Start()
    {
        UpdateBalance();
    }

    /**
    * Updates the displayed balance of coins in the scene view.
    */
    public void UpdateBalance ()
    {
        coinDisplay = GameObject.Find("CoinBalance");
        coinBalance = coinDisplay.GetComponentInChildren<TextMeshProUGUI>();
        coinBalance.text = PlayerPrefs.GetInt("coins").ToString();
    }
}
