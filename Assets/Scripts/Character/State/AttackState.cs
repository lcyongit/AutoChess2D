using UnityEditor;
using UnityEngine;

public class AttackState : IBaseState
{
    public CharacterStateMachine CharSM { get; set; }

    private bool isAttacked;
    private float attackIntervalTimer;

    public void Enter(CharacterStateMachine stateMachine)
    {
        isAttacked = false;
        attackIntervalTimer = 0f;

        CharSM = stateMachine;

        CharSM.isAttack = true;
        CharSM.animator.SetBool("isAttack", true);

        CharSM.FlipCharacter();

    }

    public void LogicUpdate()
    {
        if (isAttacked) 
            attackIntervalTimer += Time.deltaTime;

        // �ʵe���槹�� => ���ݧ������j�ɶ� => ������Idle State
        AnimatorStateInfo stateInfo = CharSM.animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsTag("Attack") && stateInfo.normalizedTime >= 1.0f)
        {
            isAttacked = true;

            if (attackIntervalTimer >= CharSM.charStats.currentAttackInterval)
            {
                CharSM.ChangeSate(CharSM.idleState);
                return;

            }

        }


    }

    public void FixedUpdate()
    {
        CharSM.agent.isStopped = true;

    }

    public void Exit()
    {
        CharSM.isAttack = false;
        CharSM.animator.SetBool("isAttack", false);

    }


}

