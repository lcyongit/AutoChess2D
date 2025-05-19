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

        // �԰����� - ����b�즲�����ƹ���m
        if (battlefieldCharSlotHandler.gameObject.activeInHierarchy)
        {
            // MouseOnWorldPos��Z�b�ƭȬ���v����Z�b�ƭ�
            // �ҥH�n��ʧ��Z�b
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
    /// �ƹ��I����������
    /// </summary>
    /// <param name="clickGameObject"></param>
    private void OnClickObject(GameObject clickGameObject)
    {
        var clickGameObjectTag = clickGameObject.tag;

        //FIXME: �i�H���P�w�����M�w�ҥέ���
        // ���d���� - �I���C�����d
        if (clickGameObjectTag == TagManager.Instance.gameLevelSlotTag)
        {
            if (clickGameObject.TryGetComponent<GameLevelNode>(out var gameLevelSlot))
                gameLevelSlot.OnClickLB();
        }
        // �԰����� - ������԰���m
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
        // �԰����� - �즲����
        else if (clickGameObjectTag == TagManager.Instance.playerCharTag && !isMovingCharacter)
        {
            if (clickGameObject.TryGetComponent<Character>(out var playerChar))
            {
                playerChar.OnClickLB();
            }
        }



    }

    /// <summary>
    /// �����ƹ���m�W���I����
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
        // ��ܲ��ʨ��⪺�Ϯ�
        isMovingCharacter = true;
        movingCharacter = character;
        battlefieldCharSlotHandler.gameObject.SetActive(isMovingCharacter);
        battlefieldCharSlotHandler.displayCharSpriteRenderer.sprite = character.characterDetails.sprite;

    }

    #endregion


}
