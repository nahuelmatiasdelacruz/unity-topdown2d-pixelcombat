using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private float konckBackThrust = 15f;
    private int currentHealth;

    private Knockback konckback;
    private Flash flash;

    void Awake()
    {
        flash = GetComponent<Flash>();
        konckback = GetComponent<Knockback>();
    }

    void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        konckback.GetKnockedBack(PlayerController.Instance.transform,konckBackThrust);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DetectDeath();
    }

    private void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathVFX,transform.position,Quaternion.identity);
            GetComponent<PickupSpawner>().DropItems();
            Destroy(gameObject);
        }
    }

}
