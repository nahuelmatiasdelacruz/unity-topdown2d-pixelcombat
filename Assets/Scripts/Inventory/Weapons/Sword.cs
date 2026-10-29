using UnityEngine;
using UnityEngine.InputSystem;

public class Sword : MonoBehaviour, IWeapon
{
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private ScreenShake screenShakeCamera;
    private Animator animator;
    private GameObject slashAnim;
    private bool warnedMissingScreenShake;

    void Awake()
    {
        animator = GetComponent<Animator>();
        screenShakeCamera = FindAnyObjectByType<ScreenShake>();
    }

    void Start()
    {
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimSpawnPoint = GameObject.Find("SlashAnimationSpawnPoint").transform;
    }

    void Update()
    {
        MouseFollowWithOffset();
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    public void DoneAttack()
    {
        weaponCollider.gameObject.SetActive(false);
    }

    public void SwingUpFlipAnimEvent()
    {
        slashAnim.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (PlayerController.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnimEvent()
    {
        slashAnim.transform.rotation = Quaternion.Euler(-180, 0, 0);
        if (PlayerController.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void Attack()
    {
        if (screenShakeCamera == null)
        {
            screenShakeCamera = FindAnyObjectByType<ScreenShake>();
        }

        if (screenShakeCamera != null)
        {
            screenShakeCamera.ShakeScreen();
            warnedMissingScreenShake = false;
        }
        else if (!warnedMissingScreenShake)
        {
            Debug.LogWarning("No active ScreenShake component was found; the attack will continue without camera shake.", this);
            warnedMissingScreenShake = true;
        }

        weaponCollider.gameObject.SetActive(true);
        animator.SetTrigger(AttackHash);
        slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        slashAnim.transform.parent = transform.parent;
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePos.y - playerScreenPoint.y, Mathf.Abs(mousePos.x - playerScreenPoint.x)) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
