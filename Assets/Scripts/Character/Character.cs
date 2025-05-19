using System;
using UnityEngine;

public class Character : MonoBehaviour, IInteractable
{
    [Header("Components")]
    public Collider2D triggerCollider;  // �즲����θI����

    [Header("Battlefield Info")]
    public BattlefieldCharSlot currentBattlefieldCharSlot;
    public Vector2Int battleSlotPosOrder = new(-1, -1);

    [Header("Character Info")]
    public CharacterDetails characterDetails;
    public SpriteRenderer characterRenderer;



    private void OnEnable()
    {
        ClickBattleCharacterEventSO.Instance.OnEventRaised += OnClickBattleCharacterEvent;
        ClickBattleCharSlotEventSO.Instance.OnEventRaised += OnClickBattleCharSlotEvent;
        BattleStartEventSO.Instance.OnEventRaised += OnBattleStartEvent;
    }

    private void OnDisable()
    {
        ClickBattleCharacterEventSO.Instance.OnEventRaised -= OnClickBattleCharacterEvent;
        ClickBattleCharSlotEventSO.Instance.OnEventRaised -= OnClickBattleCharSlotEvent;
        BattleStartEventSO.Instance.OnEventRaised -= OnBattleStartEvent;

    }

    

    private void Awake()
    {
        var characterName = GetComponent<CharacterStats>().characterName;
        characterDetails = GameDataManager.Instance.characterDataListSO.GetCharacterDetails(characterName);
        characterRenderer = GetComponent<SpriteRenderer>();
        triggerCollider = GetComponent<Collider2D>();

    }


    #region Events

    public void OnClickLB()
    {
        // ����Ϯ� => �b�z��
        characterRenderer.color = new Color(1, 1, 1, 0.5f);

        ClickBattleCharacterEventSO.Instance.RaiseEvent(this);

        Debug.Log("click character");

    }

    private void OnClickBattleCharacterEvent()
    {
        // ����n���ʪ������A�Ҧ���������collider�קK�Q�ƹ��g�u�襤
        triggerCollider.enabled = false;
    }

    private void OnClickBattleCharSlotEvent()
    {
        // ��m�����A�Ҧ�����}��collider
        triggerCollider.enabled = true;
    }

    private void OnBattleStartEvent()
    {
        // �԰��}�l��A�Ҧ���������collider�קK�Q�ƹ��g�u�襤
        triggerCollider.enabled = false;
    }

    #endregion
}
