using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    public static TurnManager Instance;
    private List<EnemyClass> Enemys;
    private int AmountOfEnemy, EnemyTurnEndNumber;

    private VoidCallBack playerUpdateLogicAndCount;
    public VoidCallBack PlayerUpdateLogicAndCount
    {
        get
        {
            return playerUpdateLogicAndCount;
        }
        set
        {
            playerUpdateLogicAndCount = value;
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Enemys = new List<EnemyClass>();
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void PlayerTurnStart()
    {
        if (PlayerUpdateLogicAndCount != null)
        {
            PlayerUpdateLogicAndCount();
        }
        TouchManager.Instance.NowMode = eTouchMode.GamePlay;
    }

    public void PlayerTurnEnd()
    {
        if (AmountOfEnemy == 0)
        {
            PlayerTurnStart();
        }
        EnemyTurnEndNumber = 0;
        Enemys[0].GetComponent<EnemyClass>().RunState();
    }


    public void EnemyTurnEnd()
    {
        EnemyTurnEndNumber++;

        if(AmountOfEnemy == EnemyTurnEndNumber)
        {
            PlayerTurnStart();
        }
        else
        {
            Enemys[EnemyTurnEndNumber].GetComponent<EnemyClass>().RunState();
        }
    }



    public void SetEnemyList(EnemyClass TargetEnemy)
    {
        Enemys.Add(TargetEnemy);
        AmountOfEnemy++;
    }

    public void RemoveEnemyList(EnemyClass TargetEnemy)
    {
        if(!Enemys.Remove(TargetEnemy))
        {
            Debug.Log("Error Can't Remove EnemyClass!!");
            return;
        }
        AmountOfEnemy--;
    }
}
