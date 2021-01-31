using System;
using System.Collections;
using System.Collections.Generic;
using Logging;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CelestialBodyManager))]
public class GameManager : MonoBehaviour
{
    public GameOptions GameOptions;
    public CelestialBodyManager CelestialBodyManager;
    public SectorUIManager SectorUIManager;
    public SectorResolver SectorResolver;

    private void OnValidate()
    {
        Debug.Assert(GameOptions != null, "Missing game options!");
        Debug.Assert(CelestialBodyManager != null, "Missing celestial body manager!");
        Debug.Assert(SectorUIManager != null, "Missing sector ui manager!");
        Debug.Assert(SectorResolver != null, "Missing sector resolver!");
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
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    IEnumerator GameLoop()
    {
        L.og(L.Contexts.GAME_MANAGER, "Starting GameLoop()");

        SectorUIManager.ToggleSectorView(true);
        SectorResolver.OnNewSector += SectorUIManager.UpdateSector;

        // var planet1 = (Planet)CelestialBodyManager.CreateCelestialBody(CelestialBody.Type.PLANET);

        int sectorWidth = Mathf.FloorToInt(GameOptions.Map_SizeX / GameOptions.Map_SectorCountX);
        int sectorHeight = Mathf.FloorToInt(GameOptions.Map_SizeY / GameOptions.Map_SectorCountY);

        for (int y = 0; y < GameOptions.Map_SectorCountY; y++)
        {
            var yPos = (-GameOptions.Map_SizeY / 2) + ((y) * sectorHeight);
            for (int x = 0; x < GameOptions.Map_SectorCountX; x++)
            {
                var xPos = (-GameOptions.Map_SizeX / 2) + ((x) * sectorWidth);
                var star1 = (Star) CelestialBodyManager.CreateCelestialBody(CelestialBody.Type.STAR,
                    new Vector2(xPos, yPos), false);
            }
        }

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
