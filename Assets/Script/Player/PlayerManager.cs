using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    // Use this for initialization
    private int healthPoint, attackPoint;
    private float attackRange;

    public int HealthPoint
    {
        get
        {
            return healthPoint;
        }
        set
        {
            healthPoint = value;
            if (healthPoint <= 0)
            {
                Anim.SetBool(AnimatorHashKeys.Instance.AnimIsDeadHash, true);
                TurnManager.Instance.enabled = false;
            }
        }
    }

    public int AttackPoint
    {
        get { return attackPoint; }

        set
        {
            attackPoint = value;
        }
    }

    public float AttackRange
    {
        get { return attackRange; }

        set
        {
            attackRange = value;
        }
    }

    [SerializeField]
    private Animator Anim;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
    void Start()
    {
        healthPoint = 100;
        attackPoint = 10;
        attackRange = 0.25f;
    }

public IEnumerator MovePosition(Vector3 TargetPosition)
    {
        Anim.SetBool(AnimatorHashKeys.Instance.AnimIsMoveHash, true);
        Vector3 MovePointPerSecond = (TargetPosition - transform.position) / 15;
        ChangeDirection(TargetPosition);

        while (transform.position != TargetPosition)
        {
            transform.position += MovePointPerSecond;
            yield return null;
        }
        transform.position = TargetPosition;
        Anim.SetBool(AnimatorHashKeys.Instance.AnimIsMoveHash, false);
        MapManager.Instance.ShadowCast(transform.position, 12);
        TurnManager.Instance.PlayerTurnEnd();
    }

    public void PlayerAttack(EnemyClass Target)
    {
        ChangeDirection(Target.transform.position);
        Target.Hp -= AttackPoint;
        Anim.SetBool(AnimatorHashKeys.Instance.AnimIsAttackHash, true);
    }

    private void ChangeDirection(Vector3 TargetVector3)
    {
        transform.rotation = Quaternion.Euler(0, 
            TargetVector3.x - transform.position.x < 0 ?
                                                180 : 0,
                                              0);
    }

    public void EndPlayerAttack()
    {
        Anim.SetBool(AnimatorHashKeys.Instance.AnimIsAttackHash, false);
        TurnManager.Instance.PlayerTurnEnd();
    }
}
