#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class PlayerMenuUI : MonoBehaviour
{
    [Header("Menu Info")]
    public GameObject pauseMenuPanel;

    [Header("Scene To Load")]
    public GameSceneDataSO mainMenuSceneSO;

    [Header("State")]
    private bool isOpenPauseMenuPanel = false;

    private void OnEnable()
    {
        BeforeSceneLoadedEventSO.Instance.OnEventRaised += OnBeforeSceneLoadedEvent;
        
    }

    private void OnDisable()
    {
        BeforeSceneLoadedEventSO.Instance.OnEventRaised -= OnBeforeSceneLoadedEvent;

    }

    void Start()
    {

    }

    void Update()
    {
        SwitchPauseMenu();
    }

    public void SwitchPauseMenu()
    {
        // ESC暫停 只能在 關卡選擇 或 戰鬥場景 時執行
        if (GameManager.Instance.gameState == GameState.GameLevelSelect || GameManager.Instance.gameState == GameState.Battlefield)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (isOpenPauseMenuPanel) 
                    OnClosePanel();
                else 
                    OnOpenPanel();

                isOpenPauseMenuPanel = !isOpenPauseMenuPanel;

                Time.timeScale = isOpenPauseMenuPanel ? 0f : 1f;

            }
        }
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
    /// 繼續遊戲 Button事件
    /// </summary>
    public void OnClickResumeGaemBtn()
    {
        Debug.Log("--- 繼續遊戲 ---");

        Time.timeScale = 1f;
        isOpenPauseMenuPanel = false;
        OnClosePanel();

    }

    /// <summary>
    /// 回到主選單 Button事件
    /// </summary>
    public void OnClickMainMenuBtn()
    {
        Debug.Log("--- 回到主選單 ---");

        Time.timeScale = 1f;
        isOpenPauseMenuPanel = false;
        OnClosePanel();

        SceneLoadManager.Instance.StartLoadScene(mainMenuSceneSO);

    }

    /// <summary>
    /// 結束遊戲 Button事件
    /// </summary>
    public void OnClickQuitGameBtn()
    {
        Debug.Log("--- 結束遊戲 ---");

        Time.timeScale = 1f;
        QuitGame();

    }

    public void OnOpenPanel()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);

    }

    public void OnClosePanel()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

    }

    private void OnBeforeSceneLoadedEvent()
    {
        isOpenPauseMenuPanel = false;
        OnClosePanel();

    }




    #endregion
}
