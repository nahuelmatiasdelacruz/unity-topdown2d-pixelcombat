using System;
using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private enum PickUpType
    {
        GoldCoin,
        StaminaGlobe,
        HealthGlobe,
    }

    [SerializeField] private PickUpType pickupType;

    [SerializeField] private float pickupDistance = 5f;
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private float moveSpeedAccelerationRate = .2f;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 1f;
    private Vector3 moveDir;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(AnimationCurveSpawnRoutine());
    }

    void Update()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;
        if(Vector3.Distance(transform.position,playerPos) < pickupDistance)
        {
            moveDir = (playerPos - transform.position).normalized;
            moveSpeed += moveSpeedAccelerationRate;
        }
        else
        {
            moveDir = Vector3.zero;
            moveSpeed = 0f;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveSpeed * Time.deltaTime * moveDir;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            DetectPickupType();
            Destroy(gameObject);
        }
    }

    private IEnumerator AnimationCurveSpawnRoutine()
    {
        Vector2 startPoint = transform.position;

        float randomX = transform.position.x + UnityEngine.Random.Range(-2f,2f);
        float randomY = transform.position.y + UnityEngine.Random.Range(-1f,1f);

        Vector2 endPoint = new Vector2(randomX,randomY);

        float timePassed = 0f;

        while (timePassed < popDuration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / popDuration;
            float heightT = animationCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f,heightY,heightT);
            transform.position = Vector2.Lerp(startPoint, endPoint, linearT) + new Vector2(0f,height);
            yield return null;
        }
    }

    private void DetectPickupType()
    {
        switch (pickupType)
        {
            case PickUpType.GoldCoin:
                EconomyManager.Instance.UpdateCurrentGold();
                break;
            case PickUpType.HealthGlobe:
                PlayerHealth.Instance.HealPlayer();
                break;
            case PickUpType.StaminaGlobe:
                Stamina.Instance.RefreshStamina();
                break;
            default:
                break;
        }
    }
}
