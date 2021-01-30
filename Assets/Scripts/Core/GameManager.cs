using System;
using System.Collections;
using System.Collections.Generic;
using Logging;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameOptions gameOptions;

    public Random random;

    private void Awake()
    {
        Debug.Assert(gameOptions != null, "Missing game options!");
    }

    void Start()
    {
        SetLoggingOptions();
        L.og(L.Contexts.GAME_MANAGER, "Start()");
        Random.InitState(DateTime.Now.Millisecond);
    }

    void Update()
    {
        
    }

    void SetLoggingOptions()
    {
        // Set Logging optionsx
        L.SetOptions(
            gameOptions.doLogging,
            gameOptions.overrideEnableAllContexts,
            gameOptions.loggingContexts
        );
        L.og(L.Contexts.GAME_MANAGER, "Set logging options.");
    }
}
