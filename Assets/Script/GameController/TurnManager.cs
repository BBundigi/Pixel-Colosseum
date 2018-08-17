using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    public static TurnManager Instance;
    private int AmountOfEnemy, EnemyTurnEndNumber;

    void Awake()
    {
        if (Instance == null)
        {
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
    
    void Start()
    {
        AmountOfEnemy = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    public void PlayerTurnEnd()
    {
        GameObject[] EnemyGameObject = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < EnemyGameObject.Length; i++)
        {
            EnemyGameObject[i].GetComponent<EnemyStateMachine>().RunState();
        }
        AmountOfEnemy = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if(AmountOfEnemy == 0)
        {
            PlayerTurnStart();
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

    public void PlayerTurnStart()
    {
        TouchManager.Instance.enabled = true;
        GameObject[] EnemyGameObject = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < EnemyGameObject.Length; i++)
        {
            EnemyGameObject[i].GetComponent<EnemyStateMachine>().enabled = false;
        }
    }
}
