using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BattleEndEventSO", menuName = "Scriptable Objects/Event/BattleEndEventSO")]
public class BattleEndEventSO : ScriptableObject
{
    private static BattleEndEventSO _instance;
    public static BattleEndEventSO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<BattleEndEventSO>("Scriptable Object/Event/BattleEndEventSO");
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
