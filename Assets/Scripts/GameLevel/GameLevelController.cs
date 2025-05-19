using System.Collections.Generic;
using UnityEngine;

public class GameLevelController : MonoBehaviour, ISavable
{
    [Header("Scene")]
    public GameSceneDataSO thisScene;

    [Header("Current GameLevel Node Info")]
    // ���a��m�ϼ�
    public GameObject playerLocationIconPrefab;
    // ���a�ثe�Ҧb�����d�`�I��m (col, row) = (0, 0) => �즸�i�J�a��
    public Vector2Int playerOnCurrentGameLevelNodePos = new();
    // ���a�ثe��ܪ��C�����d
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
    /// �[�J�a�ϩҦ��`�I��allGameLevelNodeList
    /// </summary>
    private void AddGameNodes()
    {
        allGameLevelNodeList.Add(gameLevel0);
        allGameLevelNodeList.Add(gameLevel1);
        allGameLevelNodeList.Add(gameLevel2);
        allGameLevelNodeList.Add(gameLevel3);

    }

    /// <summary>
    /// �۰ʽᤩ���d�`�I��m(x, y)
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
    /// �H���������d
    /// </summary>
    public void GenerateGameLevel()
    {
        //FIXME: �ק����d�ƶq => �T�w�άO�H��
        for (int i = 0; i < allGameLevelNodeList.Count; i++)
        {
            //int randomIndex = UnityEngine.Random.Range(1, Enum.GetValues(typeof(GameLevel)).Length);

            // enumList ��J�n�H�������denum
            List<int> enumList = new()
            {
                (int)GameLevel.Battle
            };

            int randomGameLevel = CustomUtil.RandomEnum(enumList);

            GameLevel gameLevel = (GameLevel)randomGameLevel;

            for (int j = 0; j < allGameLevelNodeList[i].Count; j++)
            {
                // (0, 0) �����a��l��m �S�����d��T
                if (i == 0 && j == 0)
                    continue;

                GameLevelNode generateGameLevelNode = allGameLevelNodeList[i][j];
                generateGameLevelNode.gameLevel = gameLevel;

                // �԰����d: �H�����ͼĤH����
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
    /// �H���������d�`�I
    /// </summary>
    public void GeneratePath()
    {
        // ���͸��|���v
        float generatePathRatio = (1f / 2f) * 100f;

        // x = �ثe�o��
        for (int x = 0; x < allGameLevelNodeList.Count - 1; x++)
        {
            // [�w�s�����̫�@�Ӹ`�I]
            int lastConnectedNodeIndex = -1;

            int y = x + 1; // y = x���U�@��

            // �M���즳���u
            for (int i = 0; i < allGameLevelNodeList[x].Count; i++)
            {
                allGameLevelNodeList[x][i].to.Clear();
            }

            // �}�l���͸��u
            for (int i = 0; i < allGameLevelNodeList[x].Count; i++)
            {
                if (i == 0) // List 1 �Ĥ@�Ӹ`�I
                {
                    // List 1 �u���@�Ӹ`�I �����s�� List 2 �Ҧ��`�I
                    if (allGameLevelNodeList[x].Count == 1)
                    {
                        for (int j = 0; j < allGameLevelNodeList[y].Count; j++)
                        {
                            allGameLevelNodeList[x][i].to.Add(allGameLevelNodeList[y][j]);
                        }

                        break;

                    }

                    // �s�� List 2 �Ĥ@�Ӹ`�I
                    allGameLevelNodeList[x][i].to.Add(allGameLevelNodeList[y][0]);
                    lastConnectedNodeIndex = 0;

                    // �Ѿl�`�I
                    for (int j = lastConnectedNodeIndex + 1; j < allGameLevelNodeList[y].Count; j++)
                    {
                        // �H�����͸��|
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
                else // List 1 ��L�`�I
                {
                    // �`�I�O�_�s�� List 2 [�w�s�����̫�@�Ӹ`�I]
                    bool isPathToLastConnectedNode = false;

                    // �p�G�Ҧ��`�I���w�s���A�����M List 2 [�w�s�����̫�@�Ӹ`�I] ���͸��|
                    if (lastConnectedNodeIndex == allGameLevelNodeList[y].Count - 1)
                    {
                        allGameLevelNodeList[x][i].to.Add(allGameLevelNodeList[y][lastConnectedNodeIndex]);

                    }
                    else
                    {
                        // List 2 [�w�s�����̫�@�Ӹ`�I] -> �H�����͸��|
                        if (UnityEngine.Random.Range(0f, 100f) > generatePathRatio)
                        {
                            allGameLevelNodeList[x][i].to.Add(allGameLevelNodeList[y][lastConnectedNodeIndex]);
                            isPathToLastConnectedNode = true;
                        }

                        // List 2 [�w�s�u���̫�@�Ӹ`�I] �p�G�S���ͳs�u
                        // => List 2 [�U�@���٥��s�u���`�I] �h���w���ͳs�u
                        if (!isPathToLastConnectedNode)
                        {
                            // ���w���ͳs�u
                            allGameLevelNodeList[x][i].to.Add(allGameLevelNodeList[y][lastConnectedNodeIndex + 1]);
                            lastConnectedNodeIndex++;
                        }

                        // List 2 �Ѿl�`�I
                        for (int j = lastConnectedNodeIndex + 1; j < allGameLevelNodeList[y].Count; j++)
                        {
                            // List 1 �̫�@�Ӹ`�I �M List 2 �Ѿl�`�I�����s��
                            if (i == allGameLevelNodeList[x].Count - 1)
                            {
                                allGameLevelNodeList[x][i].to.Add(allGameLevelNodeList[y][j]);
                                lastConnectedNodeIndex++;

                            }
                            else
                            {
                                // �H�����͸��|
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
    /// �̷Ӫ��a�ثe��m�P�w �ਫ���`�I(isWalkable) �M ���s�q���`�I(isConnected)
    /// </summary>
    public void SetGameLevelNodeState()
    {
        // ��l�� �}�l�`�IisFinished = true
        allGameLevelNodeList[0][0].isFinished = true;

        // ��l�� isWalkable & isConnected �ରfalse
        for (int i = 0; i < allGameLevelNodeList.Count; i++)
        {
            for (int j = 0; j < allGameLevelNodeList[i].Count; j++)
            {
                allGameLevelNodeList[i][j].isWalkable = false;
                allGameLevelNodeList[i][j].isConnected = false;
            }
        }

        // �]�w isConnected ���A
        // �p�G���_�l�I(0, 0) => �����`�I���s�q (isConnected = true)
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
            // �q��e�`�I to �}�l
            var currentNode = allGameLevelNodeList[playerOnCurrentGameLevelNodePos.x][playerOnCurrentGameLevelNodePos.y];
            SetCanGoInGameLevelNode(currentNode, true);

        }

        // �]�w isWalkable ���A
        // �p�G���_�l�I(0, 0) => (1, j)���d isWalkable = true
        if (playerOnCurrentGameLevelNodePos.x == 0)
        {
            for (int j = 0; j < allGameLevelNodeList[1].Count; j++)
            {
                allGameLevelNodeList[1][j].isWalkable = true;

            }
        }
        else
        {
            // �q��e�`�I to �}�l
            var currentNode = allGameLevelNodeList[playerOnCurrentGameLevelNodePos.x][playerOnCurrentGameLevelNodePos.y];

            // ��ܪ��`�I�p�G������ => ����`�I isWalkable = true
            if (currentNode.isFinished)
            {
                currentNode.isWalkable = false;

                for (int i = 0; i < currentNode.to.Count; i++)
                {
                    currentNode.to[i].isWalkable = true;
                }
            }
            // ��ܪ��`�I�p�G�S������ => ��ܪ��`�I isWalkable = true
            else if (!currentNode.isFinished)
            {
                currentNode.isWalkable = true;
            }
            

        }

        SetNodeAppearance();

    }

    /// <summary>
    /// �]�w�`�I�~�[
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
        // �}�l�s�C�� => ���s�������d�M���|
        if (!isGameLevelGenerated)
        {
            GenerateGameLevel();
            GeneratePath();

            Debug.Log("���͹C�����d");

        }

        // �e���u
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
        // �x�s�Ҧ��`�I��T
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

        // �x�s��ܪ��`�I��T
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

        // Ū���Ҧ��`�I��T
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

        // Ū����ܪ��`�I��T (�n�����ק�s�b���`�I)
        playerOnCurrentGameLevelNodePos = PlayerSavedDataSO.Instance.playerCurrentChosenGameLevelNode.gameLevelNode;
        int currentX = playerOnCurrentGameLevelNodePos.x;
        int currentY = playerOnCurrentGameLevelNodePos.y;
        currentChosenGameLevelNode = allGameLevelNodeList[currentX][currentY];
        currentChosenGameLevelNode.isFinished = PlayerSavedDataSO.Instance.playerCurrentChosenGameLevelNode.isFinished;

        // ��m���a��m�ϼ�
        SetPlayerLocationIcon();

    }

}
