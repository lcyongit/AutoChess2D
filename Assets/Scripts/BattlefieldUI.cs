using System;
using Unity.VisualScripting;
using UnityEngine;

public class BattlefieldUI : Singleton<BattlefieldUI>
{
    [Header("Battlefield Components")]
    public GameObject characterUIBarCanvas;

    [Header("GameLevel Panel")]
    public GameObject battlePreparationPanel;
    public GameObject battleVictoryPanel;

    [Header("Current Info")]
    private GameObject currentShowPanel;



    private void OnEnable()
    {
        BeforeSceneLoadedEventSO.Instance.OnEventRaised += OnBeforeSceneLoadedEvent;
        BattlePreparationEventSO.Instance.OnEventRaised += OnBattlePreparationEvent;
        BattleEndEventSO.Instance.OnEventRaised += OnBattleEndEvent;
    }

    private void OnDisable()
    {
        BeforeSceneLoadedEventSO.Instance.OnEventRaised -= OnBeforeSceneLoadedEvent;
        BattlePreparationEventSO.Instance.OnEventRaised -= OnBattlePreparationEvent;
        BattleEndEventSO.Instance.OnEventRaised -= OnBattleEndEvent;

    }

    /// <summary>
    /// �M��characterUIBarCanvas���U��UI Bar
    /// </summary>
    private void ClearCharacterUIBar()
    {
        // �q���}�l�R���~���|����
        for (int i = characterUIBarCanvas.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(characterUIBarCanvas.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// ����UI
    /// </summary>
    private void ClosePanel()
    {
        if (currentShowPanel != null)
            currentShowPanel.SetActive(false);

        currentShowPanel = null;

    }

    #region Events

    private void OnBeforeSceneLoadedEvent()
    { 
        ClosePanel();

        ClearCharacterUIBar();

    }

    /// <summary>
    /// �԰��ǳ� �ƥ�
    /// </summary>
    private void OnBattlePreparationEvent()
    {
        currentShowPanel = battlePreparationPanel;
        currentShowPanel.SetActive(true);
    }

    /// <summary>
    /// �԰����� �ƥ�
    /// </summary>
    private void OnBattleEndEvent()
    {
        currentShowPanel = battleVictoryPanel;
        currentShowPanel.SetActive(true);

    }

    /// <summary>
    /// �԰��}�l ���s �ƥ�
    /// </summary>
    public void OnClickBattleStartBtn()
    {
        currentShowPanel.SetActive(false);
        currentShowPanel = null;

        BattleStartEventSO.Instance.RaiseEvent();

    }

    /// <summary>
    /// �԰����� ���s �ƥ�
    /// </summary>
    public void OnClickConfirmBtn()
    {
        // �ഫ���� => ���d���
        currentShowPanel.SetActive(false);
        currentShowPanel = null;

        var sceneToLoad = SceneLoadManager.Instance.gameLevelSelectScene;

        SceneLoadManager.Instance.StartLoadScene(sceneToLoad);

    }



    #endregion


}
