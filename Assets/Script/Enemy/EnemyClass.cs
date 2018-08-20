using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyClass : MonoBehaviour {

    private int hp;
    private int attackPoint;
    private float attackRange;

    private Vector3 [][] defaultDestination;
    private int DDPivot;//Default Destination Pivot

    public Vector3 [][] DefaultDestination
    {
        set
        {
            defaultDestination = value;
        }
        get
        {
            return defaultDestination;
        }
    }


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

    public Vector2 FindMovePosition( bool ToPlayer)
    {
        Vector2 Destination;
        if (ToPlayer)
        {
            Destination = PlayerManager.Instance.transform.position  - transform.position;
        }
        else
        {
            Destination = DefaultDestination[DDPivot][0] - transform.position;
        }

        if ((int)Mathf.Round(Destination.x * 100) == 0)
        {
            if (Destination.y > 0)
            {
                return transform.position + new Vector3(0.0f, 0.64f, 0.0f);
            }
            else
            {
                return transform.position + new Vector3(0.0f, -0.64f, 0.0f);
            }
        }
        else if ((int)Mathf.Round(Destination.y * 100) == 0)
        {
            if (Destination.x > 0)
            {
                return transform.position + new Vector3(0.64f,0.0f, 0.0f);
            }
            else
            {
                return transform.position + new Vector3(-0.64f, 0.0f, 0.0f);
            }
        }
        else if (Destination.x > 0 && Destination.y > 0)
        { 
            return transform.position + new Vector3(0.64f, 0.64f, 0.0f);
        }

        else if (Destination.x > 0 && Destination.y < 0)
        {
            return transform.position + new Vector3(0.64f, -0.64f,0.0f);
        }
        else if (Destination.x < 0 && Destination.y > 0) 
        {
            return transform.position + new Vector3(-0.64f, 0.64f, 0.0f);
        }
        else if(Destination.x < 0 && Destination.y < 0)
        {
            return transform.position + new Vector3(-0.64f, -0.64f, 0.0f);
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
                if(MapManager.GetTileState(i) == eTileState.Player)
                {
                    return true;
                }
            }
            else
            {
                continue;
            }
        }
        SetTraformPivot();
        return false;
    }
    public void SetTraformPivot()
    {
        float ShortestMagnitude = 0;
        for(int i =0; i < DefaultDestination.Length; i++)
        {
            float NowMagniutde = (DefaultDestination[i][0] - transform.position).magnitude;
            if ((DefaultDestination[i][0] - transform.position).magnitude < ShortestMagnitude)
            {
                DDPivot = i;
            }
        }
    }

    public void UpdateTransformPivot()
    {
        if (transform.position == DefaultDestination[DDPivot][0])
        {
            int RandomTransform = Random.Range(0, defaultDestination[DDPivot].Length);

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
