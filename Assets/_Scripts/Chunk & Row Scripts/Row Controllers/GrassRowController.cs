using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassRowController : MonoBehaviour, IReshuffle
{
    private List<GameObject> staticObstaclePrefabs;
    private GameObject grassPrefab;

    private GameObject coinPrefab;
    private SpriteRenderer spriteRenderer;
    private int maxNumberOfObstacles = 4;
    private float chanceOfSpawningCoin = 0.10f;

    private string[] grassBlocks = { "Grass", "Grass2", "Grass3", "Grass4", "Grass5", "Grass6", "Grass7", "Grass8", "Grass9" };

    private List<GameObject> coins;

    private void Awake()
    {
        staticObstaclePrefabs = new List<GameObject>();

        //staticObstaclePrefabs.Add(Resources.Load("Prefabs/Static Obstacles/Static Obstacle") as GameObject);
        staticObstaclePrefabs.Add(Resources.Load("Prefabs/Static Obstacles/Tree") as GameObject);
        staticObstaclePrefabs.Add(Resources.Load("Prefabs/Static Obstacles/Rock") as GameObject);
        coinPrefab = Resources.Load("Prefabs/Coin") as GameObject;
        grassPrefab = Resources.Load("Prefabs/grassBlock") as GameObject;

        spriteRenderer = GetComponent<SpriteRenderer>();
        coins = new List<GameObject>();
    }

    private void Start()
    {
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Game Sprites/Level Sprites/Bold Grass/Grass");
        generateRow();
    }

    public void generateRow()
    {
        int numOfObstacles = 0;
        int i = 0;
        while(i < ChunkPoolController.chunkWidth)
        {
            Vector2 position = new Vector2(transform.position.x - (ChunkPoolController.chunkWidth / 2f) + i, transform.position.y);
            generateGrassBlock(position);
            if (Random.Range(0, 3) == 1 && numOfObstacles < maxNumberOfObstacles)
            {
                GameObject obstacle = Instantiate(staticObstaclePrefabs[Random.Range(0, staticObstaclePrefabs.Count)], position, Quaternion.identity);
                obstacle.transform.parent = transform;
                numOfObstacles++;
            }
            else if (doGenerateCoin())
            {
                GameObject coin = Instantiate(coinPrefab, position, Quaternion.identity);
                coins.Add(coin);
                coin.transform.parent = transform;
            }
            i++;          
        }
    }

    public void reshuffle()
    {
        //Debug.Log("Reshuffling!");
        List<float> newObstaclesXPositions = new List<float>();
        foreach(Transform child in transform)
        {
            //reactivate coins
            if (child.tag == "Coin")
            {
                child.gameObject.SetActive(true);
                child.GetComponent<CoinController>().setAmountRandom();
            }

            float newXPos = Random.Range(0, ChunkPoolController.chunkWidth);
            newXPos += 0.5f;
            while(newObstaclesXPositions.Contains(newXPos))
            {
                newXPos = Random.Range(0, ChunkPoolController.chunkWidth);
                newXPos += 0.5f;
            }
            newObstaclesXPositions.Add(newXPos);
            Vector2 newPos = new Vector2(-ChunkPoolController.chunkWidth / 2f + newXPos, transform.position.y);
            child.transform.position = newPos;

            for (int i = 0; i < ChunkPoolController.chunkWidth; i++)
            {
                Vector2 position = new Vector2(transform.position.x - (ChunkPoolController.chunkWidth / 2f) + i, transform.position.y);
                generateGrassBlock(position);
            }
        }

    }

    private bool doGenerateCoin()
    {       
        int denominator = Mathf.FloorToInt(1f / chanceOfSpawningCoin);
        if(Random.Range(0, denominator) == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void generateGrassBlock(Vector2 pos)
    {
        GameObject grassBlock = Instantiate(grassPrefab, pos, Quaternion.identity);
        SpriteRenderer grassPrefabSprite = grassBlock.GetComponent<SpriteRenderer>();
        grassPrefabSprite.sprite = Resources.Load<Sprite>("Sprites/Game Sprites/Level Sprites/Bold Grass/" + grassBlocks[Random.Range(0, grassBlocks.Length)]);
        
    }
}
