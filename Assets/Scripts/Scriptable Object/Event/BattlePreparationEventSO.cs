using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BattlePreparationEventSO", menuName = "Scriptable Objects/Event/BattlePreparationEventSO")]
public class BattlePreparationEventSO : ScriptableObject
{
    private static BattlePreparationEventSO _instance;
    public static BattlePreparationEventSO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<BattlePreparationEventSO>("Scriptable Object/Event/BattlePreparationEventSO");
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
