// ===================================
// Author: Andrada Joaquín Guillermo
// Script: PlayerController
// Type: MonoBehaviour
//
// Description:
// This script handles player movement using WASD keys, changes sprites based on movement direction, 
// and listens to game state changes to enable or disable movement.
// It also updates player stats affecting movement speed.
//
// Course: Tabsil Unity 2D Game - Kawaii Survivor - The Coolest Roguelike Ever
//
// Date: 11/05/2025
// ===================================


using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerStatsDependency, IGameStateListener
{
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Directional Sprites")]
    [SerializeField] private Sprite spriteUp;
    [SerializeField] private Sprite spriteDown;
    [SerializeField] private Sprite spriteLeft;
    [SerializeField] private Sprite spriteRight;

    [Header("Stats")]
    [SerializeField] private float baseMoveSpeed;
    private float moveSpeed;
    private float speedX, speedY;

    [Header("Settings")]
    private bool canMove;

    void Start()
    {
        // Auto-assign Rigidbody2D if not manually set
        if (rig == null)
            rig = GetComponent<Rigidbody2D>();

        // Find the child named "Player Renderer" and get its SpriteRenderer
        if (spriteRenderer == null)
        {
            Transform child = transform.Find("Player Renderer");
            if (child != null)
            {
                spriteRenderer = child.GetComponent<SpriteRenderer>();
            }
            else
            {
                Debug.LogError("Child object 'Player Renderer' not found!");
            }
        }
    }

    private void FixedUpdate()
    {
        WASDMove();
    }

    void WASDMove()
    {
        if (!canMove)
        {
            rig.linearVelocity = Vector2.zero;
            return;
        }
        speedX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        speedY = Input.GetAxisRaw("Vertical") * moveSpeed;
        rig.linearVelocity = new Vector2(speedX, speedY);

        // Change sprite based on movement direction
        if (speedY > 0)
        {
            spriteRenderer.sprite = spriteUp;
        }
        else if (speedY < 0)
        {
            spriteRenderer.sprite = spriteDown;
        }
        else if (speedX > 0)
        {
            spriteRenderer.sprite = spriteRight;
        }
        else if (speedX < 0)
        {
            spriteRenderer.sprite = spriteLeft;
        }
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.GAME)
            canMove = true;
        else
            canMove = false;
    }

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        float moveSpeedPercent = playerStatsManager.GetStatValue(Stat.MoveSpeed) /100;
        moveSpeed = baseMoveSpeed * (1 + moveSpeedPercent);
    }
}
