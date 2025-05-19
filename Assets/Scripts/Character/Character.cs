using System;
using UnityEngine;

public class Character : MonoBehaviour, IInteractable
{
    [Header("Components")]
    public Collider2D triggerCollider;  // 拖曳角色用碰撞體

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
        // 角色圖案 => 半透明
        characterRenderer.color = new Color(1, 1, 1, 0.5f);

        ClickBattleCharacterEventSO.Instance.RaiseEvent(this);

        Debug.Log("click character");

    }

    private void OnClickBattleCharacterEvent()
    {
        // 選取要移動的角色後，所有角色關閉collider避免被滑鼠射線選中
        triggerCollider.enabled = false;
    }

    private void OnClickBattleCharSlotEvent()
    {
        // 放置角色後，所有角色開啟collider
        triggerCollider.enabled = true;
    }

    private void OnBattleStartEvent()
    {
        // 戰鬥開始後，所有角色關閉collider避免被滑鼠射線選中
        triggerCollider.enabled = false;
    }

    #endregion
}
