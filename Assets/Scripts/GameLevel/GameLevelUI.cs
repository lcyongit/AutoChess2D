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

        // ��ܹ������dUI
        currentChosenGameLevelNode = gameLevelNode;

        switch (gameLevelNode.gameLevel)
        {
            case GameLevel.Battle:

                isShowPanel = true;
                currentShowPanel = battlePanel;
                currentShowPanel.SetActive(true);

                // �M��enemyFormation prefab
                foreach (Transform characterImage in enemyFormation)
                {
                    Destroy(characterImage.gameObject);
                }

                // ���d��T��J�ĤH��T
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
    /// �԰����dPanel => �}�l�԰� Button�ƥ�
    /// </summary>
    public void OnClickBattleBtn()
    {
        // �ഫ���� => �԰����� or ���ഫ����
        var gameLevelDetails = GameDataManager.Instance.gameLevelDataListSO.GetGameLevelDetails(currentChosenGameLevelNode.gameLevel);

        if (gameLevelDetails.isNeedLoadScene)
        {
            var sceneToLoad = gameLevelDetails.gameScene;

            SceneLoadManager.Instance.StartLoadScene(sceneToLoad);

        }

    }

    /// <summary>
    /// �������dPanel
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
