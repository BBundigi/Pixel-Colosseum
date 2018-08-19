using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyClass : MonoBehaviour {

    private int hp;
    private int attackPoint;
    private float attackRange;

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

        if ((int)Mathf.Round(EnemyToPlayer.x * 100) == 0)
        {
            if (EnemyToPlayer.y > 0)
            {
                return transform.position + new Vector3(0.0f, 0.16f, 0.0f);
            }
            else
            {
                return transform.position + new Vector3(0.0f, -0.16f, 0.0f);
            }
        }
        else if ((int)Mathf.Round(EnemyToPlayer.y * 100) == 0)
        {
            if (EnemyToPlayer.x > 0)
            {
                return transform.position + new Vector3(0.16f,0.0f, 0.0f);
            }
            else
            {
                return transform.position + new Vector3(-0.16f, 0.0f, 0.0f);
            }
        }
        else if (EnemyToPlayer.x > 0 && EnemyToPlayer.y > 0)
        {
            return transform.position + new Vector3(0.16f, 0.16f, 0.0f);
        }

        else if (EnemyToPlayer.x > 0 && EnemyToPlayer.y < 0)
        {
            return transform.position + new Vector3(0.16f, -0.16f,0.0f);
        }
        else if (EnemyToPlayer.x < 0 && EnemyToPlayer.y > 0) 
        {
            return transform.position + new Vector3(-0.16f, 0.16f, 0.0f);
        }
        else if(EnemyToPlayer.x < 0 && EnemyToPlayer.y < 0)
        {
            return transform.position + new Vector3(-0.16f, -0.16f, 0.0f);
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

    public bool FindPlayer()
    {
        bool[] EnemySightFlag = MapManager.SetShadowFlag(transform.position, 12);

        for(int i =0; i < EnemySightFlag.Length; i++)
        {
            if(EnemySightFlag[i])
            {
                int RowIndex;
                int ColumnIndex;

                MapManager.ConvertIndexTo2D(i, out ColumnIndex, out RowIndex);

                if(MapManager.MapData[ColumnIndex,RowIndex] == TileState.Player)
                {
                    return = true;
                }
            }
            else
            {
                continue;
            }
        }
        return false;
    }
}
