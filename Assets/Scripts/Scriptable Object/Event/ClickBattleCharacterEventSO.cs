using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ClickBattleCharacterEventSO", menuName = "Scriptable Objects/Event/ClickBattleCharacterEventSO")]
public class ClickBattleCharacterEventSO : ScriptableObject
{
    private static ClickBattleCharacterEventSO _instance;
    public static ClickBattleCharacterEventSO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<ClickBattleCharacterEventSO>("Scriptable Object/Event/ClickBattleCharacterEventSO");
                if (_instance == null)
                {
                    Debug.LogError("無法在 Resources 中找到 對應SO");
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
