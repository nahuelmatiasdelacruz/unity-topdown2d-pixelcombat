using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool IsDead {get; private set;}
    private Knockback knockback;
    private bool canTakeDamage = true;
    private Flash flash;
    private int currentHealth;
    private const string HEALTH_SLIDER_TEXT = "Health Slider";
    private const string TOWN_TEXT = "Town";
    private readonly int DEATH_HASH = Animator.StringToHash("Death");
    private Slider healthSlider;

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockbackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    protected override void Awake()
    {
        base.Awake();
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    void Start()
    {
        IsDead = false;
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
        if (enemy)
        {
            TakeDamage(1, collision.transform);
        }
    }

    public void HealPlayer()
    {
        if(currentHealth < maxHealth)
        {
            currentHealth += 1;
            UpdateHealthSlider();
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if(!canTakeDamage) return;
        ScreenShakeManager.Instance.ShakeScreen();
        knockback.GetKnockedBack(hitTransform,knockbackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath()
    {
        if(currentHealth <= 0 && !IsDead)
        {
            IsDead = true;
            Destroy(ActiveWeapon.Instance.gameObject);
            currentHealth = 0;
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }

    private IEnumerator DeathLoadSceneRoutine()
    {
        yield return new WaitForSeconds(2f);
        //Destroy(gameObject);
        Stamina.Instance.ReplenishStaminaOnDeath();
        SceneManager.LoadScene(TOWN_TEXT);
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }
    private void UpdateHealthSlider()
    {
        if(healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
