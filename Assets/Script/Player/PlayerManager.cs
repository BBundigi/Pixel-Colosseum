using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharaterClass
{
    public static PlayerManager Instance;

    public int PlayerXPos
    {
        get
        {
            return localXPos;
        }
    }

    public int PlayerYPos
    {
        get
        {
            return localYPos;
        }
    }
    
    public override int HealthPoint
    {
        get
        {
            return healthPoint;
        }
        set
        {
            healthPoint = value;
            PlayerStatusUIManager.Instance.SetHP(MaxHealthPoint, value);
            if (healthPoint <= 0)
            {
                Anim.SetBool(AnimatorHashKeys.AnimIsDeadHash, true);
                TurnManager.Instance.enabled = false;
            }
            else if(healthPoint > MaxHealthPoint)
            {
                healthPoint = MaxHealthPoint;
            }
        }
    }


    protected override void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            int x;
            int y;

            MapManager.ConvertPositionToIndexs(transform.position, out x, out y);
            SetPositionData(x, y);
            base.Awake();
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private int MaxHealthPoint;

    void Start()
    {
        MaxHealthPoint = 100;
        healthPoint = 100;
        attackPoint = 1000;
        ShadowCaster.ShadowCast(localXPos, localYPos, 8);
        TurnManager.Instance.PlayerUpdateLogicAndCount += UpdatePlayerLogic;
    }

    private void UpdatePlayerLogic()
    {
        SetBuffs();
        PlayBuffs();
        CountTurn();
        SetMovableTile();
    }

    public void PlayerMove(int TargetXPos, int TargetYPos)
    {
        RemoveMovalbeTile();
        ChangeDirection(TargetXPos);
        SetPositionData(TargetXPos, TargetYPos);
        SetMovableTile();
        ShadowCaster.ShadowCast(localXPos, localYPos, 8);

        StartCoroutine(MovePosition(TargetXPos, TargetYPos));
    }

    public void PlayerAttack(EnemyClass Target)
    {
        ChangeDirection(Target.EnemyXPos);
        Target.HealthPoint -= attackPoint;
        Anim.SetBool(AnimatorHashKeys.AnimIsAttackHash, true);
    }

    protected override IEnumerator MovePosition(int TargetXPos, int TargetYPos)
    {
        yield return base.MovePosition(TargetXPos, TargetYPos);
        TurnManager.Instance.PlayerTurnEnd();
    }

    protected override void SetPositionData(int NewXPos, int NewYPos)
    {
        MapManager.RemoveTileState(localXPos, localYPos, eTileState.Player);//Before move
        base.SetPositionData(NewXPos,NewYPos);
        MapManager.AddTileState(localXPos, localYPos, eTileState.Player);
    }

    private void SetMovableTile()
    {
        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j< 2; j++)
            {
                int targetX = localXPos + i;
                int targetY = localYPos + j;
                eTileState TargetTile = MapManager.GetTileState(targetX, targetY);
                if (TargetTile != eTileState.None)
                {
                    if ((TargetTile & eTileState.BasicTile) == eTileState.BasicTile)
                    {
                        MapManager.AddTileState(targetX, targetY, eTileState.Movable);
                    }
                }
            }
        }
    }

    private void RemoveMovalbeTile()
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int targetX = localXPos + i;
                int targetY = localYPos + j;

                eTileState TargetTile = MapManager.GetTileState(targetX, targetY);
                if (TargetTile != eTileState.None)
                {
                    if ((TargetTile & eTileState.Movable) == eTileState.Movable)
                    {
                        MapManager.RemoveTileState(targetX, targetY, eTileState.Movable);
                    }
                }
            }
        }
    }

    private void AfterAttack_AnimEvent()
    {
        Anim.SetBool(AnimatorHashKeys.AnimIsAttackHash, false);
        TurnManager.Instance.PlayerTurnEnd();
    }
}
