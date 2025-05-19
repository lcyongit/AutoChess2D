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
                    Debug.LogError("�L�k�b Resources ����� ����SO");
                }
            }
            return _instance;
        }
    }

    // *** => �䴩���a�s��

    // ���a�C���i��

    [Header("Player Game Progress Info")]
    // *** ���a�ثe�Ҧb�����d�`�I��m (col, row) = (0, 0) => �즸�i�J�a��
    // �C���i�J��l�Ƭ�(-1, -1)�A��ܶ}�l�s�C���N�ܬ�(0, 0)�άOŪ���C���i��
    public GameLeveNodeSavedData playerCurrentChosenGameLevelNode;

    // *** �Ҧ��`�I��T
    public List<GameLeveNodeSavedData> gameLeveNodeSavedDataList = new();

    // *** ���a�����T
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
