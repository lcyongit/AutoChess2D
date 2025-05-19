using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ClickBattleCharSlotEventSO", menuName = "Scriptable Objects/Event/ClickBattleCharSlotEventSO")]
public class ClickBattleCharSlotEventSO : ScriptableObject
{
    private static ClickBattleCharSlotEventSO _instance;
    public static ClickBattleCharSlotEventSO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<ClickBattleCharSlotEventSO>("Scriptable Object/Event/ClickBattleCharSlotEventSO");
                if (_instance == null)
                {
                    Debug.LogError("�L�k�b Resources ����� ����SO");
                }
            }
            return _instance;
        }
    }

    public UnityAction OnEventRaised;
    public UnityAction<Character> OnEventRaisedWithCharacter;
    
    public void RaiseEvent(Character character)
    {
        OnEventRaised?.Invoke();
        OnEventRaisedWithCharacter?.Invoke(character);
    }
}
