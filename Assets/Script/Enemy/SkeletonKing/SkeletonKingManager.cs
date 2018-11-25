using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKingManager : EnemyClass {
    protected override void Start()
    {
        base.Start();
    }

    protected override eEnemyState ChooseNextState()
    {
        SetBuffs();
        PlayBuffs();
        if (MapManager.Instance.CheckTileState(1, eTileState.Player, localXPos, localYPos))
        {
            return eEnemyState.Attack;
        }
        else
        {
            return eEnemyState.Move;
        }
    }

    protected override void  EnemyMove()
    {
        base.EnemyMove();
    }

    protected override void EnemyAttack()
    {
        base.EnemyAttack();
    }
}
