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
                    Debug.LogError("無法在 Resources 中找到 對應SO");
                }
            }
            return _instance;
        }
    }

    public UnityAction OnEventRaised;

    public void RegisterListener(UnityAction listener)
    {
        // 確保事件不重複
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
