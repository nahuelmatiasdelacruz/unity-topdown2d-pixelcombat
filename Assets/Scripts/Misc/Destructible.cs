using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destroyVisualEffect;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageSource>() || collision.gameObject.GetComponent<Projectile>())
        {
            PickupSpawner spawner = GetComponent<PickupSpawner>();
            spawner?.DropItems();
            Instantiate(destroyVisualEffect, transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
