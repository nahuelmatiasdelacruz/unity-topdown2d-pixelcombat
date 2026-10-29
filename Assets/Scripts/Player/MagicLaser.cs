using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicLaser : MonoBehaviour
{
    [SerializeField] private float laserGrowTime = 1f;

    private bool isGrowing = true;
    private CapsuleCollider2D capsuleCollider;
    private float laserRange;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        LaserFaceMouse();
    }

    public void UpdateLaerRange(float laserRange)
    {
        this.laserRange = laserRange;
        StartCoroutine(IncreaseLaserLengthRoutine());
    }

    private void LaserFaceMouse()
    {
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mousePositionInCamera = new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, -Camera.main.transform.position.z);
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePositionInCamera);
        Vector2 direction = mouseWorldPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Indestructible>() && !collision.isTrigger)
        {
            isGrowing = false;
        }
    }

    private IEnumerator IncreaseLaserLengthRoutine()
    {
        float timePassed = 0f;
        while (spriteRenderer.size.x < laserRange && isGrowing)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / laserGrowTime;


            spriteRenderer.size = new Vector2(Mathf.Lerp(1f, laserRange,linearT), 1f);

            capsuleCollider.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), capsuleCollider.size.y);
            capsuleCollider.offset = new Vector2(Mathf.Lerp(1f, laserRange, linearT) / 2, capsuleCollider.offset.y);

            yield return null;
        }
        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
    }
}
