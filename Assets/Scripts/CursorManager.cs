using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : Singleton<CursorManager>
{
    [Header("Components")]
    public BattlefieldCharSlotHandler battlefieldCharSlotHandler;

    [Header("Mouse on World Position")]
    private Vector3 MouseOnWorldPos =>
        Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));

    [Header("Cursor State")]
    public bool isMovingCharacter;

    private Character movingCharacter;

    private bool canClick;

    private void OnEnable()
    {
        ClickBattleCharacterEventSO.Instance.OnEventRaisedWithCharacter += OnClickBattleCharacterEvent;
    }

    private void OnDisable()
    {
        ClickBattleCharacterEventSO.Instance.OnEventRaisedWithCharacter -= OnClickBattleCharacterEvent;

    }

    

    protected override void Awake()
    {
        base.Awake();


    }

    private void Update()
    {
        canClick = ObjectOnMousePosition();

        // 戰鬥場景 - 角色在拖曳中的滑鼠位置
        if (battlefieldCharSlotHandler.gameObject.activeInHierarchy)
        {
            // MouseOnWorldPos的Z軸數值為攝影機的Z軸數值
            // 所以要手動改動Z軸
            battlefieldCharSlotHandler.transform.position = 
                new Vector3(MouseOnWorldPos.x, MouseOnWorldPos.y, 0f);
            
        }

        if (IsInteractWithUI())
        {

        }

        if (canClick && Input.GetMouseButtonDown(0))
        {
            OnClickObject(ObjectOnMousePosition().gameObject);

        }



    }

    /// <summary>
    /// 滑鼠點擊場景物件
    /// </summary>
    /// <param name="clickGameObject"></param>
    private void OnClickObject(GameObject clickGameObject)
    {
        var clickGameObjectTag = clickGameObject.tag;

        //FIXME: 可以先判定場景決定啟用哪個
        // 關卡場景 - 點擊遊戲關卡
        if (clickGameObjectTag == TagManager.Instance.gameLevelSlotTag)
        {
            if (clickGameObject.TryGetComponent<GameLevelNode>(out var gameLevelSlot))
                gameLevelSlot.OnClickLB();
        }
        // 戰鬥場景 - 角色放到戰鬥位置
        else if (clickGameObjectTag == TagManager.Instance.playerCharSlotTag && isMovingCharacter)
        {
            if (clickGameObject.TryGetComponent<BattlefieldCharSlot>(out var playerCharSlot))
            {
                isMovingCharacter = false;
                battlefieldCharSlotHandler.gameObject.SetActive(isMovingCharacter);
                ClickBattleCharSlotEventSO.Instance.RaiseEvent(movingCharacter);
                playerCharSlot.OnClickLB();
            }
        }
        // 戰鬥場景 - 拖曳角色
        else if (clickGameObjectTag == TagManager.Instance.playerCharTag && !isMovingCharacter)
        {
            if (clickGameObject.TryGetComponent<Character>(out var playerChar))
            {
                playerChar.OnClickLB();
            }
        }



    }

    /// <summary>
    /// 偵測滑鼠位置上的碰撞體
    /// </summary>
    /// <returns></returns>
    private Collider2D ObjectOnMousePosition()
    {
        //if (Physics2D.OverlapPoint(MouseWorldPos).gameObject != null)
        //    Debug.Log(Physics2D.OverlapPoint(MouseWorldPos).gameObject.name);
        return Physics2D.OverlapPoint(MouseOnWorldPos);
    }

    private bool IsInteractWithUI()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return true;
        else
            return false;
    }

    #region Events

    private void OnClickBattleCharacterEvent(Character character)
    {
        // 顯示移動角色的圖案
        isMovingCharacter = true;
        movingCharacter = character;
        battlefieldCharSlotHandler.gameObject.SetActive(isMovingCharacter);
        battlefieldCharSlotHandler.displayCharSpriteRenderer.sprite = character.characterDetails.sprite;

    }

    #endregion


}
