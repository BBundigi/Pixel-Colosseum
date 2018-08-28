using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyClass : MonoBehaviour
{
    private int hp;
    private int attackPoint;

    private int xPos, yPos;

    public int EnemyXPos
    {
        get
        {
            return xPos;
        }
    }

    public int EnemyYPos
    {
        get
        {
            return yPos;
        }
    }


    private float attackRange;

    protected Vector2[][] defaultDestination;
    protected int DDPivot;//Default Destination Pivot

    //public Vector2 [][] DefaultDestination
    //{
    //    set
    //    {
    //        defaultDestination = value;
    //    }
    //    get
    //    {
    //        return defaultDestination;
    //    }
    //}


    public Animator Anim;

    public int Hp
    {
        get { return hp; }
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

    protected virtual void Start()
    {
        Hp = 100;
        AttackPoint = 10;
        AttackRange = 0.92f;
        DDPivot = -1;
        defaultDestination = MapManager.EnemyDefaultDestination;

        int rowIndex, columnIndex;
        MapManager.ConvertPositionToIndexs(transform.position,out rowIndex, out columnIndex);

        xPos = rowIndex;
        yPos = columnIndex;
    }

    protected virtual void OnDisable()
    {
        RemovePositionData();
    }

    public int[] FindMovePosition()
    {
        int[] Destination = FindPlayer();

        int[] returnPositions = new int[2] { xPos, yPos };

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
        Destination[0] -= xPos;
        Destination[1] -= yPos;

        if (Destination[0] == 0)
        {
            if (Destination[1] > 0)
            {
                returnPositions[0] = xPos;
                returnPositions[1] = yPos + 1;
            }
            else
            {
                returnPositions[0] = xPos;
                returnPositions[1] = yPos - 1;
            }
        }
        else if (Destination[1] == 0)
        {
            if (Destination[0] > 0)
            {
                returnPositions[0] = xPos + 1;
                returnPositions[1] = yPos;
            }
            else
            {
                returnPositions[0] = xPos - 1;
                returnPositions[1] = yPos;
            }
        }
        else if (Destination[0] > 0 && Destination[1] > 0)
        {
            returnPositions[0] = xPos + 1;
            returnPositions[1] = yPos + 1;
            if (MapManager.GetTileState(returnPositions[0], returnPositions[1]) == eTileState.Wall)
            {
                if (MapManager.GetTileState(xPos + 1, yPos) != eTileState.Wall)
                {
                    returnPositions[0] = xPos + 1;
                    returnPositions[1] = yPos;
                }
                else
                {
                    returnPositions[0] = xPos;
                    returnPositions[1] = yPos + 1;
                }
            }
        }
        else if (Destination[0] > 0 && Destination[1] < 0)
        {
            returnPositions[0] = xPos + 1;
            returnPositions[1] = yPos - 1;
            if (MapManager.GetTileState(returnPositions[0], returnPositions[1]) == eTileState.Wall)
            {
                if (MapManager.GetTileState(xPos + 1, yPos) != eTileState.Wall)
                {
                    returnPositions[0] = xPos + 1;
                    returnPositions[1] = yPos;
                }
                else
                {
                    returnPositions[0] = xPos;
                    returnPositions[1] = yPos - 1;
                }
            }
        }
        else if (Destination[0] < 0 && Destination[1] > 0)
        {
            returnPositions[0] = xPos - 1;
            returnPositions[1] = yPos + 1;
            if (MapManager.GetTileState(returnPositions[0], returnPositions[1]) == eTileState.Wall)
            {
                if (MapManager.GetTileState(xPos - 1, yPos) != eTileState.Wall)
                {
                    returnPositions[0] = xPos - 1;
                    returnPositions[1] = yPos;
                }
                else
                {
                    returnPositions[0] = xPos;
                    returnPositions[1] = yPos + 1;
                }
            }
        }
        else if (Destination[0] < 0 && Destination[1] < 0)
        {
            returnPositions[0] = xPos - 1;
            returnPositions[1] = yPos - 1;
            if (MapManager.GetTileState(returnPositions[0], returnPositions[1]) == eTileState.Wall)
            {
                if (MapManager.GetTileState(xPos - 1, yPos) != eTileState.Wall)
                {
                    returnPositions[0] = xPos - 1;
                    returnPositions[1] = yPos;
                }
                else
                {
                    returnPositions[0] = xPos;
                    returnPositions[1] = yPos - 1;
                }
            }
        }
        RemovePositionData();
        ChangeDirection(returnPositions[0]);
        SetPositionData(returnPositions[0], returnPositions[1]);
        

        return returnPositions;
    }

    public void EnemyAttack()
    {
        PlayerManager.Instance.HealthPoint -= AttackPoint;
        Anim.SetBool(AnimatorHashKeys.Instance.AnimIsAttackHash, true);
        ChangeDirection(PlayerManager.Instance.PlayerXPos);
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

    private void SetPositionData(int NewXPos, int NewYPos)
    {
        xPos = NewXPos;
        yPos = NewYPos;
        MapManager.SetTileState(xPos, yPos, eTileState.Enemy);
        MapManager.SetMapObjects(xPos, yPos, this);
    }

    private void RemovePositionData()
    {
        MapManager.SetTileState(xPos, yPos, eTileState.BasicTile);
        MapManager.SetMapObjects(xPos, yPos, null);
    }

    private void ChangeDirection(int LocalPosX)
    {
        Debug.Log(LocalPosX);
        Debug.Log(xPos);
        transform.rotation = Quaternion.Euler(0,
            LocalPosX - xPos > 0 ? 0 : 180,
                                               0);
    }

    private void SetDDPivot()
    {
        float ShortestMagnitude = Mathf.Infinity;

        for (int i = 0; i < defaultDestination.Length; i++)
        {
            float NowMagniutde = (defaultDestination[i][0] - new Vector2(xPos,yPos)).magnitude;
            if (ShortestMagnitude > NowMagniutde)
            {
                DDPivot = i;
                ShortestMagnitude = NowMagniutde;
            }

        }
    }

    private void UpdateTransformPivot()
    {
        if ((new Vector2(xPos,yPos) - defaultDestination[DDPivot][0]).magnitude == 0)
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
        bool[] EnemySightFlag = ShadowCaster.SetShadowFlag(xPos,yPos, 8);
        int[] Destination = new int[2];
        for (int i = 0; i < EnemySightFlag.Length; i++)
        {
            if (EnemySightFlag[i])
            {
                if (MapManager.GetTileState(i) == eTileState.Player)
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
