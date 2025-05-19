using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BeforeSceneUnLoadEventSO", menuName = "Scriptable Objects/Event/BeforeSceneUnLoadEventSO")]
public class BeforeSceneUnLoadEventSO : ScriptableObject
{
    private static BeforeSceneUnLoadEventSO _instance;
    public static BeforeSceneUnLoadEventSO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<BeforeSceneUnLoadEventSO>("Scriptable Object/Event/BeforeSceneUnLoadEventSO");
                if (_instance == null)
                {
                    Debug.LogError("�L�k�b Resources ����� ����SO");
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
