using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BeforeSceneLoadedEventSO", menuName = "Scriptable Objects/Event/BeforeSceneLoadedEventSO")]
public class BeforeSceneLoadedEventSO : ScriptableObject
{
    private static BeforeSceneLoadedEventSO _instance;
    public static BeforeSceneLoadedEventSO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<BeforeSceneLoadedEventSO>("Scriptable Object/Event/BeforeSceneLoadedEventSO");
                if (_instance == null)
                {
                    Debug.LogError("無法在 Resources 中找到 對應SO");
                }
            }
            return _instance;
        }
    }

    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }

}
