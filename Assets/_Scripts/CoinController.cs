using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * This script is a components of each instance of the coin prefab
 * It assigns and stores its value which is randomly picked from possibleAmounts adjusted for the weights given
 */
public class CoinController : MonoBehaviour
{
    [SerializeField] private List<KeyValuePair<int, int>> possibleAmounts = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(1, 60), new KeyValuePair<int, int>(5, 30), new KeyValuePair<int, int>(10, 10)};
    /* possible amounts:
     *  1 - 60%
     *  5 - 30%
     *  10 - 10%
     */

    private int amount;
    private CoinCounter coinCounter;

    private void Awake()
    {
        coinCounter = GameObject.FindGameObjectWithTag("GameController").GetComponent<CoinCounter>();
    }

    private void Start()
    {
        setAmountRandom();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //Debug.Log("Player just picked up coin with the value: " + amount.ToString());
            coinCounter.increaseCoinCount(amount);
            gameObject.SetActive(false);
        }
    }

    public void setAmountRandom()
    {
        amount = getRandomAmount();
    }

    private int getRandomAmount()
    {
        // Calculate the total weight of all strings
        int totalWeight = 0;
        foreach(var pair in possibleAmounts)
        {
            totalWeight += pair.Value;
        }

        // Generate a random number between 0 and the total weight
        int randomNumber = Random.Range(0, totalWeight);

        // Loop through the strings and subtract their weights from the random number until we find the selected string
        foreach(var pair in possibleAmounts) {
            randomNumber -= pair.Value;
            if(randomNumber < 0)
            {
                return pair.Key;
            }
        }
        return 0;
    }
}
