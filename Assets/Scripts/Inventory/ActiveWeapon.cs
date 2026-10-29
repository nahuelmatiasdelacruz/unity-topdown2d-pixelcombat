using System.Collections;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }
    private float timeBetweenAttacks;
    private PlayerControls playerControls;
    private bool attackButtonDown = false;
    private bool isAttacking = false;
    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void Start()
    {
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
        AttackCooldown();
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void Update()
    {
        Attack();
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;
        AttackCooldown();
        timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;
    }

    public void WeaponNull()
    {
        CurrentActiveWeapon = null;
    }

    private void Attack()
    {
        if (attackButtonDown && !isAttacking && CurrentActiveWeapon)
        {
            AttackCooldown();
            (CurrentActiveWeapon as IWeapon).Attack();
        }
    }

    private void AttackCooldown()
    {
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttacks());
    }
    private IEnumerator TimeBetweenAttacks()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }
}
