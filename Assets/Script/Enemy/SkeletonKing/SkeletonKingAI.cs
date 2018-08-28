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
        else if (MapManager.CheckTileState(1, eTileState.Player, AttachedEnemy.EnemyXPos, AttachedEnemy.EnemyYPos))  
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
