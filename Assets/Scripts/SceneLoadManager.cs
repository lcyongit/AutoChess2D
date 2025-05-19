using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    [Header("Scene Info")]
    public GameSceneDataSO startScene;
    public GameSceneDataSO currentScene;

    [Header("Game Scene SO")]
    public GameSceneDataSO menuScene;
    public GameSceneDataSO gameLevelSelectScene;
    public GameSceneDataSO battlefieldScene;

    [Header("Components")]
    public CanvasGroup fadeScreenCanvasGroup;


    [Header("Fade Screen")]
    public float fadeScreenDuration;
    private bool isFade;

    [Header("State")]
    private bool isSceneLoading;

    protected override void Awake()
    {
        base.Awake();
        currentScene = null;

    }

    private void Start()
    {
        StartLoadScene(startScene);

    }


    /// <summary>
    /// 開始載入場景
    /// </summary>
    /// <param name="sceneToLoad"></param>
    public void StartLoadScene(GameSceneDataSO sceneToLoad)
    {
        if (!isFade && !isSceneLoading)
            StartCoroutine(LoadScene(sceneToLoad));

    }

    /// <summary>
    /// 載入場景 完整過程
    /// </summary>
    /// <param name="sceneToLoad"></param>
    /// <returns></returns>
    public IEnumerator LoadScene(GameSceneDataSO sceneToLoad)
    {
        // 淡出
        yield return FadeScreen(1f);

        isSceneLoading = true;

        //yield return new WaitForSeconds(1f);

        // 第一次加載場景不用卸載
        if (currentScene != null)
        {
            //Debug.Log("--- 卸載場景前 ---");
            // 場景卸載前 事件
            yield return SceneBeforeUnload();

            // 場景卸載
            yield return currentScene.sceneReference.UnLoadScene();

        }

        //Debug.Log("--- 加載場景前 ---");
        // 加載場景前 事件
        SceneBeforeLoad();

        // 加載場景
        var loadOperation = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        yield return loadOperation;

        string loadedSceneName = "";

        if (loadOperation.Status == AsyncOperationStatus.Succeeded)
        {
            // 直接取得載入的場景
            Scene loadedScene = loadOperation.Result.Scene;
            loadedSceneName = loadedScene.name;

            if (loadedScene.IsValid())
            {
                SceneManager.SetActiveScene(loadedScene);
                Debug.Log($"加載場景: {loadedScene.name}");
            }
            else
            {
                Debug.LogError("Loaded scene is not valid.");
            }
        }
        else
        {
            Debug.LogError("場景加載失敗.");
        }

        currentScene = sceneToLoad;

        //Debug.Log("--- 加載場景後 ---");
        // 加載場景後 事件
        yield return SceneLoadedComplete(loadedSceneName);

        //yield return new WaitForSeconds(1f);

        isSceneLoading = false;

        // 淡入
        yield return FadeScreen(0f);


    }

    /// <summary>
    /// 場景卸載前 事件
    /// </summary>
    private IEnumerator SceneBeforeUnload()
    {
        // 儲存
        GameDataManager.Instance.SaveData();
        GameDataManager.Instance.SaveDataInDisk();

        yield return null;

    }

    /// <summary>
    /// 場景加載前 事件
    /// </summary>
    private void SceneBeforeLoad()
    {
        BeforeSceneLoadedEventSO.Instance.RaiseEvent();

    }

    /// <summary>
    /// 場景加載完成後 事件
    /// </summary>
    private IEnumerator SceneLoadedComplete(string loadedSceneName)
    {
        Debug.Log("場景加載完成");

        // 讀取
        GameDataManager.Instance.LoadData();

        // 確保 場景已加載並初始化，不然build出來會讀取不到
        switch (loadedSceneName)
        {
            case "Menu":
                yield return new WaitUntil(() => MainMenuUI.IsInitialized());
                GameManager.Instance.gameState = GameState.MainMenu;
                break;
            case "GameLevelSelect":
                yield return new WaitUntil(() => GameLevelController.IsInitialized());
                GameManager.Instance.gameState = GameState.GameLevelSelect;
                break;
            case "Battlefield":
                yield return new WaitUntil(() => BattleController.IsInitialized());
                GameManager.Instance.gameState = GameState.Battlefield;
                break;
            default:
                break;
        }
        
        AfterSceneLoadedEventSO.Instance.RaiseEvent();

        // 儲存
        GameDataManager.Instance.SaveData();

        // 進到menu不執行本地存檔
        if (currentScene != menuScene)
            GameDataManager.Instance.SaveDataInDisk();

        yield return null;
    }

    /// <summary>
    /// 淡入淡出，1=>淡出、0=>淡入
    /// </summary>
    /// <param name="setAlpha"></param>
    /// <returns></returns>
    private IEnumerator FadeScreen(float setAlpha)
    {
        isFade = true;

        fadeScreenCanvasGroup.blocksRaycasts = true;

        fadeScreenDuration = Mathf.Max(fadeScreenDuration, 0.5f);
        float fadeSpeed = 1f / fadeScreenDuration;

        while (!Mathf.Approximately(fadeScreenCanvasGroup.alpha, setAlpha))
        {
            fadeScreenCanvasGroup.alpha = Mathf.MoveTowards(fadeScreenCanvasGroup.alpha, setAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        fadeScreenCanvasGroup.blocksRaycasts = false;

        isFade = false;

    }


}
