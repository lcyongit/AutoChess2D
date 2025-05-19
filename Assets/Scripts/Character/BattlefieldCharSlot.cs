using System;
using UnityEngine;

public class BattlefieldCharSlot : MonoBehaviour, IInteractable
{
    [Header("Battle Slot Info")]
    public Character currentCharacter;
    public Vector2Int battleSlotPosOrder = new (0, 0);

    [Header("Slot State")]
    public bool isEmpty;

    [Header("Draw Square")]
    public Vector2 size = new(2f, 2f);
    public Color lineColor = Color.red;

    private LineRenderer lineRenderer;



    private Character movingCharacter;

    private void OnEnable()
    {
        ClickBattleCharSlotEventSO.Instance.OnEventRaisedWithCharacter += OnClickBattleCharSlotEvent;
        ClickBattleCharacterEventSO.Instance.OnEventRaisedWithCharacter += OnClickBattleCharacterEventEvent;
    }

    private void OnDisable()
    {
        ClickBattleCharSlotEventSO.Instance.OnEventRaisedWithCharacter -= OnClickBattleCharSlotEvent;
        ClickBattleCharacterEventSO.Instance.OnEventRaisedWithCharacter -= OnClickBattleCharacterEventEvent;
    }

    

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        SetEmpty();

    }
    private void Start()
    {
        
    }

    /// <summary>
    /// 戰鬥位置設為空
    /// </summary>
    public void SetEmpty()
    {
        isEmpty = true;
        currentCharacter = null;
    }

    /// <summary>
    /// 角色放入戰鬥位置
    /// </summary>
    /// <param name="character"></param>
    public void SetCharacter(Character character)
    {
        isEmpty = false;
        currentCharacter = character;
        character.battleSlotPosOrder = battleSlotPosOrder;

    }

    private void DrawBattleCharSlotSquare()
    {
        lineRenderer.positionCount = 5; // 繪製四條線 + 封閉線
        lineRenderer.loop = false;
        lineRenderer.useWorldSpace = false;

        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;

        // 設定框框頂點位置
        Vector3[] points = new Vector3[5]
        {
            new Vector3(-size.x / 2, -size.y / 2, 0),
            new Vector3(-size.x / 2,  size.y / 2, 0),
            new Vector3( size.x / 2,  size.y / 2, 0),
            new Vector3( size.x / 2, -size.y / 2, 0),
            new Vector3(-size.x / 2, -size.y / 2, 0)
        };
        lineRenderer.SetPositions(points);
    }

    private void ClearBattleCharSlotSquare()
    {
        lineRenderer.positionCount = 0;
        
    }

    #region Events

    public void OnClickLB()
    {
        // 恢復移動中角色圖案的透明度
        if (movingCharacter != null)
            movingCharacter.characterRenderer.color = new Color(1f, 1f, 1f, 1f);

        var originalSlot = movingCharacter.currentBattlefieldCharSlot;
        var beChangedSlot = this;

        // 位置為空 => 放置
        if (isEmpty)
        {
            originalSlot.SetEmpty();
            beChangedSlot.SetCharacter(movingCharacter);

            currentCharacter.currentBattlefieldCharSlot = beChangedSlot;

            // 改變角色位置
            currentCharacter.transform.position = transform.position;

        }
        // 位置上有別的角色 => 交換位置
        else if (!isEmpty)
        {
            if (currentCharacter != movingCharacter)
            {
                var originalCharacter = movingCharacter;
                var beChangedCharacter = currentCharacter;

                // 改變 位置裡面的角色
                originalSlot.SetCharacter(beChangedCharacter);
                beChangedSlot.SetCharacter(originalCharacter);

                // 改變 角色裡面的位置
                originalCharacter.currentBattlefieldCharSlot = beChangedSlot;
                beChangedCharacter.currentBattlefieldCharSlot = originalSlot;

                // 改變 角色位置
                originalCharacter.transform.position = transform.position;
                beChangedCharacter.transform.position = originalSlot.transform.position;

            }

        }

        movingCharacter = null;


    }

    private void OnClickBattleCharSlotEvent(Character character)
    {
        movingCharacter = character;
        ClearBattleCharSlotSquare();
    }

    private void OnClickBattleCharacterEventEvent(Character arg0)
    {
        DrawBattleCharSlotSquare();

    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(2f, 2f, 0f));
    }
}
