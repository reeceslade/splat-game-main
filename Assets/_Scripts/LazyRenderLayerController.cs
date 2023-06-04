using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazyRenderLayerController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        int sortingOrder = -Mathf.FloorToInt(transform.position.y);
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = sortingOrder;
        }
    }
}
