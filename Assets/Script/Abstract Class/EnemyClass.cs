using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyClass : CharaterClass
{
    public int EnemyXPos
    {
        get
        {
            return localXPos;
        }
    }

    public int EnemyYPos
    {
        get
        {
            return localYPos;
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

    public int HealthPoint
    {
        get
        {
            return healthPoint;
        }

        set
        {
            healthPoint = value;
        }
    }

    protected eEnemyState currentState;

    private Vector2[][] defaultDestination;

    private int DDPivot;//Default Destination Pivot

    protected virtual void Start()
    {
        HealthPoint = 100;
        AttackPoint = 10;
        DDPivot = -1;
        defaultDestination = MapManager.EnemyDefaultDestination;

        int rowIndex, columnIndex;
        MapManager.ConvertPositionToIndexs(transform.position, out rowIndex, out columnIndex);
        TurnManager.Instance.SetEnemyList(this);
        localXPos = rowIndex;
        localYPos = columnIndex;
    }

    public void RunState()
    {
       currentState =ChooseNextState();
        switch (currentState)
        {
            case eEnemyState.Attack:
                {
                    EnemyAttack();
                    break;
                }
            case eEnemyState.Move:
                {
                    EnemyMove();
                    break;
                }
        }
    }

    protected virtual void EnemyMove()
    {
        int[] TargetPositions = FindMovePosition();
        SetPositionData(TargetPositions[0], TargetPositions[1]);

        StartCoroutine(MovePosition(TargetPositions[0], TargetPositions[1]));
    }

    protected virtual void EnemyAttack()
    {
        PlayerManager.Instance.HealthPoint -= AttackPoint;
        Anim.SetBool(AnimatorHashKeys.Instance.AnimIsAttackHash, true);
        ChangeDirection(PlayerManager.Instance.PlayerXPos);
    }

    protected override void SetPositionData(int NewEnemyXPos, int NewlocalYPos)
    {
        RemovePositionData();//before move
        
        base.SetPositionData(NewEnemyXPos, NewlocalYPos);

        MapManager.AddTileState(localXPos, localYPos, eTileState.Enemy);//after move
        MapManager.SetMapObjects(localXPos, localYPos,this);
        SetBuffs();
    }
    protected override IEnumerator MovePosition(int TargetXPos, int TargetYPos)
    {
        yield return base.MovePosition(TargetXPos, TargetYPos);
        TurnManager.Instance.EnemyTurnEnd();
    }

    private void AfterAttack_AnimEvent()
    {
        TurnManager.Instance.EnemyTurnEnd();
    }

    protected abstract eEnemyState ChooseNextState();

    private int[] FindMovePosition()
    {
        int[] Destination = FindPlayer();

        int[] returnPositions = new int[2] { localXPos, localYPos };

        if (Destination[0] == -1)
        {
            if (DDPivot == -1)
            {
                SetDDPivot();
            }
            UpdateTransformPivot();
            Destination[0] = (int)defaultDestination[DDPivot][0].x;
            Destination[1] = (int)defaultDestination[DDPivot][0].y;
        }
        Destination[0] -= localXPos;
        Destination[1] -= localYPos;

        if (Destination[0] == 0)
        {
            if (Destination[1] > 0)
            {
                returnPositions[0] = localXPos;
                returnPositions[1] = localYPos + 1;
            }
            else
            {
                returnPositions[0] = localXPos;
                returnPositions[1] = localYPos - 1;
            }
        }
        else if (Destination[1] == 0)
        {
            if (Destination[0] > 0)
            {
                returnPositions[0] = localXPos + 1;
                returnPositions[1] = localYPos;
            }
            else
            {
                returnPositions[0] = localXPos - 1;
                returnPositions[1] = localYPos;
            }
        }
        else if (Destination[0] > 0 && Destination[1] > 0)
        {
            returnPositions[0] = localXPos + 1;
            returnPositions[1] = localYPos + 1;
            if ((MapManager.GetTileState(returnPositions[0], returnPositions[1]) & eTileState.Wall) == eTileState.Wall)
            {
                if (MapManager.GetTileState(localXPos + 1, localYPos) != eTileState.Wall)
                {
                    returnPositions[0] = localXPos + 1;
                    returnPositions[1] = localYPos;
                }
                else
                {
                    returnPositions[0] = localXPos;
                    returnPositions[1] = localYPos + 1;
                }
            }
        }
        else if (Destination[0] > 0 && Destination[1] < 0)
        {
            returnPositions[0] = localXPos + 1;
            returnPositions[1] = localYPos - 1;
            if ((MapManager.GetTileState(returnPositions[0], returnPositions[1]) & eTileState.Wall) == eTileState.Wall)
            {
                if (MapManager.GetTileState(localXPos + 1, localYPos) != eTileState.Wall)
                {
                    returnPositions[0] = localXPos + 1;
                    returnPositions[1] = localYPos;
                }
                else
                {
                    returnPositions[0] = localXPos;
                    returnPositions[1] = localYPos - 1;
                }
            }
        }
        else if (Destination[0] < 0 && Destination[1] > 0)
        {
            returnPositions[0] = localXPos - 1;
            returnPositions[1] = localYPos + 1;
            if ((MapManager.GetTileState(returnPositions[0], returnPositions[1]) & eTileState.Wall) == eTileState.Wall)
            {
                if (MapManager.GetTileState(localXPos - 1, localYPos) != eTileState.Wall)
                {
                    returnPositions[0] = localXPos - 1;
                    returnPositions[1] = localYPos;
                }
                else
                {
                    returnPositions[0] = localXPos;
                    returnPositions[1] = localYPos + 1;
                }
            }
        }
        else if (Destination[0] < 0 && Destination[1] < 0)
        {
            returnPositions[0] = localXPos - 1;
            returnPositions[1] = localYPos - 1;
            if ((MapManager.GetTileState(returnPositions[0], returnPositions[1]) & eTileState.Wall) == eTileState.Wall)
            {
                if (MapManager.GetTileState(localXPos - 1, localYPos) != eTileState.Wall)
                {
                    returnPositions[0] = localXPos - 1;
                    returnPositions[1] = localYPos;
                }
                else
                {
                    returnPositions[0] = localXPos;
                    returnPositions[1] = localYPos - 1;
                }
            }
        }
        RemovePositionData();
        ChangeDirection(returnPositions[0]);
        SetPositionData(returnPositions[0], returnPositions[1]);


        return returnPositions;
    }

    private void RemovePositionData()
    {
        MapManager.RemoveTileState(localXPos, localYPos, eTileState.Enemy);
        MapManager.SetMapObjects(localXPos, localYPos, null);
    }

    private void SetDDPivot()
    {
        float ShortestMagnitude = Mathf.Infinity;

        for (int i = 0; i < defaultDestination.Length; i++)
        {
            float NowMagniutde = (defaultDestination[i][0] - new Vector2(localXPos, localYPos)).magnitude;
            if (ShortestMagnitude > NowMagniutde)
            {
                DDPivot = i;
                ShortestMagnitude = NowMagniutde;
            }
        }
    }

    private void UpdateTransformPivot()
    {
        if ((new Vector2(localXPos, localYPos) - defaultDestination[DDPivot][0]).magnitude == 0)
        {
            int RandomTransform = Random.Range(1, defaultDestination[DDPivot].Length);
            for (int i = 0; i < defaultDestination.Length; i++)
            {
                if (defaultDestination[DDPivot][RandomTransform] == defaultDestination[i][0])
                {
                    DDPivot = i;
                    break;
                }
            }
        }
    }

    private int[] FindPlayer()
    {
        bool[] EnemySightFlag = ShadowCaster.SetShadowFlag(localXPos, localYPos, 8);
        int[] Destination = new int[2];
        for (int i = 0; i < EnemySightFlag.Length; i++)
        {
            if (EnemySightFlag[i])
            {
                if ((MapManager.GetTileState(i) & eTileState.Player) == eTileState.Player)
                {
                    DDPivot = -1;
                    int TempX;
                    int TempY;
                    MapManager.ConvertIndexTo2D(i, out TempX, out TempY);
                    Destination[0] = TempX;
                    Destination[1] = TempY;
                    return Destination;
                }
            }
        }
        Destination[0] = -1;//return Destination[0] = -1 if Can't Find Player

        return Destination;
    }
}
