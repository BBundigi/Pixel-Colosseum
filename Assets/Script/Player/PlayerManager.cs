using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public Vector2 Position
    {
        get
        {
            return position;
        }
    }
    private int healthPoint, attackPoint;

    private Vector2 position;
    private float attackRange;

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

    [SerializeField]
    private Animator Anim;

    private void Awake()
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
        healthPoint = 100;
        attackPoint = 1000;
        attackRange = 0.25f;
        SetPosition();
    }

    public IEnumerator MovePosition(Vector2 TargetPosition)
    {
        Anim.SetBool(AnimatorHashKeys.Instance.AnimIsMoveHash, true);
        ChangeDirection(TargetPosition);

        Vector3 MovePointPerSecond = ((Vector3)TargetPosition - transform.position) / 15;
        MapManager.SetTileState(transform.position, eTileState.BasicTile);
        RemoveMovalbeTile();
        while (transform.position != (Vector3)TargetPosition)
        {
            transform.position += MovePointPerSecond;
            yield return null;
        }

        Anim.SetBool(AnimatorHashKeys.Instance.AnimIsMoveHash, false);
        SetPosition();
        TurnManager.Instance.PlayerTurnEnd();
    }

    public void PlayerAttack(int EnemyPositionX, int EnemyPositionY, EnemyClass Target)
    {
        ChangeDirection(Target.transform.position);
        Target.Hp -= AttackPoint;
        Anim.SetBool(AnimatorHashKeys.Instance.AnimIsAttackHash, true);
    }

    public void EndPlayerAttack()
    {
        Anim.SetBool(AnimatorHashKeys.Instance.AnimIsAttackHash, false);
        TurnManager.Instance.PlayerTurnEnd();
    }

    private void SetPosition()
    {
        position = MapManager.ConvertWorldPositionToLocal(transform.position);
        MapManager.SetTileState(transform.position, eTileState.Player);
        ShadowCaster.ShadowCast(transform.position, 8);
        SetMovableTile();
    }

    private void ChangeDirection(Vector2 TargetVector2)
    {
        transform.rotation = Quaternion.Euler(0, 
            TargetVector2.x - transform.position.x < 0 ?
                                                180 : 0,
                                              0);
    }

    private void SetMovableTile()
    {
        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j< 2; j++)
            {
                int targetX = (int)Position.x + i;
                int targetY = (int)Position.y + j;
                if (MapManager.GetTileState(targetX,targetY) == eTileState.BasicTile)
                {
                    MapManager.SetTileState(targetX, targetY, eTileState.Movable);
                }
            }
        }
    }

    private void RemoveMovalbeTile()
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int targetX = (int)Position.x + i;
                int targetY = (int)Position.y + j;
                if (MapManager.GetTileState(targetX, targetY) == eTileState.Movable)
                {
                    MapManager.SetTileState(targetX, targetY, eTileState.BasicTile);
                }
            }
        }
    }
}
