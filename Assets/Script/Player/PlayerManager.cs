using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public int PlayerXPos
    {
        get
        {
            return xPos;
        }
    }

    public int PlayerYPos
    {
        get
        {
            return yPos;
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
            if (healthPoint <= 0)
            {
                Anim.SetBool(AnimatorHashKeys.Instance.AnimIsDeadHash, true);
                TurnManager.Instance.enabled = false;
            }
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

    private int xPos, yPos;
    private float attackRange;
    private int healthPoint, attackPoint;

    [SerializeField]
    private Animator Anim;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            int x;
            int y;

            MapManager.ConvertPositionToIndexs(transform.position, out x, out y);

            SetPositionData(x, y);
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
        healthPoint = 100;
        attackPoint = 1000;
        attackRange = 0.25f;

    }

    public IEnumerator MovePosition(int TargetXPos, int TargetYPos)
    {
        Anim.SetBool(AnimatorHashKeys.Instance.AnimIsMoveHash, true);

        Vector3 TargetPosition = MapManager.ConvertIndexsToPosition(TargetXPos, TargetYPos);
        Vector3 MovePointPerSecond = (TargetPosition - transform.position) / 15;
        RemoveMovalbeTile();
        SetPositionData(TargetXPos, TargetYPos);
        

        ChangeDirection(TargetXPos);
        while (transform.position != TargetPosition)
        {
            transform.position += MovePointPerSecond;
            yield return null;
        }

        Anim.SetBool(AnimatorHashKeys.Instance.AnimIsMoveHash, false);
        TurnManager.Instance.PlayerTurnEnd();
    }

    public void PlayerAttack(EnemyClass Target)
    {
        ChangeDirection(Target.EnemyXPos);
        Target.Hp -= AttackPoint;
        Anim.SetBool(AnimatorHashKeys.Instance.AnimIsAttackHash, true);
    }

    public void EndPlayerAttack()
    {
        Anim.SetBool(AnimatorHashKeys.Instance.AnimIsAttackHash, false);
        SetMovableTile();
        TurnManager.Instance.PlayerTurnEnd();
    }

    private void SetPositionData(int NewXPos, int NewYPos)
    {
        xPos = NewXPos;
        yPos = NewYPos;
        MapManager.SetTileState(xPos,yPos, eTileState.Player);
    }

    private void ChangeDirection(float LocalXPos)
    {
        transform.rotation = Quaternion.Euler(0,
            LocalXPos - xPos < 0 ?
                                                180 : 0,
                                              0);
    }

    public void SetMovableTile()
    {
        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j< 2; j++)
            {
                int targetX = xPos + i;
                int targetY = yPos+ j;
                if (targetX > 0 && targetY > 0 && targetX < MapManager.WIDTH && targetY < MapManager.HEIGH)
                {
                    if (MapManager.GetTileState(targetX, targetY) == eTileState.BasicTile)
                    {
                        MapManager.SetTileState(targetX, targetY, eTileState.Movable);
                    }
                }
            }
        }
    }

    private void RemoveMovalbeTile()
    {
        MapManager.SetTileState(xPos, yPos, eTileState.BasicTile);
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int targetX = xPos + i;
                int targetY = yPos + j;

                if (targetX > 0 && targetY > 0 && targetX < MapManager.WIDTH && targetY < MapManager.HEIGH)
                {
                    if (MapManager.GetTileState(targetX, targetY) == eTileState.Movable)
                    {
                        MapManager.SetTileState(targetX, targetY, eTileState.BasicTile);
                    }
                }
            }
        }
    }
}
