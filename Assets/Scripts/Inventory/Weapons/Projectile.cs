using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleVfxPrefab;
    [SerializeField] private float projectileRange = 10f;
    [SerializeField] private bool isEnemyProjectile = false;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    public void UpdateProjectileRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    }

    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        Indestructible indestructible = collision.gameObject.GetComponent<Indestructible>();
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
        if(!collision.isTrigger && (enemyHealth || indestructible || player))
        {
            if((player && isEnemyProjectile) || (enemyHealth && !isEnemyProjectile))
            {
                player?.TakeDamage(1, transform);
                Destroy(gameObject);
                Instantiate(particleVfxPrefab,collision.transform.position, transform.rotation);
            }else if(!collision.isTrigger && indestructible)
            {
                Instantiate(particleVfxPrefab,collision.transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }

    private void DetectFireDistance()
    {
        if(Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

}
