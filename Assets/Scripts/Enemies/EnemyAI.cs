using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyPathFinding))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool dontMoveWhileAttack = false;
    private bool canAttack = true;
    private Vector2 roamPosition;
    private float timeRoaming = 0f;
    private enum State
    {
        Roaming,
        Attacking,
    }

    private State state;
    private EnemyPathFinding enemyPathFinding;


    void Awake()
    {
        enemyPathFinding = GetComponent<EnemyPathFinding>();
        state = State.Roaming;
    }

    void Start()
    {
        roamPosition = GetRoamingPosition();
    }

    void Update()
    {
        MovementStateControl();
    }

    private void MovementStateControl()
    {
        switch (state)
        {

            default:
            case State.Roaming:
                Roaming();
                break;
            case State.Attacking:
                Attacking();
                break;
        }
    }

    private void Roaming()
    {
        timeRoaming += Time.deltaTime;
        enemyPathFinding.MoveTo(roamPosition);
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange)
        {
            state = State.Attacking;
        }
        if (timeRoaming > roamChangeDirFloat)
        {
            roamPosition = GetRoamingPosition();
        }
    }

    private void Attacking()
    {
        if(!enemyType) return;
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange)
        {
            state = State.Roaming;
        }
        if (attackRange != 0 && canAttack)
        {
            canAttack = false;
            (enemyType as IEnemy).Attack();
            if (dontMoveWhileAttack)
            {
                enemyPathFinding.StopMoving();
            }
            else
            {
                enemyPathFinding.MoveTo(roamPosition);
            }
            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private Vector2 GetRoamingPosition()
    {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
