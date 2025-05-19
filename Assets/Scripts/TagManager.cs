using UnityEngine;

public class TagManager : Singleton<TagManager>
{
    [Header("Tag Name")]
    [TagName] public string gameLevelSlotTag;
    [TagName] public string playerCharSlotTag;
    [TagName] public string enemyCharSlotTag;
    [TagName] public string playerCharTag;
    [TagName] public string enemyCharTag;

    protected override void Awake()
    {
        base.Awake();

    }
}
