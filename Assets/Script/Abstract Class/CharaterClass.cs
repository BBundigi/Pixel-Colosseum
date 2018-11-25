using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharaterClass : MonoBehaviour {
    protected int healthPoint, attackPoint, localXPos, localYPos;
    protected eBuffType Buffs; //Include deBuff^^
    protected List<int> Counts;
    protected List<eBuffType> BuffList;//Use this for RemoveBuff Or RenewBuff with CountList

    public abstract  int HealthPoint
    {
        get;set;
    }
    [SerializeField]
    protected Animator Anim;

    protected virtual void Awake()
    {
        int m_StartXPos;
        int m_StartYPos;

        MapManager.Instance.ConvertPositionToIndexs(transform.position, out m_StartXPos, out m_StartYPos);

        localXPos = m_StartXPos;
        localYPos = m_StartYPos;

        Counts = new List<int>();
        BuffList = new List<eBuffType>();
    }

    protected void SetBuffs()
    {
        eTileState TileStateUnderCharacter = MapManager.Instance.GetTileState(localXPos, localYPos);
        if ((TileStateUnderCharacter & eTileState.OnFire) == eTileState.OnFire)
        {
            if (!BuffList.Contains(eBuffType.OnFire))
            {
                Buffs += (int)eBuffType.OnFire;
                BuffList.Add(eBuffType.OnFire);
                Counts.Add(4);
            }
            else
            {
                Counts[BuffList.IndexOf(eBuffType.OnFire)] = 4;//renew Counts
            }
        }

        if ((TileStateUnderCharacter & eTileState.OnPoison) == eTileState.OnPoison)
        {
            if (!BuffList.Contains(eBuffType.OnPoison))
            {
                Buffs += (int)eBuffType.OnPoison;
                BuffList.Add(eBuffType.OnPoison);
                Counts.Add(2);
            }
            else
            {
                Counts[BuffList.IndexOf(eBuffType.OnPoison)] = 1;//renew Counts
            }
        }
    }

    protected void PlayBuffs()
    {
        if ((Buffs & eBuffType.OnFire) == eBuffType.OnFire)
        {
            HealthPoint -= 4;
        }

        if ((Buffs & eBuffType.OnPoison) == eBuffType.OnPoison)
        {
            //PlayEffect
            HealthPoint -= 3;
        }
    }

    protected void CountTurn()
    {
        for (int i = 0; i < Counts.Count; i++)
        {
            Counts[i]--;
            if (Counts[i] == 0)
            {
                Buffs -= (int)BuffList[i];
                Counts.RemoveAt(i);
                BuffList.RemoveAt(i);
            }
        }
    }

    protected virtual IEnumerator MovePosition(int TargetXPos, int TargetYPos)
    {
        Anim.SetBool(AnimatorHashKeys.AnimIsMoveHash, true);

        Vector3 TargetPosition = MapManager.Instance.ConvertIndexsToPosition(TargetXPos, TargetYPos);
        Vector3 MovePointPerSecond = (TargetPosition - transform.position) / 16;

        while (transform.position != TargetPosition)
        {
            transform.position += MovePointPerSecond;
            yield return null;
        }

        Anim.SetBool(AnimatorHashKeys.AnimIsMoveHash, false);
    }

    protected virtual void SetPositionData(int NewEnemyXPos, int NewlocalYPos)
    {
        localXPos = NewEnemyXPos;
        localYPos = NewlocalYPos;
    }

    protected virtual void ChangeDirection(int LocalTargetXPos)
    {
        transform.rotation = Quaternion.Euler(0,
            LocalTargetXPos - localXPos < 0 ?
                                                180 : 0,
                                              0);
    }
}
