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
    /// 清除characterUIBarCanvas底下的UI Bar
    /// </summary>
    private void ClearCharacterUIBar()
    {
        // 從尾開始刪除才不會報錯
        for (int i = characterUIBarCanvas.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(characterUIBarCanvas.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 關閉UI
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
    /// 戰鬥準備 事件
    /// </summary>
    private void OnBattlePreparationEvent()
    {
        currentShowPanel = battlePreparationPanel;
        currentShowPanel.SetActive(true);
    }

    /// <summary>
    /// 戰鬥結束 事件
    /// </summary>
    private void OnBattleEndEvent()
    {
        currentShowPanel = battleVictoryPanel;
        currentShowPanel.SetActive(true);

    }

    /// <summary>
    /// 戰鬥開始 按鈕 事件
    /// </summary>
    public void OnClickBattleStartBtn()
    {
        currentShowPanel.SetActive(false);
        currentShowPanel = null;

        BattleStartEventSO.Instance.RaiseEvent();

    }

    /// <summary>
    /// 戰鬥結束 按鈕 事件
    /// </summary>
    public void OnClickConfirmBtn()
    {
        // 轉換場景 => 關卡選擇
        currentShowPanel.SetActive(false);
        currentShowPanel = null;

        var sceneToLoad = SceneLoadManager.Instance.gameLevelSelectScene;

        SceneLoadManager.Instance.StartLoadScene(sceneToLoad);

    }



    #endregion


}
