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

    [Header("Debug")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 1.2f;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private int doubleJumps;
    [SerializeField] private float coyoteTimeCounter;
    [SerializeField] public bool wonState = false;

    private void Start()
    {
        health = maxHealth;
        doubleJumps = doubleJumpsValue;
    }

    private void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
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

        if (Input.GetButtonDown("Jump"))
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
