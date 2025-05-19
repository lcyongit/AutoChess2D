using System;

public enum GameState
{
    Empty, 
    Gameplay, 
    Pause, 
    MainMenu, 
    GameLevelSelect,
    Battlefield,
    IsFighting, 

}

[Serializable]
public enum CharacterName
{
    Empty = 0,
    Knight = 1,
    Lancer = 2,
    Archer = 3,

}

public enum GameLevel
{
    Empty = 0,
    Battle = 1,
    GameEvent = 2,

}
