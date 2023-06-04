using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRowController : MonoBehaviour, IReshuffle
{
    private SpriteRenderer spriteRenderer;
    private GameObject bridgeInstance;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Dev Sprites/Water");
        int xPos = Random.Range(0, ChunkPoolController.chunkWidth);
        Vector2 bridgePos = new Vector2(xPos - ChunkPoolController.chunkWidth / 2f + 0.5f, transform.position.y);
        bridgeInstance = Instantiate(Resources.Load<GameObject>("Prefabs/Bridge"), bridgePos, Quaternion.identity);
        bridgeInstance.transform.parent = transform;
    }

    private void positionBridge(int defaultPosition)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position - Vector3.down + new Vector3(0.5f, 0.5f), 0.1f, LayerMask.NameToLayer("Row"));
        if (colliders[0].GetComponent<WaterRowController>() != null)
        {

        }

        
    }

    public void reshuffle()
    {
        //do something
    }
}
