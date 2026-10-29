using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Bow : MonoBehaviour, IWeapon
{
    private static readonly int FireHash = Animator.StringToHash("Fire");
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Attack()
    {
        animator.SetTrigger(FireHash);
        GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        newArrow.GetComponent<Projectile>().UpdateProjectileRange(weaponInfo.weaponRange);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

}
