using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyClass : MonoBehaviour {
    private int hp;
    private int attackPoint;
    private float attackRange;
    private FindShortestMovePosition PathFinder;

    public Animator Anim;

    public int Hp
    {
        get { return hp;}
        set
        {
            hp = value;
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

    public Vector2 FindMovePosition()
    {
        Vector2 EnemyToPlayer = PlayerManager.Instance.transform.position - transform.position;

        if (EnemyToPlayer.x == 0)
        {
            if (EnemyToPlayer.y > 0)
            {
                return new Vector2(0.0f, 0.16f);
            }
            else
            {
                return new Vector2(0.0f, -0.16f);
            }
        }

        else if (EnemyToPlayer.y == 0)
        {
            if (EnemyToPlayer.x > 0)
            {
                return new Vector2(0.16f, 0.0f);
            }
            else
            {
                return new Vector2(-0.16f, 0.0f);
            }
        }

        else if (EnemyToPlayer.x > 0 && EnemyToPlayer.y > 0)
        {
            return new Vector2(0.16f, 0.16f);
        }

        else if (EnemyToPlayer.x > 0 && EnemyToPlayer.y < 0)
        {
            return new Vector2(0.16f, -0.16f);
        }
        else if (EnemyToPlayer.x < 0 && EnemyToPlayer.y > 0) 
        {
            return new Vector2(-0.16f, 0.16f);
        }
        else if(EnemyToPlayer.x < 0 && EnemyToPlayer.y < 0)
        {
            return new Vector2(-0.16f, -0.16f);
        }
        return transform.position;
    }

    public void ChangeDirection()
    {
        transform.rotation = Quaternion.Euler(0,
            PlayerManager.Instance.transform.position.x - transform.position.x < 0 ? 180 : 0,
                                               0);
    }

    public void StopAttackAnim()
    {
        Anim.SetBool(AnimatorHashKeys.Instance.AnimIsAttackHash, false);
        TurnManager.Instance.EnemyTurnEnd();
    }
    
    public void AfterDeadAnim()
    {
        Destroy(gameObject);
        TurnManager.Instance.EnemyTurnEnd();
    }
}
