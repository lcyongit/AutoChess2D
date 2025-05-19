using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "AfterSceneLoadedEventSO", menuName = "Scriptable Objects/Event/AfterSceneLoadedEventSO")]
public class AfterSceneLoadedEventSO : ScriptableObject
{
    private static AfterSceneLoadedEventSO _instance;
    public static AfterSceneLoadedEventSO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<AfterSceneLoadedEventSO>("Scriptable Object/Event/AfterSceneLoadedEventSO");
                if (_instance == null)
                {
                    Debug.LogError("�L�k�b Resources ����� ����SO");
                }
            }
            return _instance;
        }
    }

    public UnityAction OnEventRaised;

    public void RegisterListener(UnityAction listener)
    {
        // �T�O�ƥ󤣭���
        OnEventRaised -= listener;
        OnEventRaised += listener;
        
    }
    public void UnregisterListener(UnityAction listener)
    {
        OnEventRaised -= listener;
    }

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}
