using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour, ISavable
{
    [Header("Scene")]
    public GameSceneDataSO thisScene;

    [Header("Scene State")]
    public bool isBattlePreparation;
    public bool isBattleWin;

    [Header("Battle Info")]
    public GameLevel gameLevel;
    public List<CharacterName> enemyList;
    public List<CharacterInfo> playerCharacterInfoList = new();
    private List<Character> playerCharacterList = new();

    public int remainingEnemyCount = 0;

    [Header("Battle Slot Setting")]
    public GameObject battleSlotPrefab;
    public Transform playerUnitBattlePosTrans;
    public Transform enemyUnitBattlePosTrans;
    public Vector2Int battleSlotColRowCount = new(0, 0);
    public Vector2 playerBattleSlotStartPos = new(0f, 0f);
    public Vector2 enemyBattleSlotStartPos = new(0f, 0f);
    public float battleSlotGap = 0f;

    [Header("Character BattleSystem")]
    public List<CharacterBattleSystem> playerBattleSystemList = new();
    public List<CharacterBattleSystem> enemyBattleSystemList = new();

    [Header("Character Spawn Points")]
    public List<GameObject> playerBattlePosList = new();
    public List<GameObject> enemyBattlePosList = new();



    [Header("State")]
    private static bool isInitialized;


    private void OnEnable()
    {
        isInitialized = true;

        ISavable savable = this;
        savable.RegisterISavable();

        AfterSceneLoadedEventSO.Instance.OnEventRaised += OnAfterSceneLoadedEvent;
        BattleStartEventSO.Instance.OnEventRaised += OnBattleStartEvent;
        CharacterDieEventSO.Instance.OnEventRaised += OnCharacterDieEvent;
    }

    private void OnDisable()
    {
        ISavable savable = this;
        savable.UnRegisterISavable();

        AfterSceneLoadedEventSO.Instance.OnEventRaised -= OnAfterSceneLoadedEvent;
        BattleStartEventSO.Instance.OnEventRaised -= OnBattleStartEvent;
        CharacterDieEventSO.Instance.OnEventRaised -= OnCharacterDieEvent;

        isInitialized = false;

    }

    public static bool IsInitialized()
    {
        return isInitialized;
    }

    private void Awake()
    {
        isBattlePreparation = true;
    }

    private void Start()
    {
        BattlePreparationEventSO.Instance.RaiseEvent();

    }
    /// <summary>
    /// 找尋最短距離的敵人
    /// </summary>
    /// <param name="charBS"></param>
    /// <returns></returns>
    public CharacterBattleSystem SearchTarget(CharacterBattleSystem charBS)
    {
        CharacterBattleSystem target = null;
        float closestDistance = Mathf.Infinity;

        if (charBS.CompareTag(TagManager.Instance.playerCharTag))
        {
            foreach (var targetCharBS in enemyBattleSystemList)
            {
                float distance = Vector3.Distance(charBS.transform.position, targetCharBS.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = targetCharBS;
                }

            }
        }
        else if (charBS.CompareTag(TagManager.Instance.enemyCharTag))
        {
            foreach (var targetCharBS in playerBattleSystemList)
            {
                float distance = Vector3.Distance(charBS.transform.position, targetCharBS.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = targetCharBS;
                }

            }
        }



        return target;

    }

    /// <summary>
    /// 生成戰鬥位置
    /// </summary>
    private void GenerateBattleSlotOnBattlefield()
    {
        // 生成玩家戰鬥位置
        for (int i = 0; i < battleSlotColRowCount.x; i++)
        {
            for (int j = 0; j < battleSlotColRowCount.y; j++)
            {
                GameObject battleSlot = Instantiate(battleSlotPrefab, playerUnitBattlePosTrans);
                BattlefieldCharSlot battlefieldCharSlot = battleSlot.GetComponent<BattlefieldCharSlot>();

                battleSlot.transform.position = 
                    new(playerBattleSlotStartPos.x + (i * battleSlotGap), 
                    playerBattleSlotStartPos.y - (j * battleSlotGap), 0f);
                battlefieldCharSlot.battleSlotPosOrder = new(i, j);


                playerBattlePosList.Add(battleSlot);

            }
        }

        // 生成敵人戰鬥位置
        for (int i = 0; i < battleSlotColRowCount.x; i++)
        {
            for (int j = 0; j < battleSlotColRowCount.y; j++)
            {
                GameObject battleSlot = Instantiate(battleSlotPrefab, enemyUnitBattlePosTrans);
                BattlefieldCharSlot battlefieldCharSlot = battleSlot.GetComponent<BattlefieldCharSlot>();

                battleSlot.transform.position =
                    new(enemyBattleSlotStartPos.x - (i * battleSlotGap),
                    enemyBattleSlotStartPos.y - (j * battleSlotGap), 0f);
                battlefieldCharSlot.battleSlotPosOrder = new(i, j);


                enemyBattlePosList.Add(battleSlot);

            }
        }
    }

    /// <summary>
    /// 生成角色
    /// </summary>
    private void GenerateCharacter()
    {
        // 生成我方角色
        for (int i = 0; i < playerCharacterInfoList.Count; i++)
        {
            InstantiatePlayerCharGameObject(playerCharacterInfoList[i], TagManager.Instance.playerCharTag);
        }

        // 生成敵方角色
        remainingEnemyCount = enemyList.Count;

        if (gameLevel == GameLevel.Battle)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                InstantiateEnemyCharGameObject(enemyList[i], TagManager.Instance.enemyCharTag);
            }
        }
    }

    /// <summary>
    /// 生成玩家角色
    /// </summary>
    /// <param name="charInfo"></param>
    /// <param name="tag"></param>
    public void InstantiatePlayerCharGameObject(CharacterInfo charInfo, string tag)
    {
        // 生成角色GameObject
        var charDetails = GameDataManager.Instance.characterDataListSO.GetCharacterDetails(charInfo.characterName);
        GameObject charGO = Instantiate(charDetails.prefab);
        charGO.tag = tag;
        var character = charGO.GetComponent<Character>();
        character.battleSlotPosOrder = charInfo.battleSlotPosOrder;
        playerCharacterList.Add(character);

        // 角色按照儲存資料放置戰鬥位置  
        if (charGO.CompareTag(TagManager.Instance.playerCharTag))
        {
            // 角色要放置的戰鬥位置
            foreach (GameObject battleSlot in playerBattlePosList)
            {
                var battlefieldCharSlot = battleSlot.GetComponent<BattlefieldCharSlot>();
                if (battlefieldCharSlot.isEmpty && battlefieldCharSlot.battleSlotPosOrder == character.battleSlotPosOrder)
                {
                    battlefieldCharSlot.isEmpty = false;
                    battlefieldCharSlot.currentCharacter = character;
                    character.currentBattlefieldCharSlot = battlefieldCharSlot;
                    charGO.transform.SetPositionAndRotation(battleSlot.transform.position, Quaternion.identity);
                    break;
                }
            }
        }

    }

    /// <summary>
    /// 生成敵方角色
    /// </summary>
    /// <param name="characterName"></param>
    /// <param name="tag"></param>
    public void InstantiateEnemyCharGameObject(CharacterName characterName, string tag)
    {
        // 生成角色GameObject
        var charDetails = GameDataManager.Instance.characterDataListSO.GetCharacterDetails(characterName);
        GameObject charGO = Instantiate(charDetails.prefab);
        charGO.tag = tag;
        var character = charGO.GetComponent<Character>();

        //FIXME: 敵人按照什麼?? 放置戰鬥位置
        // 預設位置為照著戰鬥位置順序放   
        if (charGO.CompareTag(TagManager.Instance.enemyCharTag))
        {
            // 角色要放置的戰鬥位置
            foreach (GameObject battleSlot in enemyBattlePosList)
            {
                var battlefieldCharSlot = battleSlot.GetComponent<BattlefieldCharSlot>();
                if (battlefieldCharSlot.isEmpty)
                {
                    battlefieldCharSlot.isEmpty = false;
                    battlefieldCharSlot.currentCharacter = character;
                    character.currentBattlefieldCharSlot = battlefieldCharSlot;
                    charGO.transform.SetPositionAndRotation(battleSlot.transform.position, Quaternion.identity);
                    charGO.GetComponent<SpriteRenderer>().flipX = true;
                    break;
                }
            }
        }

    }


    #region Register and UnRegister

    public void RegisterCharBS(CharacterBattleSystem charBS)
    {
        if (charBS.CompareTag(TagManager.Instance.playerCharTag))
        {
            if (!playerBattleSystemList.Contains(charBS))
                playerBattleSystemList.Add(charBS);
        }
        else if (charBS.CompareTag(TagManager.Instance.enemyCharTag))
        {
            if (!enemyBattleSystemList.Contains(charBS))
                enemyBattleSystemList.Add(charBS);
        }
    }

    public void UnRegisterCharBS(CharacterBattleSystem charBS)
    {
        if (charBS.CompareTag(TagManager.Instance.playerCharTag))
            playerBattleSystemList.Remove(charBS);
        else if (charBS.CompareTag(TagManager.Instance.enemyCharTag))
            enemyBattleSystemList.Remove(charBS);

    }

    #endregion

    #region Events

    private void OnAfterSceneLoadedEvent()
    {
        GenerateBattleSlotOnBattlefield();
        GenerateCharacter();

    }

    public void OnBattleStartEvent()
    {
        isBattlePreparation = false;
    }

    private void OnCharacterDieEvent()
    {
        remainingEnemyCount--;

        // 戰鬥結束條件: 敵人剩餘 = 0
        if(remainingEnemyCount == 0)
        {
            // 勝利
            isBattleWin = true;

            // 戰鬥結束 事件
            BattleEndEventSO.Instance.RaiseEvent();

        }
        //TODO:  戰鬥結束條件: 我方剩餘 = 0

    }


    #endregion

    /// <summary>
    /// 將角色CharacterStats儲存
    /// </summary>
    private void GetCharacterInfoInSavedData()
    {
        for (int i = 0; i < playerCharacterInfoList.Count; i++)
        {
            playerCharacterInfoList[i].battleSlotPosOrder = playerCharacterList[i].battleSlotPosOrder;
        }

        PlayerSavedDataSO.Instance.playerCharacterInfoList = playerCharacterInfoList;
        //TODO: 將角色CharacterStats儲存
    }

    /// <summary>
    /// 將存檔匯入角色CharacterStats
    /// </summary>
    private void SetSavedDataInCharacter()
    {
        playerCharacterInfoList = PlayerSavedDataSO.Instance.playerCharacterInfoList;
        //TODO: 將存檔匯入角色CharacterStats
    }

    public void SaveData()
    {
        if (isBattleWin)
        {
            PlayerSavedDataSO.Instance.playerCurrentChosenGameLevelNode.isFinished = true;
        }

        GetCharacterInfoInSavedData();

    }

    public void LoadData()
    {
        gameLevel = PlayerSavedDataSO.Instance.playerCurrentChosenGameLevelNode.gameLevel;
        enemyList = PlayerSavedDataSO.Instance.playerCurrentChosenGameLevelNode.enemyList;

        SetSavedDataInCharacter();

    }


}
