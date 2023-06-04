using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject grassPrefab;
    public GameObject roadPrefab;
    public GameObject waterPrefab;

    public int lvlLength = 100;

    private enum BlockType {
        Grass, 
        Road, 
        Water
    }

    private enum Collectables {
        Coins,
        PawPrints
    }

    private List<GameObject> terrainBlocks;

    void Start() {
        terrainBlocks = new List<GameObject>();
        GenerateLevel();
        GameObject startGrass= null;
        for (int i = 0; i < 8; i++) {
        startGrass = Instantiate(grassPrefab, new Vector3(i, 0, 0), Quaternion.identity);
        }
        terrainBlocks.Add(startGrass);
    }

    void GenerateLevel() {

        int position = 4;

        while (position < lvlLength) {

            BlockType blockType = (BlockType)Random.Range(0, 3);
            int blockLength = 0;
            GameObject blockPrefab = null;

            switch (blockType) {

                case BlockType.Grass:
                    blockLength = Random.Range(3, 6);
                    blockPrefab = grassPrefab;
                    break;

                case BlockType.Road:
                    blockLength = Random.Range(6, 12);
                    blockPrefab = roadPrefab;
                    break;

                case BlockType.Water:
                    blockLength = Random.Range(5, 10);
                    blockPrefab = waterPrefab;
                    break;
                }

            for (int i = 0; i < blockLength; i++) {
                if (position >= lvlLength) break;
                GameObject newBlock = Instantiate(blockPrefab, new Vector3(position, 0, 0), Quaternion.identity);
                terrainBlocks.Add(newBlock);

                position++;
            }



            if (blockType == BlockType.Water) {

                if (position > 4) {
                    Debug.Log(terrainBlocks[(position - 4) -1]);
                    GameObject previousBlock = terrainBlocks[(position - 4) - 1];
                    Debug.Log(position - 1 + "(Clone)");
                    Debug.Log(previousBlock);

                    if (previousBlock.CompareTag("Water")) {
                        GameObject grassBlock = Instantiate(grassPrefab, new Vector3(position - 1, 0, 0), Quaternion.identity);
                        terrainBlocks[(position -4) - 1] = grassBlock;
                        //Destroy(previousBlock);
                    }
                }

                if (position < lvlLength) {
                    
                    GameObject grassBlock = Instantiate(grassPrefab, new Vector3(position, 0, 0), Quaternion.identity);
                    terrainBlocks.Add(grassBlock);
                    position++;
                }
            }

        }
    }
}
