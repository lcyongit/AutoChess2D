using System.Collections.Generic;
using UnityEngine;

public class GameLevelController : MonoBehaviour, ISavable
{
    [Header("Scene")]
    public GameSceneDataSO thisScene;

    [Header("Current GameLevel Node Info")]
    // 玩家位置圖標
    public GameObject playerLocationIconPrefab;
    // 玩家目前所在的關卡節點位置 (col, row) = (0, 0) => 初次進入地圖
    public Vector2Int playerOnCurrentGameLevelNodePos = new();
    // 玩家目前選擇的遊戲關卡
    public GameLevelNode currentChosenGameLevelNode;

    [Header("All GameLevel Nodes List")]
    private List<List<GameLevelNode>> allGameLevelNodeList = new();

    [Header("GameLevel Node")]
    public List<GameLevelNode> gameLevel0 = new();
    public List<GameLevelNode> gameLevel1 = new();
    public List<GameLevelNode> gameLevel2 = new();
    public List<GameLevelNode> gameLevel3 = new();

    [Header("Draw Line")]
    private DrawLine drawLine;

    [Header("State")]
    private bool isGameLevelGenerated;
    private static bool isInitialized;

    private GameObject playerLocationIcon;

    private void OnEnable()
    {
        isInitialized = true;

        ISavable savable = this;
        savable.RegisterISavable();

        AfterSceneLoadedEventSO.Instance.RegisterListener(OnAfterSceneLoadedEvent);
        ClickGameLevelNodeEventSO.Instance.OnEventRaisedWithGameLevelNode += OnClickGameLevelNodeEvent;
        
    }

    private void OnDisable()
    {
        ISavable savable = this;
        savable.UnRegisterISavable();

        AfterSceneLoadedEventSO.Instance.UnregisterListener(OnAfterSceneLoadedEvent);
        ClickGameLevelNodeEventSO.Instance.OnEventRaisedWithGameLevelNode -= OnClickGameLevelNodeEvent;

        isInitialized = false;
    }

    public static bool IsInitialized()
    {
        return isInitialized;
    }

    private void Awake()
    {
        drawLine = GetComponent<DrawLine>();

        AddGameNodes();
        SetGameLevelNodePos();


    }

    /// <summary>
    /// 加入地圖所有節點到allGameLevelNodeList
    /// </summary>
    private void AddGameNodes()
    {
        allGameLevelNodeList.Add(gameLevel0);
        allGameLevelNodeList.Add(gameLevel1);
        allGameLevelNodeList.Add(gameLevel2);
        allGameLevelNodeList.Add(gameLevel3);

    }

    /// <summary>
    /// 自動賦予關卡節點位置(x, y)
    /// </summary>
    private void SetGameLevelNodePos()
    {
        for (int i = 0; i < allGameLevelNodeList.Count; i++)
        {
            for (int j = 0; j < allGameLevelNodeList[i].Count; j++)
            {
                allGameLevelNodeList[i][j].gameLevelNode = new Vector2Int(i, j);
            }
        }

    }

    /// <summary>
    /// 隨機產生關卡
    /// </summary>
    public void GenerateGameLevel()
    {
        //FIXME: 修改關卡數量 => 固定或是隨機
        for (int i = 0; i < allGameLevelNodeList.Count; i++)
        {
            //int randomIndex = UnityEngine.Random.Range(1, Enum.GetValues(typeof(GameLevel)).Length);

            // enumList 放入要隨機的關卡enum
            List<int> enumList = new()
            {
                (int)GameLevel.Battle
            };

            int randomGameLevel = CustomUtil.RandomEnum(enumList);

            GameLevel gameLevel = (GameLevel)randomGameLevel;

            for (int j = 0; j < allGameLevelNodeList[i].Count; j++)
            {
                // (0, 0) 為玩家初始位置 沒有關卡資訊
                if (i == 0 && j == 0)
                    continue;

                GameLevelNode generateGameLevelNode = allGameLevelNodeList[i][j];
                generateGameLevelNode.gameLevel = gameLevel;

                // 戰鬥關卡: 隨機產生敵人種類
                if (gameLevel == GameLevel.Battle)
                {
                    for (int k = 0; k < GameDataManager.Instance.gameLevelDataListSO.GetGameLevelDetails(gameLevel).enemyCount; k++)
                    {
                        int randomEnemy = UnityEngine.Random.Range(1, 3);
                        CharacterName enemy = (CharacterName)randomEnemy;

                        generateGameLevelNode.enemyList.Add(enemy);

                    }


                }
            }


        }


    }

