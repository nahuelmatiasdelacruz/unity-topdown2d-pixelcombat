using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject magicLaser;
    [SerializeField] private Transform magicLaserSpawnPoint;
    private readonly int ATTACK_HASH = Animator.StringToHash("Attack");
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }
    public void Attack()
    {
        animator.SetTrigger(ATTACK_HASH);
    }

    public void SpawnStaffProjectileAnimEvent()
    {
        GameObject newLaser = Instantiate(magicLaser, magicLaserSpawnPoint.position, Quaternion.identity);
        newLaser.GetComponent<MagicLaser>().UpdateLaerRange(weaponInfo.weaponRange);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();

        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePosition.y - playerScreenPoint.y, Mathf.Abs(mousePosition.x - playerScreenPoint.x)) * Mathf.Rad2Deg;

        if (mousePosition.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
