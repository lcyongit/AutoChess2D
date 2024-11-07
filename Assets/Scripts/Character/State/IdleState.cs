using UnityEngine;

public class IdleState : IBaseState
{
    public CharacterStateMachine CharSM { get; set; }

    public void Enter(CharacterStateMachine stateMachine)
    {
        CharSM = stateMachine;

        CharSM.isIdle = true;
        CharSM.animator.SetBool("isIdle", true);

    }

    public void LogicUpdate()
    {
        if (CharSM.SearchTarget())
        {
            CharSM.ChangeSate(CharSM.chaseState);
            return;
        }

    }

    public void FixedUpdate()
    {
        CharSM.agent.isStopped = true;

    }

    public void Exit()
    {
        CharSM.isIdle = false;
        CharSM.animator.SetBool("isIdle", false);

    }



}