    /// <summary>
    /// 隨機產生關卡節點
    /// </summary>
    public void GeneratePath()
    {
        // 產生路徑機率
        float generatePathRatio = (1f / 2f) * 100f;

        // x = 目前這行
        for (int x = 0; x < allGameLevelNodeList.Count - 1; x++)
        {
            // [已連接的最後一個節點]
            int lastConnectedNodeIndex = -1;

            int y = x + 1; // y = x的下一行

            // 清除原有路線
            for (int i = 0; i < allGameLevelNodeList[x].Count; i++)
            {
                allGameLevelNodeList[x][i].to.Clear();
            }

            // 開始產生路線
            for (int i = 0; i < allGameLevelNodeList[x].Count; i++)
            {
                if (i == 0) // List 1 第一個節點
                {
                    // List 1 只有一個節點 直接連接 List 2 所有節點
                    if (allGameLevelNodeList[x].Count == 1)
                    {
                        for (int j = 0; j < allGameLevelNodeList[y].Count; j++)
                        {
                            allGameLevelNodeList[x][i].to.Add(allGameLevelNodeList[y][j]);
                        }

                        break;

                    }

                    // 連接 List 2 第一個節點
                    allGameLevelNodeList[x][i].to.Add(allGameLevelNodeList[y][0]);
                    lastConnectedNodeIndex = 0;

                    // 剩餘節點
                    for (int j = lastConnectedNodeIndex + 1; j < allGameLevelNodeList[y].Count; j++)
                    {
                        // 隨機產生路徑
                        if (UnityEngine.Random.Range(0f, 100f) > generatePathRatio)
                        {
                            allGameLevelNodeList[x][i].to.Add(allGameLevelNodeList[y][j]);
                            lastConnectedNodeIndex++;
                        }
                        else
                        {
                            break;
                        }

                    }

                }
                else // List 1 其他節點
                {
                    // 節點是否連接 List 2 [已連接的最後一個節點]
                    bool isPathToLastConnectedNode = false;

                    // 如果所有節點都已連接，直接和 List 2 [已連接的最後一個節點] 產生路徑
                    if (lastConnectedNodeIndex == allGameLevelNodeList[y].Count - 1)
                    {
                        allGameLevelNodeList[x][i].to.Add(allGameLevelNodeList[y][lastConnectedNodeIndex]);

                    }
                    else
                    {
                        // List 2 [已連接的最後一個節點] -> 隨機產生路徑
                        if (UnityEngine.Random.Range(0f, 100f) > generatePathRatio)
                        {
                            allGameLevelNodeList[x][i].to.Add(allGameLevelNodeList[y][lastConnectedNodeIndex]);
                            isPathToLastConnectedNode = true;
                        }

                        // List 2 [已連線的最後一個節點] 如果沒產生連線
                        // => List 2 [下一個還未連線的節點] 則必定產生連線
                        if (!isPathToLastConnectedNode)
                        {
                            // 必定產生連線
                            allGameLevelNodeList[x][i].to.Add(allGameLevelNodeList[y][lastConnectedNodeIndex + 1]);
                            lastConnectedNodeIndex++;
                        }

                        // List 2 剩餘節點
                        for (int j = lastConnectedNodeIndex + 1; j < allGameLevelNodeList[y].Count; j++)
                        {
                            // List 1 最後一個節點 和 List 2 剩餘節點全部連接
                            if (i == allGameLevelNodeList[x].Count - 1)
                            {
                                allGameLevelNodeList[x][i].to.Add(allGameLevelNodeList[y][j]);
                                lastConnectedNodeIndex++;

                            }
                            else
                            {
                                // 隨機產生路徑
                                if (UnityEngine.Random.Range(0f, 100f) > generatePathRatio)
                                {
                                    allGameLevelNodeList[x][i].to.Add(allGameLevelNodeList[y][j]);
                                    lastConnectedNodeIndex++;
                                }
                                else
                                {
                                    break;
                                }
                            }


                        }


                    }



                }


            }
        }


    }

