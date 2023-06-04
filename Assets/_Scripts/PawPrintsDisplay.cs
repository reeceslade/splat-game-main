using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class PawPrintsDisplay : MonoBehaviour
{

    private GameObject pawPrintsDisplay;
    private TextMeshProUGUI pawPrintsBalance;

    void Start()
    {
        UpdateBalance();
    }

    /**
     * Updates the displayed balance of pawprints in the scene view.
     */
    public void UpdateBalance()
    {
        int digitCount = PlayerPrefs.GetInt("coins").ToString().Length;

        RectTransform parentRectTransform = GetComponent<RectTransform>();
        Vector3 currentPosition = parentRectTransform.anchoredPosition3D;
        parentRectTransform.anchoredPosition3D = new Vector3
            (-92 + (20 * (digitCount-1)), 
            currentPosition.y, 
            currentPosition.z);

        pawPrintsDisplay = GameObject.Find("PawPrintsDisplay");
        pawPrintsBalance = pawPrintsDisplay.GetComponentInChildren<TextMeshProUGUI>();
        pawPrintsBalance.text = PlayerPrefs.GetInt("paw_prints").ToString();


    }
}
