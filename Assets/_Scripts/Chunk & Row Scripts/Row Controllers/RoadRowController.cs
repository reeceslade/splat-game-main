using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This script is added to a row instance to make it a road.
 * It is responsible for generating coins and instantiating the vehicle chain for its row
 */
public class RoadRowController : MonoBehaviour, IReshuffle
{
    private SpriteRenderer spriteRenderer;
    private List<GameObject> coins;
    private GameObject coinPrefab;
    private float chanceOfSpawningCoin = 0.10f;
    private int maxCoins = 3;

    private GameObject vehicleChainPrefab;
    private GameObject vehicleChainInstance;

    private void Awake()
    {
        //set sprite to road sprite
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Game Sprites/Level Sprites/Road");
        //get coin prefab
        coinPrefab = Resources.Load("Prefabs/Coin") as GameObject;
        //get chain vehicle prefab
        vehicleChainPrefab = Resources.Load<GameObject>("Prefabs/Vehicle Chain");
    }

    private void Start()
    {
        generateRow();
    }

    /**
     * This method is called when the gameobject is done instantiating.
     * It will randomly place coins across the row and instantiate an instance of the vehicleChainPrefab
     */
    public void generateRow()
    {
        //place coins randomly
        coins = new List<GameObject>();
        int numOfCoins = 0;
        int x = 0;
        while (x < ChunkPoolController.chunkWidth && numOfCoins < maxCoins)
        {
            Vector2 position = new Vector2(transform.position.x - (ChunkPoolController.chunkWidth / 2f) + x, transform.position.y);
            if(doGenerateCoin())
            {
                GameObject coin = Instantiate(coinPrefab, position, Quaternion.identity);
                coins.Add(coin);
                coin.transform.parent = transform;
                numOfCoins++;
            }
            x++;
        }

        //create vehicle chain
        vehicleChainInstance = Instantiate(vehicleChainPrefab, transform.position, Quaternion.identity);
        vehicleChainInstance.transform.parent = transform;
    }

    /**
     * This method is called by the ChunkController when it reshuffles, it loops through every coin instance it generated and 
     * repositions it randomly. It also destroys the previous vehicle chain instance and makes a new one to make the row completely different
     * from its preivous state.
     */
    public void reshuffle()
    {
        //reshuffle coins
        List<float> newRandomXPositions = new List<float>();
        foreach(var coin in coins)
        {
            coin.gameObject.SetActive(true);
            coin.GetComponent<CoinController>().setAmountRandom();

            float newXPos;
            do
            {
                newXPos = Random.Range(0, ChunkPoolController.chunkWidth);
                newXPos += 0.5f;
            } while (newRandomXPositions.Contains(newXPos));
            newRandomXPositions.Add(newXPos);
            Vector2 newPos = new Vector2(-ChunkPoolController.chunkWidth / 2f + newXPos, transform.position.y);
            coin.transform.position = newPos;
        }
        //regenerate car chain
        Destroy(vehicleChainInstance);
        vehicleChainInstance = Instantiate(vehicleChainPrefab, transform.position, Quaternion.identity);
        vehicleChainInstance.transform.parent = transform;
    }

    private bool doGenerateCoin()
    {
        int denominator = Mathf.FloorToInt(1f / chanceOfSpawningCoin);
        if (Random.Range(0, denominator) == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
