using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSavedDataSO", menuName = "Scriptable Objects/Data/PlayerSavedDataSO")]
public class PlayerSavedDataSO : ScriptableObject
{
    private static PlayerSavedDataSO _instance;
    public static PlayerSavedDataSO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<PlayerSavedDataSO>("Scriptable Object/Data/PlayerSavedDataSO");
                if (_instance == null)
                {
                    Debug.LogError("無法在 Resources 中找到 對應SO");
                }
            }
            return _instance;
        }
    }

    // *** => 支援本地存檔

    // 玩家遊戲進度

    [Header("Player Game Progress Info")]
    // *** 玩家目前所在的關卡節點位置 (col, row) = (0, 0) => 初次進入地圖
    // 遊戲進入初始化為(-1, -1)，選擇開始新遊戲將變為(0, 0)或是讀取遊戲進度
    public GameLeveNodeSavedData playerCurrentChosenGameLevelNode;

    // *** 所有節點資訊
    public List<GameLeveNodeSavedData> gameLeveNodeSavedDataList = new();

    // *** 玩家隊伍資訊
    [Header("Player-Owned Characters")]
    public List<CharacterInfo> playerCharacterInfoList = new();


    
}

[Serializable]
public class CharacterInfo
{
    public CharacterName characterName;
    public int level;
    public int starRate;
    public Vector2Int battleSlotPosOrder;

}

[Serializable]
public class GameLeveNodeSavedData
{
    public GameLevel gameLevel;
    public Vector2Int gameLevelNode;
    public List<Vector2Int> toGameLevelNodeList = new();
    public bool isFinished;
    public List<CharacterName> enemyList = new();

}
