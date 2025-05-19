using UnityEngine;

public interface ISavable
{
    public void RegisterISavable() => GameDataManager.Instance.RegisterISavable(this);
    public void UnRegisterISavable() => GameDataManager.Instance.UnRegisterISavable(this);
    public void SaveData();
    public void LoadData();


}
