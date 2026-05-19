using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Image healthBarFill;

    [Header("SFX & VFX")]
    [SerializeField] private AudioSource stepAudioSource;     
    [SerializeField] private ParticleSystem dustParticles;    

    [Header("Sprites")]
    [SerializeField] private Sprite idleSprite;               
    [SerializeField] private List<Sprite> walkSpritesList;    
    [SerializeField] private float animationFps = 10f;        

    [Header("Settings")]
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public float health;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private int doubleJumpsValue = 1;
    [SerializeField] private float knockback = 5f;
    [SerializeField] private float coyoteTime = 0.2f;

    [Header("Debug & Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.25f;
    [SerializeField] private Transform groundCheckPoint;     
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private int doubleJumps;
    [SerializeField] private float coyoteTimeCounter;
    [SerializeField] public bool winState = false;

    private KeyCode leftKey;
    private KeyCode rightKey;
    private KeyCode jumpKey;

    private int currentAnimationFrame;
    private float animationTimer;
    private bool isInputMoving;

    private void Start()
    {
        health = maxHealth;
        doubleJumps = doubleJumpsValue;

        if (groundCheckPoint == null)
        {
            groundCheckPoint = transform;
        }

        leftKey = (KeyCode)PlayerPrefs.GetInt("LeftKey", (int)KeyCode.A);
        rightKey = (KeyCode)PlayerPrefs.GetInt("RightKey", (int)KeyCode.D);
        jumpKey = (KeyCode)PlayerPrefs.GetInt("JumpKey", (int)KeyCode.Space);
    }

    private void Update()
    {
        float moveInput = 0f;
        if (Input.GetKey(rightKey)) moveInput = 1f;
        if (Input.GetKey(leftKey)) moveInput = -1f;

        rb.linearVelocityX = moveInput * moveSpeed;
        isInputMoving = (moveInput != 0f);

        if (moveInput > 0.1f) spriteRenderer.flipX = false;
        else if (moveInput < -0.1f) spriteRenderer.flipX = true;

        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            doubleJumps = doubleJumpsValue;
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(jumpKey))
        {
            if (coyoteTimeCounter > 0f)
            {
                Jump();
                coyoteTimeCounter = 0f;
            }
            else if (doubleJumps > 0)
            {
                Jump();
                doubleJumps--;
            }
        }

        healthBarFill.fillAmount = health / maxHealth;
        bool shouldPlayEffects = (isInputMoving && isGrounded);
        HandleAnimationAndEffects(shouldPlayEffects);
    }

    private void HandleAnimationAndEffects(bool playing)
    {
        if (spriteRenderer == null) return;

        if (playing)
        {
            if (walkSpritesList != null && walkSpritesList.Count > 1)
            {
                animationTimer += Time.deltaTime;
                if (animationTimer >= (1f / animationFps))
                {
                    currentAnimationFrame++;
                    if (currentAnimationFrame >= walkSpritesList.Count) currentAnimationFrame = 0;
                    spriteRenderer.sprite = walkSpritesList[currentAnimationFrame];
                    animationTimer = 0f;
                }
            }

            if (stepAudioSource != null && !stepAudioSource.isPlaying) stepAudioSource.Play();
            if (dustParticles != null && !dustParticles.isPlaying) dustParticles.Play();
        }
        else
        {
            if (idleSprite != null) spriteRenderer.sprite = idleSprite;
            currentAnimationFrame = 0;
            animationTimer = 0f;

            if (stepAudioSource != null && stepAudioSource.isPlaying) stepAudioSource.Stop();
            if (dustParticles != null && dustParticles.isPlaying) dustParticles.Stop();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            health -= 20f;
            rb.AddForceY(knockback, ForceMode2D.Impulse);
            StartCoroutine(BlinkRed());
        }
        if (collision.gameObject.CompareTag("Exit"))
        {
            winState = true;
        }
    }

    private IEnumerator BlinkRed()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}