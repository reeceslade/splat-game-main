using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 *  This script will be a component on each chunk instance created by the chunk pool.
 *  It is responsible for creating instances of the rows and determining wether each row will be a grass row or road row
 */
public class ChunkGenerator : MonoBehaviour
{
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private int maxRoads = 10;
    private GameObject[] rowInstances = new GameObject[ChunkPoolController.chunkHeight];

    private void Start()
    {
        instantiateChunk();
    }
    /**
     * This method loops through the set height of a chunk,
     * it instantiates a row gameObject from the rowPrefab, sets its position (which is offset by the index of the loop) and parent and 
     * either adds a RoadRowController or GrassRowController component to the instance randomly as long as the max number of road rows in the chunk hasn't been reached. 
     * If it has it will just add GrassRowController components.
     */
    public void instantiateChunk()
    {
        int roadCount = 0;        
        for(int i = 0; i < ChunkPoolController.chunkHeight; i++)
        {
            Vector2 rowPosition = new Vector2(transform.position.x + 0.5f, transform.position.y + i);
            GameObject rowInstance = Instantiate(rowPrefab, rowPosition, Quaternion.identity);
            rowInstances[i] = rowInstance;
            rowInstance.transform.name = "Row " + i.ToString();
            rowInstance.transform.parent = transform;

            int randInt = Random.Range(0, 2);
            if(randInt == 1 && roadCount < maxRoads)
            {
                rowInstance.AddComponent<RoadRowController>();
                roadCount++;
            }
            else
            {
                rowInstance.AddComponent<GrassRowController>();
            }
        } 
    }
    /**
     * This method is called by the ChunkPoolController when it repositions the chunk instance.
     * It loops through the already instantiated chunk rows and repositions each row randomly to make the chunk
     * 'appear' to be completely different to its previous state allowing for infinite unique chunk generation.
     * It also calls the reshuffle method on that row through its interace which connects to either its GrassRowController or RoadRowController script
     */
    public void reshuffleChunk()
    {
        List<int> rowPositions = new List<int>();

        foreach(var rowInstance in rowInstances)
        {
            int newRowIndex = Random.Range(0, ChunkPoolController.chunkHeight);
            while (rowPositions.Contains(newRowIndex))
            {
                newRowIndex = Random.Range(0, ChunkPoolController.chunkHeight);
            }
            rowPositions.Add(newRowIndex);
            Vector2 newPos = new Vector2(transform.position.x + 0.5f, transform.position.y + newRowIndex);
            rowInstance.transform.position = newPos;


            if (rowInstance.GetComponent<IReshuffle>() != null)
            {
                rowInstance.GetComponent<IReshuffle>().reshuffle();
            }
        }
    }
}
