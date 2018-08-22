using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyClass : MonoBehaviour {

    private int hp;
    private int attackPoint;
    private Vector2 Position;
    private float attackRange;

    protected Vector2 [][] defaultDestination;
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
    public void SetPosition()
    {
        MapManager.ConvertPositionToIndexs(transform.position, Position);
    }

    public Vector2 FindMovePosition()
    {
        Vector2 Destination = new Vector2(-1.0f, -1.0f);
        FindPlayer(Destination);

        if (Destination.x == -1 && Destination.y == -1)
        {
            Destination = defaultDestination[DDPivot][0];
        }

        Destination -= Position;

        if (Destination.x == 0)
        {
            if (Destination.y > 0)
            {
                return MapManager.ConvertIndexsToPosition((int)Position.x, (int)Position.y + 1);
            }
            else
            {
                return MapManager.ConvertIndexsToPosition((int)Position.x, (int)Position.y - 1);
            }
        }
        else if (Destination.y == 0)
        {
            if (Destination.x > 0)
            {
                return MapManager.ConvertIndexsToPosition((int)Position.x + 1, (int)Position.y);
            }
            else
            {
                return MapManager.ConvertIndexsToPosition((int)Position.x - 1, (int)Position.y);
            }
        }
        else if (Destination.x > 0 && Destination.y > 0)
        {

            Vector2 ReturnPosition = Position + new Vector2(1, 1);
            if (MapManager.GetTileState(ReturnPosition) == eTileState.Wall)
            {
                Vector2 xTempVector2 = new Vector2(ReturnPosition.x, ReturnPosition.y - 1);
                Vector2 yTempVector2 = new Vector2(ReturnPosition.x - 1, ReturnPosition.y);
                if (MapManager.GetTileState((int)xTempVector2.x, (int)xTempVector2.y) != eTileState.Wall)
                {
                    return MapManager.ConvertIndexsToPosition((int)xTempVector2.x, (int)xTempVector2.y);
                }
                else
                {
                    return MapManager.ConvertIndexsToPosition((int)xTempVector2.x, (int)yTempVector2.y);
                }
            }
            else
            {
                return MapManager.ConvertIndexsToPosition((int)ReturnPosition.x, (int)ReturnPosition.y);
            }
        }


        else if (Destination.x > 0 && Destination.y < 0)
        {
            Vector2 ReturnPosition = Position + new Vector2(1, -1);
            if (MapManager.GetTileState(ReturnPosition) == eTileState.Wall)
            {
                Vector2 xTempVector2 = new Vector2(ReturnPosition.x, ReturnPosition.y + 1);
                Vector2 yTempVector2 = new Vector2(ReturnPosition.x -1, ReturnPosition.y);
                if (MapManager.GetTileState((int)xTempVector2.x, (int)xTempVector2.y) != eTileState.Wall)
                {
                    return MapManager.ConvertIndexsToPosition((int)xTempVector2.x, (int)xTempVector2.y);
                }
                else
                {
                    return MapManager.ConvertIndexsToPosition((int)yTempVector2.x, (int)yTempVector2.y);
                }
            }
            else
            {
                return MapManager.ConvertIndexsToPosition((int)ReturnPosition.x, (int)ReturnPosition.y);
            }
        }

        else if (Destination.x < 0 && Destination.y > 0)
        {
            Vector2 ReturnPosition = Position + new Vector2(-1, 1);
            if (MapManager.GetTileState(ReturnPosition) == eTileState.Wall)
            {
                Vector2 xTempVector2 = new Vector2(ReturnPosition.x, ReturnPosition.y - 1);
                Vector2 yTempVector2 = new Vector2(ReturnPosition.x + 1, ReturnPosition.y);
                if (MapManager.GetTileState((int)xTempVector2.x, (int)xTempVector2.y) != eTileState.Wall)
                {
                    return MapManager.ConvertIndexsToPosition((int)xTempVector2.x, (int)xTempVector2.y);
                }
                else
                {
                    return MapManager.ConvertIndexsToPosition((int)yTempVector2.x, (int)yTempVector2.y);
                }
            }
            else
            {
                return MapManager.ConvertIndexsToPosition((int)ReturnPosition.x, (int)ReturnPosition.y);
            }
        }
        else if (Destination.x < 0 && Destination.y < 0)
        {
            Vector2 ReturnPosition = Position + new Vector2(-1, -1);
            if (MapManager.GetTileState(ReturnPosition) == eTileState.Wall)
            {
                Vector2 xTempVector2 = new Vector2(ReturnPosition.x, ReturnPosition.y + 1);
                Vector2 yTempVector2 = new Vector2(ReturnPosition.x + 1, ReturnPosition.y);
                if (MapManager.GetTileState((int)xTempVector2.x, (int)xTempVector2.y) != eTileState.Wall)
                {
                    return MapManager.ConvertIndexsToPosition((int)xTempVector2.x, (int)xTempVector2.y);
                }
                else
                {
                    return MapManager.ConvertIndexsToPosition((int)yTempVector2.x, (int)yTempVector2.y);
                }
            }
            else
            {
                return MapManager.ConvertIndexsToPosition((int)ReturnPosition.x, (int)ReturnPosition.y);
            }
        }

        return transform.position;
    }

    public void ChangeDirection(Vector2 TargetDirection)
    {
        transform.rotation = Quaternion.Euler(0,
            TargetDirection.x - Position.x > 0 ? 0 : 180,
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

    private void FindPlayer(Vector3 Destination)
    {
        bool[] EnemySightFlag = MapManager.SetShadowFlag(transform.position, 8);
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
                    Destination = new Vector2(TempX, TempY);
                }
                return;
            }
        }
        SetTraformPivot();
        return;
    }
    public void SetTraformPivot()
    {
        if(DDPivot != -1)
        {
            return;
        }
        float ShortestMagnitude = Mathf.Infinity;
        
        for (int i =0; i < defaultDestination.Length; i++)
        {
            float NowMagniutde = (defaultDestination[i][0] - Position).magnitude;
            if (ShortestMagnitude > NowMagniutde)
            {
                DDPivot = i;
                ShortestMagnitude = NowMagniutde;
            }
            
        }
    }

    public void UpdateTransformPivot()
    {
        if ((Position - defaultDestination[DDPivot][0]).magnitude == 0)
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
}
