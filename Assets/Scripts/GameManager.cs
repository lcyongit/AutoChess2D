using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("Game State")]
    public GameState gameState;


    private void OnEnable()
    {
    }
    private void OnDisable()
    {

    }

    protected override void Awake()
    {
        base.Awake();

    }

    private void Start()
    {
        

    }


    #region Events

    


    

    #endregion


}
