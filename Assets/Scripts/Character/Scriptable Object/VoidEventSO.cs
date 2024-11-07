using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VoidEventSO", menuName = "Scriptable Objects/Event/VoidEventSO")]
public class VoidEventSO : ScriptableObject
{
    public UnityAction OnEventRaised;

    
    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }

}
