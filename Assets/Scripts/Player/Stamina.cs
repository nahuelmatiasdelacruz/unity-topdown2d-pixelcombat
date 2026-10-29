
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : Singleton<Stamina>
{
    [SerializeField] private Sprite fullStaminaImage, emptyStaminaImage;
    [SerializeField] private float timeBetweenStaminaRefresh = 3f;
    public int CurrentStamina { get; private set; }
    private Transform image;
    private int startingStamina = 3;
    private int maxStamina;
    private const string STAMINA_CONTAINER_TEXT = "Stamina Container";

    protected override void Awake()
    {
        base.Awake();
        maxStamina = startingStamina;
        CurrentStamina = startingStamina;
    }

    void Start()
    {
        image = GameObject.Find(STAMINA_CONTAINER_TEXT).transform;
    }

    public void UseStamina()
    {
        CurrentStamina--;
        UpdateStaminaImages();
        StopAllCoroutines();
        StartCoroutine(RefreshStaminaCoroutine());
    }

    public void ReplenishStaminaOnDeath()
    {
        CurrentStamina = startingStamina;
        UpdateStaminaImages();
    }

    public void RefreshStamina()
    {
        if (CurrentStamina < maxStamina && !PlayerHealth.Instance.IsDead)
        {
            CurrentStamina++;
        }
        UpdateStaminaImages();
    }

    private IEnumerator RefreshStaminaCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenStaminaRefresh);
            RefreshStamina();
        }
    }

    private void UpdateStaminaImages()
    {
        for (int i = 0; i < maxStamina; i++)
        {
            Transform child = this.image.GetChild(i);
            Image image = child != null ? child.GetComponent<Image>() : null;
            image.sprite = (i <= (CurrentStamina - 1)) ? fullStaminaImage : emptyStaminaImage;
        }
    }
}
