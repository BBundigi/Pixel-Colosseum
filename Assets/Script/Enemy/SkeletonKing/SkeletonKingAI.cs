using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKingAI : EnemyStateMachine
{
    protected override eEnmeyState ChooseNextState()
    {
        if (AttachedEnemy.Hp <= 0)
        {
            return eEnmeyState.Dead;
        }
        else if((PlayerManager.Instance.transform.position - AttachedEnemy.transform.position).magnitude < AttachedEnemy.AttackRange)
        {
            return eEnmeyState.Attack;
        }
        else
        {
            return eEnmeyState.Move;
        }
    }

    protected override IEnumerator MoveState()
    { 
        yield return base.MoveState();
    }

    protected override void AttackState()
    {
        base.AttackState();
    }

    protected override void DeadState()
    {
        base.DeadState();
    }
}
