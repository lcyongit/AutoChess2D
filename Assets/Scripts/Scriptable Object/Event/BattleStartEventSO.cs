using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BattleStartEventSO", menuName = "Scriptable Objects/Event/BattleStartEventSO")]
public class BattleStartEventSO : ScriptableObject
{
    private static BattleStartEventSO _instance;
    public static BattleStartEventSO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<BattleStartEventSO>("Scriptable Object/Event/BattleStartEventSO");
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

