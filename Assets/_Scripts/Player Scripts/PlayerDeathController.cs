using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathController : MonoBehaviour
{
    [SerializeField] private ParticleSystem splatterParticleSystem;
    private SpriteRenderer spriteRenderer;
    private Collider2D collider;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void OnEnable()
    {
        GameStateController.OnGameOver += die;
    }

    private void OnDisable()
    {
        GameStateController.OnGameOver -= die;
    }

    private void die(GameStateController.GameState gameState)
    {
        if(PlayerPrefs.GetInt("show_gore") == 1)
        {
            splatterParticleSystem.Emit(100);
        }
        spriteRenderer.enabled = false;
        collider.enabled = false;

    }
}
