using UnityEngine;

public class EmptyState : IBaseState
{
    public CharacterStateMachine CharSM { get; set; }

    public void Enter(CharacterStateMachine stateMachine)
    {
        CharSM = stateMachine;

        //CharSM. = true;
        //CharSM.animator.SetBool("", true);

    }

    public void LogicUpdate()
    {

    }

    public void FixedUpdate()
    {

    }

    public void Exit()
    {
        //CharSM. = false;
        //CharSM.animator.SetBool("", false);

    }


}
