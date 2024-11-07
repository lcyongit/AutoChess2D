using UnityEngine;

public interface IBaseState
{
    public CharacterStateMachine CharSM { get; set; }

    public void Enter(CharacterStateMachine stateMachine);

    public void LogicUpdate();

    public void FixedUpdate();

    public void Exit();


}
