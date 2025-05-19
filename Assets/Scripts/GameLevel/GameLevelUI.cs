using System;
using UnityEngine;
using UnityEngine.UI;

public class GameLevelUI : MonoBehaviour
{
    [Header("GameLevel Node Info")]
    public GameLevelNode currentChosenGameLevelNode;
    private GameObject currentShowPanel;
    private bool isShowPanel = false;

    [Header("GameLevel Panel")]
    public GameObject battlePanel;

    [Header("BattlePanel Components")]
    public Transform enemyFormation;
    public GameObject characterImage;


    private void OnEnable()
    {
        BeforeSceneLoadedEventSO.Instance.OnEventRaised += OnBeforeSceneLoadedEvent;
        ClickGameLevelNodeEventSO.Instance.OnEventRaisedWithGameLevelNode += OnClickGameLevelNodeEvent;
    }

    private void OnDisable()
    {
        BeforeSceneLoadedEventSO.Instance.OnEventRaised -= OnBeforeSceneLoadedEvent;
        ClickGameLevelNodeEventSO.Instance.OnEventRaisedWithGameLevelNode -= OnClickGameLevelNodeEvent;

    }

    #region Events

    private void OnBeforeSceneLoadedEvent()
    {
        OnClosePanel();

    }

    private void OnClickGameLevelNodeEvent(GameLevelNode gameLevelNode)
    {
        if (isShowPanel)
            return;

        // 顯示對應關卡UI
        currentChosenGameLevelNode = gameLevelNode;

        switch (gameLevelNode.gameLevel)
        {
            case GameLevel.Battle:

                isShowPanel = true;
                currentShowPanel = battlePanel;
                currentShowPanel.SetActive(true);

                // 清空enemyFormation prefab
                foreach (Transform characterImage in enemyFormation)
                {
                    Destroy(characterImage.gameObject);
                }

                // 關卡資訊放入敵人資訊
                for (int i = 0; i < currentChosenGameLevelNode.enemyList.Count; i++)
                {
                    GameObject characterImageGO = Instantiate(characterImage, enemyFormation);
                    Image image = characterImageGO.GetComponent<Image>();
                    Sprite sprite = GameDataManager.Instance.characterDataListSO.GetCharacterDetails(currentChosenGameLevelNode.enemyList[i]).sprite;
                    image.sprite = sprite;
                    characterImageGO.GetComponent<ImageFitSize>().SetImageSize();

                }

                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 戰鬥關卡Panel => 開始戰鬥 Button事件
    /// </summary>
    public void OnClickBattleBtn()
    {
        // 轉換場景 => 戰鬥場景 or 不轉換場景
        var gameLevelDetails = GameDataManager.Instance.gameLevelDataListSO.GetGameLevelDetails(currentChosenGameLevelNode.gameLevel);

        if (gameLevelDetails.isNeedLoadScene)
        {
            var sceneToLoad = gameLevelDetails.gameScene;

            SceneLoadManager.Instance.StartLoadScene(sceneToLoad);

        }

    }

    /// <summary>
    /// 關閉關卡Panel
    /// </summary>
    public void OnClosePanel()
    {
        if (currentShowPanel != null)
        {
            isShowPanel = false;
            currentShowPanel.SetActive(false);
        }

    }

    #endregion

    


}
