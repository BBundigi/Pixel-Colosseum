using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyClass : MonoBehaviour {

    private int hp;
    private int attackPoint;
    private float attackRange;

    protected Vector3 [][] defaultDestination;
    protected int DDPivot;//Default Destination Pivot

    //public Vector3 [][] DefaultDestination
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
    public Vector2 FindMovePosition( bool ToPlayer)
    {
        Vector2 Destination;
        
        if (ToPlayer)
        {
            Destination = PlayerManager.Instance.transform.position  - transform.position;
        }
        else
        {
            Destination = defaultDestination[DDPivot][0] - transform.position;
        }
        //Debug.Log((int)Mathf.Round(Destination.x * 100));
        //Debug.Log(Destination.x);
        //Debug.Log((int)Mathf.Round(Destination.y * 100));
        //Debug.Log(Destination.y);

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
        bool[] EnemySightFlag = MapManager.SetShadowFlag(transform.position, 8);
        for (int i = 0; i < EnemySightFlag.Length; i++)
        {

            if (EnemySightFlag[i])
            {
                if (MapManager.GetTileState(i) == eTileState.Player)
                {
                    DDPivot = -1;
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
        if(DDPivot != -1)
        {
            return;
        }
        float ShortestMagnitude = Mathf.Infinity;
        
        for (int i =0; i < defaultDestination.Length; i++)
        {
            float NowMagniutde = (defaultDestination[i][0] - transform.position).magnitude;
            if (ShortestMagnitude > NowMagniutde)
            {
                DDPivot = i;
                ShortestMagnitude = NowMagniutde;
            }
            
        }
        Debug.Log(DDPivot);
    }

    public void UpdateTransformPivot()
    {
        Debug.Log((transform.position - defaultDestination[DDPivot][0]).magnitude);
        if (Mathf.Round((transform.position - defaultDestination[DDPivot][0]).magnitude * 100f) == 0)
        {
            int RandomTransform = Random.Range(1, defaultDestination[DDPivot].Length);
            Debug.Log("RandomTransform: " + RandomTransform);
            for (int i = 0; i < defaultDestination.Length; i++)
            {
                Debug.Log(defaultDestination[DDPivot][RandomTransform] == defaultDestination[i][0]);
                if (defaultDestination[DDPivot][RandomTransform] == defaultDestination[i][0])
                {
                    DDPivot = i;
                    break;
                }
            }
        }
    }
}
