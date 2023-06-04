using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderLayerController : MonoBehaviour
{

    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Update()
    {
        int sortingOrder = -Mathf.FloorToInt(transform.position.y);
        if(spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = sortingOrder;
        }
    }
}
