using System;
using UnityEngine;

[Serializable]
public class GameLevelInfo
{
    public GameLevel gameLevel;
    public bool isLevelClear;
    public GameLevelNode from;
    public GameLevelNode to;

}
