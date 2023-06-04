using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public delegate void CoinCounterEvent(CoinIncrease ci);
    public static event CoinCounterEvent OnCountIncrease;

    private void Start()
    {
        //display coins from player prefs
        OnCountIncrease?.Invoke(new CoinIncrease(0, getCoinsStored()));
    }

    public static int getCoinsStored()
    {
        int coinsStored = 0;
        if(PlayerPrefs.HasKey("coins"))
        {
            coinsStored = PlayerPrefs.GetInt("coins");
        }
        return coinsStored;
    }

    public void increaseCoinCount(int increaseAmount)
    {
        PlayerPrefs.SetInt("coins", getCoinsStored() + increaseAmount);
        OnCountIncrease?.Invoke(new CoinIncrease(increaseAmount, getCoinsStored()));
    }
}

public class CoinIncrease
{
    public int increaseAmount { get; private set; }
    public int totalAmount { get; private set; }

    public CoinIncrease(int increaseAmount, int totalAmount)
    {
        this.increaseAmount = increaseAmount;
        this.totalAmount = totalAmount;
    }
}
