using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum eEnmeyState
{
    Move,
    Attack,
    Dead
};

abstract public class EnemyStateMachine : MonoBehaviour {
    protected delegate IEnumerator Del();
    protected eEnmeyState currentState;


    public EnemyClass AttachedEnemy;//이 AI머신을 부착하고있는 적 즉 이 스크립트가 조종할 적

    public virtual void RunState()
    {
        currentState = ChooseNextState();
        switch (currentState)
        {
            case eEnmeyState.Move:
                StartCoroutine(MoveState());
                break;
            case eEnmeyState.Attack:
                AttackState();
                break;
            case eEnmeyState.Dead:
                DeadState();
                break;
        }
    }

    protected virtual IEnumerator MoveState()
    {
        AttachedEnemy.Anim.SetBool(AnimatorHashKeys.Instance.AnimIsMoveHash, true);
        if (AttachedEnemy.FindPlayer())
        {

            AttachedEnemy.ChangeDirection();
            Vector3 TargetPosition = AttachedEnemy.FindMovePosition();
            Vector3 MovePointPerSecond = (TargetPosition - transform.position) / 15;

            while (transform.position != TargetPosition)
            {
                transform.position += MovePointPerSecond;
                yield return null;
            }
            transform.position = TargetPosition;
        }
        else
        {

        }
        AttachedEnemy.Anim.SetBool(AnimatorHashKeys.Instance.AnimIsMoveHash, false);
        TurnManager.Instance.EnemyTurnEnd();
    }

    protected virtual void AttackState()
    {
        PlayerManager.Instance.HealthPoint -= AttachedEnemy.AttackPoint;
        AttachedEnemy.Anim.SetBool(AnimatorHashKeys.Instance.AnimIsAttackHash, true);
        AttachedEnemy.ChangeDirection();
    }

    protected virtual void DeadState()
    {
        AttachedEnemy.Anim.SetBool(AnimatorHashKeys.Instance.AnimIsDeadHash, true);
    }

    protected abstract eEnmeyState ChooseNextState();
}
