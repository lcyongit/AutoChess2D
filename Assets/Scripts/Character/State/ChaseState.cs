using UnityEngine;

public class ChaseState : IBaseState
{
    public CharacterStateMachine CharSM { get; set; }

    public void Enter(CharacterStateMachine stateMachine)
    {
        CharSM = stateMachine;

        CharSM.isChase = true;
        CharSM.animator.SetBool("isChase", true);

    }

    public void LogicUpdate()
    {
        CharSM.FlipCharacter();

        if (CharSM.target == null)
        {
            CharSM.ChangeSate(CharSM.idleState);
            return;
        }

        if (Vector3.Distance(CharSM.transform.position, CharSM.target.position) <= CharSM.agent.stoppingDistance)
        {
            CharSM.ChangeSate(CharSM.attackState);
            return;
        }


    }

    public void FixedUpdate()
    {
        CharSM.agent.isStopped = false;
        CharSM.agent.destination = CharSM.target.position;
        CharSM.agent.stoppingDistance = CharSM.charStats.currentAttackRange;
        CharSM.agent.speed = CharSM.charStats.currentMoveSpeed;
    }

    public void Exit()
    {
        CharSM.isChase = false;
        CharSM.animator.SetBool("isChase", false);

    }


}