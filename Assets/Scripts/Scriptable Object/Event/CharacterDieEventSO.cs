using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CharacterDieEventSO", menuName = "Scriptable Objects/Event/CharacterDieEventSO")]
public class CharacterDieEventSO : ScriptableObject
{
    private static CharacterDieEventSO _instance;
    public static CharacterDieEventSO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<CharacterDieEventSO>("Scriptable Object/Event/CharacterDieEventSO");
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

