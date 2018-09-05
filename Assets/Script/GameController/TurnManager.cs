using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    public static TurnManager Instance;
    private List<EnemyClass> Enemys;
    private int AmountOfEnemy, EnemyTurnEndNumber;

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
        TouchManager.Instance.enabled = true;
        PlayerManager.Instance.SetMovableTile();

        for (int i = 0; i < Enemys.Count; i++)
        {
            Enemys[i].GetComponent<EnemyStateMachine>().enabled = false;
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
        }
        AmountOfEnemy--;
    }

    public void PlayerTurnEnd()
    {
        if(AmountOfEnemy == 0)
        {
            PlayerTurnStart();
        }

        for (int i = 0; i < Enemys.Count; i++)
        {
            Enemys[i].GetComponent<EnemyStateMachine>().RunState();
        }
    }

    public void EnemyTurnEnd()
    {
        EnemyTurnEndNumber++;
        if (AmountOfEnemy == EnemyTurnEndNumber)
        {
            EnemyTurnEndNumber = 0;
            PlayerTurnStart();
        }
    }

}
