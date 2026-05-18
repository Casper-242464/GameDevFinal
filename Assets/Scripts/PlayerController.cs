using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Settings")]
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public float health;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private int doubleJumpsValue = 1;
    [SerializeField] private float knockback = 5f;
    [SerializeField] private float coyoteTime = 0.2f;
    private KeyCode jumpKey;
    private KeyCode leftKey;
    private KeyCode rightKey;


    [Header("Debug")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 1.2f;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private int doubleJumps;
    [SerializeField] private float coyoteTimeCounter;
    [SerializeField] public bool winState = false;

    private void Start()
    {
        health = maxHealth;
        doubleJumps = doubleJumpsValue;
        jumpKey = (KeyCode)PlayerPrefs.GetInt("JumpKey", (int)KeyCode.Space);
        leftKey = (KeyCode)PlayerPrefs.GetInt("LeftKey", (int)KeyCode.A);
        rightKey = (KeyCode)PlayerPrefs.GetInt("RightKey", (int)KeyCode.D);

    }

    private void Update()
    {
        float moveInput = 0f;
        if (Input.GetKey(leftKey)) moveInput -= 1f;
        if (Input.GetKey(rightKey)) moveInput += 1f;
        rb.linearVelocityX = moveInput * moveSpeed;


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

        isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            health -= 20f;
            rb.AddForceY(knockback , ForceMode2D.Impulse);
            StartCoroutine(BlinkRed());
        }
        if (collision.gameObject.CompareTag("Exit"))
        {
            winState = true;
        }
        if (collision.gameObject.CompareTag("Death"))
        {
            health -= maxHealth;
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
}
