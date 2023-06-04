using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * This script is responsible for instantiating instances of the chunk prefab and "reshuffling" them as the player moves forward to make
 * the game "seem" infinite, almost like a convery belt or treadmill. The number of chunk instances will always been the same and it makes the game
 * more efficient by not needing to load and unload gameobjects from memory which will slow down the game.
 */
public class ChunkPoolController : MonoBehaviour
{
    public const int chunkHeight = 16;
    public const int chunkWidth = 9;

    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private int poolSize = 4;
    [SerializeField] private int reshuffleCutOff = -2;

    private Transform playerTransform;
    private List<GameObject> chunkInstances;

    private int lastPlayerChunkPos = -1;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        instantiateChunks();
    }

    private void Update()
    {
        calcChunksToReshuffle();
    }

    /**
     * This method takes the latest player y coord & divides it by the chunkHeight.
     * if it is greater than the last coord the player has moved across a chunk boundary and chunks need to be reshuffled
     * It does this by calculating the distance (in "ChunkHeights") from the player for each chunk
     * If the distances is greater than a set cut off they are added to a keyValuePair array
     * Onces all the chunk distances are calculated the y position of the chunk the furthest forward is taken as a
     * parameter for the reshuffleChunks method as well as the furthest chunks away from the player
     */
    private void calcChunksToReshuffle()
    {
        int playerYpos = Mathf.FloorToInt(playerTransform.position.y) / chunkHeight;
        if (playerYpos > lastPlayerChunkPos)
        {
            lastPlayerChunkPos = playerYpos;
            //Player crossed over to new chunk

            //calculate distances from player occupied chunk
            KeyValuePair<GameObject, int>[] distances = calcDistancesFromPlayer(playerYpos);
            //get furthest chunk on the +y axis
            KeyValuePair<GameObject, int> furthestPosChunk = maxVal(distances);
            //chunks more than [reshuffleCutOff] chunks away
            KeyValuePair<GameObject, int>[] chunksToReshuffle = valsUnderInt(distances, reshuffleCutOff);
            reshuffleChunks(chunksToReshuffle, furthestPosChunk.Key.transform.position.y);
        }
    }

    /**
     * This method creates the instances for the chunkPrefabs and adds them to the chunkInstances list for future reference. 
     */
    private void instantiateChunks()
    {
        chunkInstances = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            float yPos = i * chunkHeight;
            GameObject chunkInstance = Instantiate(chunkPrefab, new Vector3(0f, yPos), Quaternion.identity);
            chunkInstance.name = "chunk " + i.ToString();
            chunkInstance.transform.parent = transform;
            chunkInstances.Add(chunkInstance);
        }
    }

    /**
     * This method takes a float which is the new "start point" for the chunks to be repositioned and then loops through the chunksToReshuffle
     * and increase their height to their new position ahead of the player. It also calls the reshuffleChunk method in the instance's ChunkGenerater component
     */
    private void reshuffleChunks(KeyValuePair<GameObject, int>[] chunksToReshuffle, float furthestChunkPos)
    {
        int i = 1;
        //reshuffle chunks
        foreach (var chunk in chunksToReshuffle)
        {
            float reshuffleYpos = furthestChunkPos + (i * chunkHeight);
            chunk.Key.transform.position = new Vector3(0f, reshuffleYpos);
            chunk.Key.GetComponent<ChunkGenerator>().reshuffleChunk();
            i++;
        }
    }

    //Loops through all chunk instances and calculates the number of chunks, each chunk, is from the player
    private KeyValuePair<GameObject, int>[] calcDistancesFromPlayer(int playerChunkYCoord)
    {
        var distances = new List<KeyValuePair<GameObject, int>>();
        foreach(var chunk in chunkInstances)
        {
            Vector2 chunkPos = chunk.transform.position;
            int chunkYCoord = Mathf.FloorToInt(chunkPos.y) / chunkHeight;
            int distance = chunkYCoord - playerChunkYCoord;
            distances.Add(new KeyValuePair<GameObject, int>(chunk, distance));
        }
        return distances.ToArray();
    }
    
    /** 
     * Unused method that returns the KeyValuePair with the lowest int value from an array of KeyValuePairs
     */
    private KeyValuePair<GameObject, int> minVal(KeyValuePair<GameObject, int>[] list)
    {
        KeyValuePair<GameObject, int> minVal = list[0];
        foreach(var pair in list)
        {
            if(pair.Value < minVal.Value)
            {
                minVal = pair;
            }
        }
        return minVal;
    }

    /**
     * This method returns the KeyValuePair with the highest int value from an array of KeyValuePairs
     * It's used in this script to get the chunk which is the furthest ahead of the player
     */
    private KeyValuePair<GameObject, int> maxVal(KeyValuePair<GameObject, int>[] list)
    {
        KeyValuePair<GameObject, int> maxVal = list[0];
        foreach(var pair in list)
        {
            if(pair.Value >= maxVal.Value)
            {
                maxVal = pair;
            } 
        }
        return maxVal;
    }

    /**
     * Returns a list of KeyValuePairs from a given array where the int value is below a given value
     * Used in the script to find the chunks that have a y axis below the reshuffleCutoff value.
     */
    private KeyValuePair<GameObject, int>[] valsUnderInt(KeyValuePair<GameObject, int>[] list, int val)
    {
        var pairs = new List<KeyValuePair<GameObject, int>>();
        foreach(var pair in list)
        {
            if(pair.Value <= val)
            {
                pairs.Add(pair);
            }
        }
        return pairs.ToArray();
    } 


}
