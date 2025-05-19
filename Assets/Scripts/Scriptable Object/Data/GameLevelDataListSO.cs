using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameLevelDataListSO", menuName = "Scriptable Objects/Data/GameLevelDataListSO")]
public class GameLevelDataListSO : ScriptableObject
{
    public List<GameLevelDetails> gameLevelDetailsList = new List<GameLevelDetails>();

    public GameLevelDetails GetGameLevelDetails(GameLevel gameLevel)
    {
        return gameLevelDetailsList.Find(x => x.gameLevel == gameLevel);

    }

    [Serializable]
    public class GameLevelDetails
    {
        public GameLevel gameLevel;

        public GameSceneDataSO gameScene;

        public bool isNeedLoadScene;
        public bool isContainBattle;

        public int enemyCount;

        //TODO: 加入會出現的敵人種類

    }


}
