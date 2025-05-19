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
    /// �}�l���J����
    /// </summary>
    /// <param name="sceneToLoad"></param>
    public void StartLoadScene(GameSceneDataSO sceneToLoad)
    {
        if (!isFade && !isSceneLoading)
            StartCoroutine(LoadScene(sceneToLoad));

    }

    /// <summary>
    /// ���J���� ����L�{
    /// </summary>
    /// <param name="sceneToLoad"></param>
    /// <returns></returns>
    public IEnumerator LoadScene(GameSceneDataSO sceneToLoad)
    {
        // �H�X
        yield return FadeScreen(1f);

        isSceneLoading = true;

        //yield return new WaitForSeconds(1f);

        // �Ĥ@���[���������Ψ���
        if (currentScene != null)
        {
            //Debug.Log("--- ���������e ---");
            // ���������e �ƥ�
            yield return SceneBeforeUnload();

            // ��������
            yield return currentScene.sceneReference.UnLoadScene();

        }

        //Debug.Log("--- �[�������e ---");
        // �[�������e �ƥ�
        SceneBeforeLoad();

        // �[������
        var loadOperation = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        yield return loadOperation;

        string loadedSceneName = "";

        if (loadOperation.Status == AsyncOperationStatus.Succeeded)
        {
            // �������o���J������
            Scene loadedScene = loadOperation.Result.Scene;
            loadedSceneName = loadedScene.name;

            if (loadedScene.IsValid())
            {
                SceneManager.SetActiveScene(loadedScene);
                Debug.Log($"�[������: {loadedScene.name}");
            }
            else
            {
                Debug.LogError("Loaded scene is not valid.");
            }
        }
        else
        {
            Debug.LogError("�����[������.");
        }

        currentScene = sceneToLoad;

        //Debug.Log("--- �[�������� ---");
        // �[�������� �ƥ�
        yield return SceneLoadedComplete(loadedSceneName);

        //yield return new WaitForSeconds(1f);

        isSceneLoading = false;

        // �H�J
        yield return FadeScreen(0f);


    }

    /// <summary>
    /// ���������e �ƥ�
    /// </summary>
    private IEnumerator SceneBeforeUnload()
    {
        // �x�s
        GameDataManager.Instance.SaveData();
        GameDataManager.Instance.SaveDataInDisk();

        yield return null;

    }

    /// <summary>
    /// �����[���e �ƥ�
    /// </summary>
    private void SceneBeforeLoad()
    {
        BeforeSceneLoadedEventSO.Instance.RaiseEvent();

    }

    /// <summary>
    /// �����[�������� �ƥ�
    /// </summary>
    private IEnumerator SceneLoadedComplete(string loadedSceneName)
    {
        Debug.Log("�����[������");

        // Ū��
        GameDataManager.Instance.LoadData();

        // �T�O �����w�[���ê�l�ơA���Mbuild�X�ӷ|Ū������
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

        // �x�s
        GameDataManager.Instance.SaveData();

        // �i��menu�����楻�a�s��
        if (currentScene != menuScene)
            GameDataManager.Instance.SaveDataInDisk();

        yield return null;
    }

    /// <summary>
    /// �H�J�H�X�A1=>�H�X�B0=>�H�J
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
