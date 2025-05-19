using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ClickGameLevelSlotEventSO", menuName = "Scriptable Objects/Event/ClickGameLevelSlotEventSO")]
public class ClickGameLevelNodeEventSO : ScriptableObject
{
    private static ClickGameLevelNodeEventSO _instance;
    public static ClickGameLevelNodeEventSO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<ClickGameLevelNodeEventSO>("Scriptable Object/Event/ClickGameLevelNodeEventSO");
                if (_instance == null)
                {
                    Debug.LogError("�L�k�b Resources ����� ����SO");
                }
            }
            return _instance;
        }
    }

    public UnityAction<GameLevelNode> OnEventRaisedWithGameLevelNode;

    public void RaiseEvent(GameLevelNode gameLevelNode)
    {
        OnEventRaisedWithGameLevelNode?.Invoke(gameLevelNode);
    }
}
