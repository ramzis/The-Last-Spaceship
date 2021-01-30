using System;
using System.Collections;
using System.Collections.Generic;
using Logging;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CelestialBodyManager))]
public class GameManager : MonoBehaviour
{
    public GameOptions GameOptions;
    public CelestialBodyManager CelestialBodyManager;

    private void OnValidate()
    {
        Debug.Assert(GameOptions != null, "Missing game options!");
        Debug.Assert(CelestialBodyManager != null, "Missing celestial body manager!");
    }

    void Start()
    {
        SetLoggingOptions();
        L.og(L.Contexts.GAME_MANAGER, "Start()");
        Random.InitState(DateTime.Now.Millisecond);
        StartCoroutine(GameLoop());
    }

    void Update()
    {
        
    }

    IEnumerator GameLoop()
    {
        L.og(L.Contexts.GAME_MANAGER, "Starting GameLoop()");

        var planet1 = (Planet)CelestialBodyManager.CreateCelestialBody(CelestialBody.Type.PLANET);
        // var star1 = (Star)CelestialBodyManager.CreateCelestialBody(CelestialBody.Type.STAR);

        yield return null;
    }

    void SetLoggingOptions()
    {
        // Set Logging options
        L.SetOptions(
            GameOptions.doLogging,
            GameOptions.overrideEnableAllContexts,
            GameOptions.loggingContexts
        );
        L.og(L.Contexts.GAME_MANAGER, "Set logging options.");
    }
}
