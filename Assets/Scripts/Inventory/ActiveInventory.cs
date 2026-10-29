using UnityEngine;
using UnityEngine.UI;

public class ActiveInventory : Singleton<ActiveInventory>
{
    private int activeSlotIndexNumber = 0;

    private PlayerControls playerControls;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
    }

    void Start()
    {
        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
        ToggleActiveHighLight(1);
    }

    void OnEnable()
    {
        playerControls.Enable();        
    }

    public void EquipStartingWeapon()
    {
        ToggleActiveHighLight(1);
    }

    private void ToggleActiveSlot(int numValue)
    {
        ToggleActiveHighLight(numValue);
    }

    private void ToggleActiveHighLight(int indexNum)
    {
        activeSlotIndexNumber = indexNum;
        foreach(Transform slot in transform)
        {
            slot.GetChild(0).gameObject.SetActive(false);
        }
        transform.GetChild(indexNum - 1).GetChild(0).gameObject.SetActive(true);
        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        if(PlayerHealth.Instance.IsDead) return;
        if(ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }
        
        Transform selectedSlot = transform.GetChild(activeSlotIndexNumber - 1);
        InventorySlot inventorySlot = selectedSlot.GetComponentInChildren<InventorySlot>();
        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();
        
        if(weaponInfo == null)
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        GameObject weaponToSpawn = inventorySlot.GetWeaponInfo().weaponPrefab;
        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0,0,0);
        newWeapon.transform.parent = ActiveWeapon.Instance.transform;
        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
