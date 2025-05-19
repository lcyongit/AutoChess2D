using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Menu Info")]
    public GameObject mainMenuPanel;
    public Button continueGameBtn;

    [Header("Scene To Load")]
    public GameSceneDataSO gameLevelSelectScene;


    [Header("Game State")]
    private static bool isInitialized;

    private void OnEnable()
    {
        isInitialized = true;

        BeforeSceneLoadedEventSO.Instance.OnEventRaised += OnBeforeSceneLoadedEvent;
        AfterSceneLoadedEventSO.Instance.RegisterListener(OnAfterSceneLoadedEvent);

    }

    private void OnDisable()
    {
        BeforeSceneLoadedEventSO.Instance.OnEventRaised -= OnBeforeSceneLoadedEvent;
        AfterSceneLoadedEventSO.Instance.UnregisterListener(OnAfterSceneLoadedEvent);

        isInitialized = false;

    }

    public static bool IsInitialized()
    {
        return isInitialized;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        // 存檔如果存在 繼續遊戲button才啟用
        String savedDataPath = Application.persistentDataPath + "/Save/SaveData.sav";
        if (!File.Exists(savedDataPath))
            continueGameBtn.interactable = false;

    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    #region Events

    /// <summary>
    /// 開始新遊戲 Button事件
    /// </summary>
    public void OnClickStartNewGameBtn()
    {
        Debug.Log("--- 開始新遊戲 ---");

        GameDataManager.Instance.StartNewGame();

        SceneLoadManager.Instance.StartLoadScene(gameLevelSelectScene);

    }

    /// <summary>
    /// 繼續遊戲 Button事件
    /// </summary>
    public void OnClickContinueGameBtn()
    {
        Debug.Log("--- 繼續遊戲 ---");

        GameDataManager.Instance.LoadDataFromDisk();

        SceneLoadManager.Instance.StartLoadScene(gameLevelSelectScene);

    }

    /// <summary>
    /// 結束遊戲 Button事件
    /// </summary>
    public void OnClickQuitGameBtn()
    {
        Debug.Log("--- 結束遊戲 ---");

        QuitGame();

    }

    private void OnBeforeSceneLoadedEvent()
    {
        OnClosePanel();
    }

    private void OnAfterSceneLoadedEvent()
    {
        if (GameManager.Instance.gameState == GameState.MainMenu)
            OnOpenPanel();

    }


    /// <summary>
    /// 開啟Menu Panel
    /// </summary>
    public void OnOpenPanel()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);

    }

    /// <summary>
    /// 關閉Menu Panel
    /// </summary>
    public void OnClosePanel()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

    }


    #endregion

}
