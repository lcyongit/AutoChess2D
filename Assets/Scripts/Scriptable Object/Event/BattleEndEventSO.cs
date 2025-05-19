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
