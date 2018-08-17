using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKingManager : EnemyClass {
    void Start()
    {
        Hp = 100;
        AttackPoint = 10;
        AttackRange = 0.25f;
    }
}