    /// <summary>
    /// 依照玩家目前位置判定 能走的節點(isWalkable) 和 有連通的節點(isConnected)
    /// </summary>
    public void SetGameLevelNodeState()
    {
        // 初始化 開始節點isFinished = true
        allGameLevelNodeList[0][0].isFinished = true;

        // 初始化 isWalkable & isConnected 轉為false
        for (int i = 0; i < allGameLevelNodeList.Count; i++)
        {
            for (int j = 0; j < allGameLevelNodeList[i].Count; j++)
            {
                allGameLevelNodeList[i][j].isWalkable = false;
                allGameLevelNodeList[i][j].isConnected = false;
            }
        }

        // 設定 isConnected 狀態
        // 如果為起始點(0, 0) => 全部節點都連通 (isConnected = true)
        if (playerOnCurrentGameLevelNodePos.x == 0)
        {
            for (int i = 0; i < allGameLevelNodeList.Count; i++)
            {
                for (int j = 0; j < allGameLevelNodeList[i].Count; j++)
                {
                    allGameLevelNodeList[i][j].isConnected = true;
                }
            }
        }
        else
        {
            // 從當前節點 to 開始
            var currentNode = allGameLevelNodeList[playerOnCurrentGameLevelNodePos.x][playerOnCurrentGameLevelNodePos.y];
            SetCanGoInGameLevelNode(currentNode, true);

        }

        // 設定 isWalkable 狀態
        // 如果為起始點(0, 0) => (1, j)關卡 isWalkable = true
        if (playerOnCurrentGameLevelNodePos.x == 0)
        {
            for (int j = 0; j < allGameLevelNodeList[1].Count; j++)
            {
                allGameLevelNodeList[1][j].isWalkable = true;

            }
        }
        else
        {
            // 從當前節點 to 開始
            var currentNode = allGameLevelNodeList[playerOnCurrentGameLevelNodePos.x][playerOnCurrentGameLevelNodePos.y];

            // 選擇的節點如果有完成 => 後續節點 isWalkable = true
            if (currentNode.isFinished)
            {
                currentNode.isWalkable = false;

                for (int i = 0; i < currentNode.to.Count; i++)
                {
                    currentNode.to[i].isWalkable = true;
                }
            }
            // 選擇的節點如果沒有完成 => 選擇的節點 isWalkable = true
            else if (!currentNode.isFinished)
            {
                currentNode.isWalkable = true;
            }
            

        }

        SetNodeAppearance();

    }

    /// <summary>
    /// 設定節點外觀
    /// </summary>
    void SetNodeAppearance()
    {
        for (int i = 0; i < allGameLevelNodeList.Count; i++)
        {
            for (int j = 0; j < allGameLevelNodeList[i].Count; j++)
            {
                allGameLevelNodeList[i][j].SetNodeAppearance();
            }
        }
    }

    void SetCanGoInGameLevelNode(GameLevelNode node, bool value)
    {
        if (node == null) return;
        if (node.isConnected == value) return;

        node.isConnected = value;

        foreach (var toNode in node.to)
        {
            SetCanGoInGameLevelNode(toNode, value);
        }

    }

    void SetPlayerLocationIcon()
    {
        playerLocationIcon = Instantiate(playerLocationIconPrefab);
        var playerPos = allGameLevelNodeList[playerOnCurrentGameLevelNodePos.x][playerOnCurrentGameLevelNodePos.y].transform.position;
        playerLocationIcon.transform.position = new Vector3(playerPos.x, playerPos.y + 1.2f);

    }

