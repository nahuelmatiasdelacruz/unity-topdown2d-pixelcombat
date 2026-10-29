using UnityEngine;

public class DamageSource : MonoBehaviour
{
    private int damageAmount;

    void Start()
    {
        MonoBehaviour currentActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        damageAmount = (currentActiveWeapon as IWeapon).GetWeaponInfo().weaponDamage;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<EnemyHealth>()) return;
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        enemyHealth.TakeDamage(damageAmount);
    }
}
