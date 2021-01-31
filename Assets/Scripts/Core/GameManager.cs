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
    public TextboxManager TextboxManager;

    private void OnValidate()
    {
        Debug.Assert(GameOptions != null, "Missing game options!");
        Debug.Assert(CelestialBodyManager != null, "Missing celestial body manager!");
        Debug.Assert(SectorUIManager != null, "Missing sector ui manager!");
        Debug.Assert(SectorResolver != null, "Missing sector resolver!");
        Debug.Assert(TextboxManager != null, "Missing textbox manager!");
    }

    void Start()
    {
        SetLoggingOptions();
        L.og(L.Contexts.GAME_MANAGER, "Start()");
        Random.InitState(DateTime.Now.Millisecond);
        StartCoroutine(GameLoop());
    }

    public void HandleEvent((string msg, CelestialBody.EventID cbEvent) e)
    {
        switch (e.cbEvent.ID)
        {
            case "message":
            {
                TextboxManager.TextQueue.Enqueue(e.msg);
                break;
            }
            case "first star":
            {
                TextboxManager.TextQueue.Enqueue("A star is a great source of fuel for your ship. Use the mining beam to collect it’s energy.");
                break;
            }
            case "first planet":
            {
                TextboxManager.TextQueue.Enqueue("You’ve come upon a planet. Press scan when in range to gather information.");
                break;
            }
            case "interact beginner planet":
            {
                gameState = GameState.STATE_WAITING_FOR_STAR_INTERACTION;
                break;
            }
            case "destroying first star":
            {
                gameState = GameState.STATE_DESTROYING_STARS;
                break;
            }
            case "full fuel":
            {
                if (gameState != GameState.STATE_DESTROYING_STARS) break;
                gameState = GameState.STATE_WAITING_FOR_FULL_FUEL;
                break;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    enum GameState
    {
        STATE_WAITING_FOR_PLANET_INTERACTION,
        STATE_WAITING_FOR_STAR_INTERACTION,
        STATE_DESTROYING_STARS,
        STATE_WAITING_FOR_FULL_FUEL,
        STATE_WAITING_FOR_SETTLE
    }

    [SerializeField]
    private GameState gameState;
    IEnumerator GameLoop()
    {
        L.og(L.Contexts.GAME_MANAGER, "Starting GameLoop()");

        SectorUIManager.ToggleSectorView(true);
        SectorResolver.OnNewSector += SectorUIManager.UpdateSector;

        SpawnExplodingHomePlanet();

        TextboxManager.TextQueue.Enqueue("Your home planet has been destroyed. Luckily, you managed to escape in your tiny spaceship.");
        TextboxManager.TextQueue.Enqueue("Since you were young you’d heard of a place with many habitable planets. You know it is in sector G21 but you don’t know where that is, or even where you are. Now, lost and confused, you are wandering the stars to find a new home.");

        SpawnArrayOfBeginnerPlanets();
        gameState = GameState.STATE_WAITING_FOR_PLANET_INTERACTION;

        // Inspecting changes to STATE_WAITING_FOR_STAR_INTERACTION
        yield return new WaitWhile(() => gameState != GameState.STATE_WAITING_FOR_STAR_INTERACTION);
        SpawnArrayOfBeginnerStars();

        // Mining changes to STATE_DESTROYING_STARS
        yield return new WaitWhile(() => gameState != GameState.STATE_DESTROYING_STARS);

        TextboxManager.TextQueue.Enqueue("As you mine a star, your fuel level will rise.");
        TextboxManager.TextQueue.Enqueue("Continue mining the star until your fuel level is full.");

        // Full fuel changes to STATE_WAITING_FOR_FULL_FUEL
        yield return new WaitWhile(() => gameState != GameState.STATE_WAITING_FOR_FULL_FUEL);

        DestroyBeginnerStars();
        TextboxManager.TextQueue.Enqueue("Your fuel tank is all filled up! Now that you have enough fuel in your ship, your radar navigation has turned on. The radnav system lets you know which sector you are in at all times. It will also help you discover more potential home planets. When fuel runs out, look for a star to mine or your radar will not have enough power to discover any new planets!");
        TextboxManager.TextQueue.Enqueue("Keep following your radar to explore more planets. When you find one that suits you, you can make it your new home.");

        yield return new WaitWhile(() => gameState != GameState.STATE_WAITING_FOR_SETTLE);

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

        yield return null;
    }

    void SpawnArrayOfBeginnerPlanets()
    {
        int sectorWidth = Mathf.FloorToInt(GameOptions.Map_SizeX / GameOptions.Map_SectorCountX);
        int sectorHeight = Mathf.FloorToInt(GameOptions.Map_SizeY / GameOptions.Map_SectorCountY);

        for (int y = 0; y < GameOptions.Map_SectorCountY; y++)
        {
            var yPos = (-GameOptions.Map_SizeY / 2) + ((y) * sectorHeight);
            for (int x = 0; x < GameOptions.Map_SectorCountX; x++)
            {
                var xPos = (-GameOptions.Map_SizeX / 2) + ((x) * sectorWidth);
                var planet1 = (Planet) CelestialBodyManager.CreateCelestialBody(CelestialBody.Type.BEGINNER_PLANET,
                    new Vector2(xPos, yPos), false);
            }
        }
    }

    void SpawnExplodingHomePlanet()
    {

    }

    void SpawnArrayOfBeginnerStars()
    {

    }

    void DestroyBeginnerStars()
    {

    }

    void DestroyBegginerPlanets()
    {

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
