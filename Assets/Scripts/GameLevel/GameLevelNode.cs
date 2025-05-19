using System.Collections.Generic;
using UnityEngine;

public class GameLevelNode : MonoBehaviour, IInteractable
{
    [Header("GameLevel Node Info")]
    public GameLevel gameLevel;
    public Vector2Int gameLevelNode = new(0, 0);
    public bool isWalkable = false;  // 可以前往 (下一步能走的節點)
    public bool isConnected = false; // 下一步能走的節點to 和 後續to的節點
    public bool isFinished = false;

    //public List<GameLevelNode> from;
    public List<GameLevelNode> to;

    [Header("Battle Info")]
    public List<CharacterName> enemyList;

    [Header("Appearance")]
    public SpriteRenderer spriteRenderer;


    private void Awake()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 設定節點外觀
    /// </summary>
    public void SetNodeAppearance()
    {
        // 未連接的節點 => 透明
        if (!isConnected)
        {
            Color color = spriteRenderer.color;
            color.a = 0.3f;
            spriteRenderer.color = color;
        }
        else
        {
            Color color = spriteRenderer.color;
            color.a = 1f;
            spriteRenderer.color = color;
        }

        // 完成的節點
        if (isFinished)
        {
            spriteRenderer.color = new(0, 255, 0, 255);

        }

        // 可以點擊的節點
        if (isWalkable)
        {
            spriteRenderer.color = new(0, 255, 255, 255);

        }


    }

    public void OnClickLB()
    {
        // 1. 完成的節點不能點擊
        // 2. isWalkable才能點擊
        if (isWalkable && !isFinished)
        {
            ClickGameLevelNodeEventSO.Instance.RaiseEvent(this);

        }
        Debug.Log("click gameLevel");
    }


}
