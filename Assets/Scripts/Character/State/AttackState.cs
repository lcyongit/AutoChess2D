using UnityEditor;
using UnityEngine;

public class AttackState : IBaseState
{
    public CharacterStateMachine CharSM { get; set; }

    public void Enter(CharacterStateMachine stateMachine)
    {
        CharSM = stateMachine;

        CharSM.isAttack = true;
        CharSM.animator.SetBool("isAttack", true);

    }

    public void LogicUpdate()
    {
        // 動畫執行完畢後切換成Idle State
        AnimatorStateInfo stateInfo = CharSM.animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsTag("Attack") && stateInfo.normalizedTime >= 1.0f)
        {
            CharSM.ChangeSate(CharSM.idleState);
            return;
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

