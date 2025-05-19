using System.Collections.Generic;
using UnityEngine;

public class GameLevelNode : MonoBehaviour, IInteractable
{
    [Header("GameLevel Node Info")]
    public GameLevel gameLevel;
    public Vector2Int gameLevelNode = new(0, 0);
    public bool isWalkable = false;  // �i�H�e�� (�U�@�B�ਫ���`�I)
    public bool isConnected = false; // �U�@�B�ਫ���`�Ito �M ����to���`�I
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
    /// �]�w�`�I�~�[
    /// </summary>
    public void SetNodeAppearance()
    {
        // ���s�����`�I => �z��
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

        // �������`�I
        if (isFinished)
        {
            spriteRenderer.color = new(0, 255, 0, 255);

        }

        // �i�H�I�����`�I
        if (isWalkable)
        {
            spriteRenderer.color = new(0, 255, 255, 255);

        }


    }

    public void OnClickLB()
    {
        // 1. �������`�I�����I��
        // 2. isWalkable�~���I��
        if (isWalkable && !isFinished)
        {
            ClickGameLevelNodeEventSO.Instance.RaiseEvent(this);

        }
        Debug.Log("click gameLevel");
    }


}
