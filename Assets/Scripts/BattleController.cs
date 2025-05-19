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
    /// ��M�̵u�Z�����ĤH
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
    /// �ͦ��԰���m
    /// </summary>
    private void GenerateBattleSlotOnBattlefield()
    {
        // �ͦ����a�԰���m
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

        // �ͦ��ĤH�԰���m
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
    /// �ͦ�����
    /// </summary>
    private void GenerateCharacter()
    {
        // �ͦ��ڤ訤��
        for (int i = 0; i < playerCharacterInfoList.Count; i++)
        {
            InstantiatePlayerCharGameObject(playerCharacterInfoList[i], TagManager.Instance.playerCharTag);
        }

        // �ͦ��Ĥ訤��
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
    /// �ͦ����a����
    /// </summary>
    /// <param name="charInfo"></param>
    /// <param name="tag"></param>
    public void InstantiatePlayerCharGameObject(CharacterInfo charInfo, string tag)
    {
        // �ͦ�����GameObject
        var charDetails = GameDataManager.Instance.characterDataListSO.GetCharacterDetails(charInfo.characterName);
        GameObject charGO = Instantiate(charDetails.prefab);
        charGO.tag = tag;
        var character = charGO.GetComponent<Character>();
        character.battleSlotPosOrder = charInfo.battleSlotPosOrder;
        playerCharacterList.Add(character);

        // ��������x�s��Ʃ�m�԰���m  
        if (charGO.CompareTag(TagManager.Instance.playerCharTag))
        {
            // ����n��m���԰���m
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
    /// �ͦ��Ĥ訤��
    /// </summary>
    /// <param name="characterName"></param>
    /// <param name="tag"></param>
    public void InstantiateEnemyCharGameObject(CharacterName characterName, string tag)
    {
        // �ͦ�����GameObject
        var charDetails = GameDataManager.Instance.characterDataListSO.GetCharacterDetails(characterName);
        GameObject charGO = Instantiate(charDetails.prefab);
        charGO.tag = tag;
        var character = charGO.GetComponent<Character>();

        //FIXME: �ĤH���Ӥ���?? ��m�԰���m
        // �w�]��m���ӵ۾԰���m���ǩ�   
        if (charGO.CompareTag(TagManager.Instance.enemyCharTag))
        {
            // ����n��m���԰���m
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

        // �԰���������: �ĤH�Ѿl = 0
        if(remainingEnemyCount == 0)
        {
            // �ӧQ
            isBattleWin = true;

            // �԰����� �ƥ�
            BattleEndEventSO.Instance.RaiseEvent();

        }
        //TODO:  �԰���������: �ڤ�Ѿl = 0

    }


    #endregion

    /// <summary>
    /// �N����CharacterStats�x�s
    /// </summary>
    private void GetCharacterInfoInSavedData()
    {
        for (int i = 0; i < playerCharacterInfoList.Count; i++)
        {
            playerCharacterInfoList[i].battleSlotPosOrder = playerCharacterList[i].battleSlotPosOrder;
        }

        PlayerSavedDataSO.Instance.playerCharacterInfoList = playerCharacterInfoList;
        //TODO: �N����CharacterStats�x�s
    }

    /// <summary>
    /// �N�s�ɶפJ����CharacterStats
    /// </summary>
    private void SetSavedDataInCharacter()
    {
        playerCharacterInfoList = PlayerSavedDataSO.Instance.playerCharacterInfoList;
        //TODO: �N�s�ɶפJ����CharacterStats
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
