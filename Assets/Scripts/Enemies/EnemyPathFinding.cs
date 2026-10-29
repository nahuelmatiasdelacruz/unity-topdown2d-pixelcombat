using UnityEngine;

public class EnemyPathFinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private Knockback knockback;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Move();
    }
    public void MoveTo(Vector2 targetPosition)
    {
        moveDirection = targetPosition;
    }
    void Move()
    {
        if (knockback.GettingKnockedBack) return;
        rb.MovePosition(rb.position + moveDirection * (moveSpeed * Time.fixedDeltaTime));
        if (moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void StopMoving()
    {
        moveDirection = Vector3.zero;
    }

}
