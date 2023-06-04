using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleCollisionController : MonoBehaviour
{
    [SerializeField] private Sprite splatteredSprite;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player" && PlayerPrefs.GetInt("show_gore") == 1)
        {
            spriteRenderer.sprite = splatteredSprite;
        }
    }
}
