using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
    [Header("Data SO")]
    public GameLevelDataListSO gameLevelDataListSO;
    public CharacterDataListSO characterDataListSO;

    [Header("ISavable List")]
    public HashSet<ISavable> savableHashSet = new();

    [Header("Save Data With Json")]
    private string jsonFolder;
    private string resultPath;


    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void Start()
    {
        jsonFolder = Application.persistentDataPath + "/Save/";
        resultPath = jsonFolder + "SaveData.sav";

        InitializePlayerSavedDataSO();

    }

    private void Update()
    {
        //if (Input.GetKeyUp(KeyCode.S))
        //{
        //    SaveDataInDisk();

        //}

        //if (Input.GetKeyUp(KeyCode.L))
        //{
        //    LoadDataFromDisk();

        //}


    }

    /// <summary>
    /// 開啟遊戲時初始化PlayerSavedDataSO
    /// </summary>
    private void InitializePlayerSavedDataSO()
    {
        // 遊戲開啟
        // 初始化玩家節點位置(-1, -1)
        PlayerSavedDataSO.Instance.playerCurrentChosenGameLevelNode.gameLevelNode = new Vector2Int(-1, -1);
        PlayerSavedDataSO.Instance.playerCurrentChosenGameLevelNode.gameLevel = GameLevel.Empty;
        PlayerSavedDataSO.Instance.playerCurrentChosenGameLevelNode.enemyList.Clear();
        PlayerSavedDataSO.Instance.gameLeveNodeSavedDataList.Clear();

        // 初始化玩家擁有的角色
        //TODO: 初始化玩家擁有的角色

    }

    /// <summary>
    /// 開始新遊戲
    /// </summary>
    public void StartNewGame()
    {
        // 初始化 playerSavedDataSO

        // 清除已生成的關卡和路線
        PlayerSavedDataSO.Instance.gameLeveNodeSavedDataList.Clear();

        // 玩家節點位置(0, 0)
        PlayerSavedDataSO.Instance.playerCurrentChosenGameLevelNode.gameLevelNode = new Vector2Int(0, 0);

        //FIXME: 先隨機給玩家角色，之後修改為開始新遊戲後選擇角色
        PlayerSavedDataSO.Instance.playerCharacterInfoList.Clear();
        int charCount = 5;
        for (int i = 0; i < charCount; i++)
        {
            List<int> randomList = new() { (int)CharacterName.Knight, (int)CharacterName.Lancer, (int)CharacterName.Archer};
            int randomInt = CustomUtil.RandomEnum(randomList);

            CharacterInfo characterInfo = new()
            {
                characterName = (CharacterName)randomInt,
                level = 1, 
                starRate = 1,
                battleSlotPosOrder = new(0,  i),

            };

            PlayerSavedDataSO.Instance.playerCharacterInfoList.Add(characterInfo);

        }
        
;
    }

    /// <summary>
    /// 遊戲存檔
    /// </summary>
    public void SaveData()
    {
        foreach (var savable in savableHashSet)
        {
            savable.SaveData();
        }

        Debug.Log("遊戲存檔");

    }

    /// <summary>
    /// 遊戲讀取
    /// </summary>
    public void LoadData()
    {
        foreach (var savable in savableHashSet)
        {
            savable.LoadData();
        }

        Debug.Log("遊戲讀取");

    }

    /// <summary>
    /// 本地存檔
    /// </summary>
    public void SaveDataInDisk()
    {
        // 本地存檔的資料
        SavedData savedData = new()
        {
            playerCurrentChosenGameLevelNode = PlayerSavedDataSO.Instance.playerCurrentChosenGameLevelNode,
            playerCharacterInfoList = PlayerSavedDataSO.Instance.playerCharacterInfoList,
            gameLeveNodeSavedDataList = PlayerSavedDataSO.Instance.gameLeveNodeSavedDataList, 

        };

        // Vector2Int轉換為json會多出`magnitude` 和 `sqrMagnitude`欄位

        var jsonData = JsonConvert.SerializeObject(savedData, Formatting.Indented);

        if (!File.Exists(resultPath))
            Directory.CreateDirectory(jsonFolder);

        File.WriteAllText(resultPath, jsonData);

        Debug.Log("本地存檔，存檔位置: " + resultPath);

    }

    /// <summary>
    /// 本地讀取
    /// </summary>
    public void LoadDataFromDisk()
    {
        if (!File.Exists(resultPath)) return;

        var jsonData = File.ReadAllText(resultPath);

        var LoadedData = JsonConvert.DeserializeObject<SavedData>(jsonData);

        // 將資料放進playerSavedDataSO
        PlayerSavedDataSO.Instance.playerCurrentChosenGameLevelNode = LoadedData.playerCurrentChosenGameLevelNode;
        PlayerSavedDataSO.Instance.playerCharacterInfoList = LoadedData.playerCharacterInfoList;
        PlayerSavedDataSO.Instance.gameLeveNodeSavedDataList = LoadedData.gameLeveNodeSavedDataList;

        Debug.Log("本地讀取");

    }

    

    #region Register and UnRegister

    public void RegisterISavable(ISavable ISavable)
    {
        savableHashSet.Add(ISavable);

    }

    public void UnRegisterISavable(ISavable ISavable)
    {
        savableHashSet.Remove(ISavable);
    }

    #endregion

    #region Event



    #endregion

    /// <summary>
    /// 儲存到電腦的資料類型
    /// </summary>
    private class SavedData
    {
        // 只保留Json能支援的資料型別

        [Header("Current GameLevel Info")]
        public GameLeveNodeSavedData playerCurrentChosenGameLevelNode;
        public List<GameLeveNodeSavedData> gameLeveNodeSavedDataList;

        [Header("Player-Owned Characters")]
        public List<CharacterInfo> playerCharacterInfoList = new();



    }


}
