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
    /// �}�ҹC���ɪ�l��PlayerSavedDataSO
    /// </summary>
    private void InitializePlayerSavedDataSO()
    {
        // �C���}��
        // ��l�ƪ��a�`�I��m(-1, -1)
        PlayerSavedDataSO.Instance.playerCurrentChosenGameLevelNode.gameLevelNode = new Vector2Int(-1, -1);
        PlayerSavedDataSO.Instance.playerCurrentChosenGameLevelNode.gameLevel = GameLevel.Empty;
        PlayerSavedDataSO.Instance.playerCurrentChosenGameLevelNode.enemyList.Clear();
        PlayerSavedDataSO.Instance.gameLeveNodeSavedDataList.Clear();

        // ��l�ƪ��a�֦�������
        //TODO: ��l�ƪ��a�֦�������

    }

    /// <summary>
    /// �}�l�s�C��
    /// </summary>
    public void StartNewGame()
    {
        // ��l�� playerSavedDataSO

        // �M���w�ͦ������d�M���u
        PlayerSavedDataSO.Instance.gameLeveNodeSavedDataList.Clear();

        // ���a�`�I��m(0, 0)
        PlayerSavedDataSO.Instance.playerCurrentChosenGameLevelNode.gameLevelNode = new Vector2Int(0, 0);

        //FIXME: ���H�������a����A����קאּ�}�l�s�C�����ܨ���
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
    /// �C���s��
    /// </summary>
    public void SaveData()
    {
        foreach (var savable in savableHashSet)
        {
            savable.SaveData();
        }

        Debug.Log("�C���s��");

    }

    /// <summary>
    /// �C��Ū��
    /// </summary>
    public void LoadData()
    {
        foreach (var savable in savableHashSet)
        {
            savable.LoadData();
        }

        Debug.Log("�C��Ū��");

    }

    /// <summary>
    /// ���a�s��
    /// </summary>
    public void SaveDataInDisk()
    {
        // ���a�s�ɪ����
        SavedData savedData = new()
        {
            playerCurrentChosenGameLevelNode = PlayerSavedDataSO.Instance.playerCurrentChosenGameLevelNode,
            playerCharacterInfoList = PlayerSavedDataSO.Instance.playerCharacterInfoList,
            gameLeveNodeSavedDataList = PlayerSavedDataSO.Instance.gameLeveNodeSavedDataList, 

        };

        // Vector2Int�ഫ��json�|�h�X`magnitude` �M `sqrMagnitude`���

        var jsonData = JsonConvert.SerializeObject(savedData, Formatting.Indented);

        if (!File.Exists(resultPath))
            Directory.CreateDirectory(jsonFolder);

        File.WriteAllText(resultPath, jsonData);

        Debug.Log("���a�s�ɡA�s�ɦ�m: " + resultPath);

    }

    /// <summary>
    /// ���aŪ��
    /// </summary>
    public void LoadDataFromDisk()
    {
        if (!File.Exists(resultPath)) return;

        var jsonData = File.ReadAllText(resultPath);

        var LoadedData = JsonConvert.DeserializeObject<SavedData>(jsonData);

        // �N��Ʃ�iplayerSavedDataSO
        PlayerSavedDataSO.Instance.playerCurrentChosenGameLevelNode = LoadedData.playerCurrentChosenGameLevelNode;
        PlayerSavedDataSO.Instance.playerCharacterInfoList = LoadedData.playerCharacterInfoList;
        PlayerSavedDataSO.Instance.gameLeveNodeSavedDataList = LoadedData.gameLeveNodeSavedDataList;

        Debug.Log("���aŪ��");

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
    /// �x�s��q�����������
    /// </summary>
    private class SavedData
    {
        // �u�O�dJson��䴩����ƫ��O

        [Header("Current GameLevel Info")]
        public GameLeveNodeSavedData playerCurrentChosenGameLevelNode;
        public List<GameLeveNodeSavedData> gameLeveNodeSavedDataList;

        [Header("Player-Owned Characters")]
        public List<CharacterInfo> playerCharacterInfoList = new();



    }


}