    #region Events

    private void OnAfterSceneLoadedEvent()
    {
        // 開始新遊戲 => 重新產生關卡和路徑
        if (!isGameLevelGenerated)
        {
            GenerateGameLevel();
            GeneratePath();

            Debug.Log("產生遊戲關卡");

        }

        // 畫路線
        drawLine.GenerateCurvedLine(allGameLevelNodeList, 10);

        SetGameLevelNodeState();

    }

    private void OnClickGameLevelNodeEvent(GameLevelNode gameLevelNode)
    {
        currentChosenGameLevelNode = gameLevelNode;
    }

    #endregion

    public void SaveData()
    {
        // 儲存所有節點資訊
        PlayerSavedDataSO.Instance.gameLeveNodeSavedDataList.Clear();
        for (int i = 0; i < allGameLevelNodeList.Count; i++)
        {
            for (int j = 0; j < allGameLevelNodeList[i].Count; j++)
            {
                GameLeveNodeSavedData gameLeveNodeSavedData = new()
                {
                    gameLevel = allGameLevelNodeList[i][j].gameLevel,
                    gameLevelNode = allGameLevelNodeList[i][j].gameLevelNode,
                    isFinished = allGameLevelNodeList[i][j].isFinished,
                    enemyList = allGameLevelNodeList[i][j].enemyList,

                };

                foreach (var toNode in allGameLevelNodeList[i][j].to)
                {
                    Vector2Int toNodePos = toNode.gameLevelNode;
                    gameLeveNodeSavedData.toGameLevelNodeList.Add(toNodePos);
                }

                PlayerSavedDataSO.Instance.gameLeveNodeSavedDataList.Add(gameLeveNodeSavedData);

            }
        }

        // 儲存選擇的節點資訊
        if (currentChosenGameLevelNode != null)
        {
            GameLeveNodeSavedData gameLeveNodeSavedData = new()
            {
                gameLevel = currentChosenGameLevelNode.gameLevel,
                gameLevelNode = currentChosenGameLevelNode.gameLevelNode,
                isFinished = currentChosenGameLevelNode.isFinished,
                enemyList = currentChosenGameLevelNode.enemyList,

            };

            PlayerSavedDataSO.Instance.playerCurrentChosenGameLevelNode = gameLeveNodeSavedData;

            
        }

    }

    public void LoadData()
    {
        if (PlayerSavedDataSO.Instance.gameLeveNodeSavedDataList.Count != 0)
            isGameLevelGenerated = true;

        // 讀取所有節點資訊
        foreach (var node in PlayerSavedDataSO.Instance.gameLeveNodeSavedDataList)
        {
            int x = node.gameLevelNode.x;
            int y = node.gameLevelNode.y;
            allGameLevelNodeList[x][y].gameLevel = node.gameLevel;
            allGameLevelNodeList[x][y].isFinished = node.isFinished;
            allGameLevelNodeList[x][y].enemyList = node.enemyList;

            foreach (var toNode in node.toGameLevelNodeList)
            {
                int i = toNode.x;
                int j = toNode.y;
                allGameLevelNodeList[x][y].to.Add(allGameLevelNodeList[i][j]);

            }

        }

        // 讀取選擇的節點資訊 (要直接修改存在的節點)
        playerOnCurrentGameLevelNodePos = PlayerSavedDataSO.Instance.playerCurrentChosenGameLevelNode.gameLevelNode;
        int currentX = playerOnCurrentGameLevelNodePos.x;
        int currentY = playerOnCurrentGameLevelNodePos.y;
        currentChosenGameLevelNode = allGameLevelNodeList[currentX][currentY];
        currentChosenGameLevelNode.isFinished = PlayerSavedDataSO.Instance.playerCurrentChosenGameLevelNode.isFinished;

        // 放置玩家位置圖標
        SetPlayerLocationIcon();

    }

}
