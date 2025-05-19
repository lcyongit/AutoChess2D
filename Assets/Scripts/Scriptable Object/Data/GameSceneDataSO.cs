using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "GameSceneDataSO", menuName = "Scriptable Objects/Data/GameSceneDataSO")]
public class GameSceneDataSO : ScriptableObject
{
    public AssetReference sceneReference;
}
