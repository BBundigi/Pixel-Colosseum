using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKingManager : EnemyClass {
    void Start()
    {
        Hp = 100;
        AttackPoint = 10;
        AttackRange = 0.92f;
        DDPivot = -1;
        defaultDestination = MapManager.EnemyDefaultDestination;
        SetPosition();
        PlayerManager.Instance.setEnemyList(this);
    }

    private void OnDisable()
    {
        PlayerManager.Instance.setEnemyList(this);
    }
}
