using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer),typeof(Animator),typeof(Rigidbody2D))]
public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform weaponCollider;
    public bool FacingLeft
    {
        get
        {
            return facingLeft;
        }
    }
    [SerializeField] private float moveSpeed = 1f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Knockback knockback;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool facingLeft = false;
    private bool isDashing = false;
    private float startMoveSpeed;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
    }

    void Start()
    {
        startMoveSpeed = moveSpeed;
        playerControls.Combat.Dash.performed += _ => Dash();
        ActiveInventory.Instance.EquipStartingWeapon();
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {
        PlayerInput();
    }

    void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }

    void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        animator.SetFloat("moveX",movement.x);
        animator.SetFloat("moveY",movement.y);
    }

    void Move()
    {
        if(knockback.GettingKnockedBack || PlayerHealth.Instance.IsDead) return;
        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * movement);
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        if(mousePosition.x < playerScreenPoint.x)
        {
            spriteRenderer.flipX = true;
            facingLeft = true;
        }
        else
        {
            spriteRenderer.flipX = false;
            facingLeft = false;
        }
    }

    private void Dash()
    {
        if(isDashing || Stamina.Instance.CurrentStamina == 0) return;
        Stamina.Instance.UseStamina();
        isDashing = true;
        trailRenderer.emitting = true;
        moveSpeed *= dashSpeed;
        StartCoroutine(EndDashRoutine());
    }

    private IEnumerator EndDashRoutine()
    {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startMoveSpeed;
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }

}
