using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform2D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float length = 4.0f;
    [SerializeField] private bool horizontal = true; 

    private Rigidbody2D rb;
    private Vector2 startPos;
    private Vector2 targetPos;
    private bool movingToTarget = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        rb.bodyType = RigidbodyType2D.Kinematic; 
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; 
        
        startPos = transform.position;

        Vector2 direction = horizontal ? Vector2.right : Vector2.up;
        targetPos = startPos + (direction * length);
    }

    void FixedUpdate()
    {
        Vector2 currentGoal = movingToTarget ? targetPos : startPos;

        Vector2 newPos = Vector2.MoveTowards(rb.position, currentGoal, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (Vector2.Distance(rb.position, currentGoal) < 0.05f)
        {
            movingToTarget = !movingToTarget;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector2 visualStart = Application.isPlaying ? startPos : (Vector2)transform.position;
        Vector2 direction = horizontal ? Vector2.right : Vector2.up;
        Gizmos.DrawLine(visualStart, visualStart + (direction * length));
    }
}