using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "WinBattleEventSO", menuName = "Scriptable Objects/Event/WinBattleEventSO")]
public class WinBattleEventSO : ScriptableObject
{
    private static WinBattleEventSO _instance;
    public static WinBattleEventSO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<WinBattleEventSO>("Scriptable Object/Event/WinBattleEventSO");
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
