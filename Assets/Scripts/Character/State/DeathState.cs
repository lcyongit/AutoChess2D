using UnityEngine;

public class DeathState : IBaseState
{
    public CharacterStateMachine CharSM { get; set; }

    public void Enter(CharacterStateMachine stateMachine)
    {
        CharSM = stateMachine;
        
        CharSM.isDead = true;
        CharSM.animator.SetBool("isDeath", true);

        CharSM.target = null;
        CharSM.agent.enabled = false;
        CharSM.charBS.UnRegisterCharBS();


    }

    public void LogicUpdate()
    {

    }

    public void FixedUpdate()
    {

    }

    public void Exit()
    {
        CharSM.isDead = false;
        CharSM.animator.SetBool("isDeath", false);

    }


}

