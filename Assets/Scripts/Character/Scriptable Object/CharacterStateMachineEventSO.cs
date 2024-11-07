using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CharacterStateMachineEventSO", menuName = "Scriptable Objects/Event/CharacterStateMachineEventSO")]
public class CharacterStateMachineEventSO : ScriptableObject
{
    public UnityAction<CharacterStateMachine> OnEventRaised;


    public void RaiseEvent(CharacterStateMachine characterStateMachine)
    {
        OnEventRaised?.Invoke(characterStateMachine);
    }

}
