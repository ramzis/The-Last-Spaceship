using System;
using System.Collections;
using System.Collections.Generic;
using Logging;
using UnityEngine;
using Random = UnityEngine.Random;

public class CelestialBodyManager : MonoBehaviour
{
    public GameOptions GameOptions;
    [SerializeField]
    CelestialBody[] CelestialBodies;

    private void OnValidate()
    {
        Debug.Assert(GameOptions.PlanetPrefabs.Length > 0, "GameOptions.PlanetPrefabs.Length should be > 0");
        Debug.Assert(GameOptions.StarPrefabs.Length > 0, "GameOptions.SunPrefabs.Length should be > 0");
        Debug.Assert(GameOptions != null, "Missing game options!");
    }
    /*
    GameObject Player;

    [Range(1,20)]
    public int planetsCount = 5;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        Planets = new GameObject[planetsCount];
        
        for(int i =0;i<planetsCount;i++)
        {
            Planets[i] = (GameObject)Instantiate(PlanetPrefabs[(int)Random.Range(0,2)],new Vector3(Random.Range(-50,50f),Random.Range(-50f,50f),0), Quaternion.identity);
        }
        InvokeRepeating("GetClosest",1f,1f);
    }

    void GetClosest()
    {
        int idOfClosest =0;
        float distance =2000000000;
        for(int i =0;i<planetsCount;i++)
        {
            distance = (Player.transform.position -Planets[idOfClosest].transform.position).sqrMagnitude;
            if(distance > (Player.transform.position - Planets[i].transform.position).sqrMagnitude)
            {
                idOfClosest = i;
            }
        }
        
        for(int i =0;i<planetsCount;i++)
        {
            Planets[i].SetActive(false);
        }
        Planets[idOfClosest].SetActive(true);
    }*/

    private int planetCount = 0;
    private int starCount = 0;
    public CelestialBody CreateCelestialBody(CelestialBody.Type type, Vector2 pos, bool active)
    {
        switch (type)
        {
            case CelestialBody.Type.PLANET:
            {
                var planetPrefab = GameOptions.PlanetPrefabs[
                    Random.Range(0, GameOptions.PlanetPrefabs.Length)];
                var cb_go = GameObject.Instantiate(planetPrefab, Vector3.zero, Quaternion.identity);
                var cb = cb_go.AddComponent<Planet>();
                cb.responses = new List<(string, CelestialBody.EventID)>();
                if (Random.Range(0f, 100f) > 50f)
                {
                    cb.responses.Add(("You have found a new home!", new CelestialBody.EventID("home")));
                }
                cb.Name = "Planet_"+planetCount;
                planetCount++;
                cb.go = cb_go;
                var trigger_go = new GameObject("Trigger");
                trigger_go.layer = LayerMask.NameToLayer("Ignore Raycast");
                var trigger = trigger_go.AddComponent<CircleCollider2D>();
                trigger.isTrigger = true;
                trigger.radius = GameOptions.CelestialBody_VisibilityDistance;
                cb_go.transform.SetParent(trigger_go.transform);
                trigger_go.AddComponent<HandleTrigger>();
                trigger_go.transform.position = pos;
                cb_go.SetActive(active);
                return cb;
            }
            case CelestialBody.Type.BEGINNER_PLANET:
            {
                var planetPrefab = GameOptions.PlanetPrefabs[
                    Random.Range(0, GameOptions.PlanetPrefabs.Length)];
                var cb_go = GameObject.Instantiate(planetPrefab, Vector3.zero, Quaternion.identity);
                var cb = cb_go.AddComponent<Planet>();
                cb.responses = new List<(string, CelestialBody.EventID)>()
                {
                    ("There is no life on this planet. However, you’ve picked up a signal from an abandoned transmitter. It says a sector number G21. Sounds familiar.", new CelestialBody.EventID("interact beginner planet")),
                    ("There is no life on this planet. However, you’ve picked up a signal from an abandoned transmitter. It says a sector number G21. Sounds familiar.", new CelestialBody.EventID("message")),
                    ("Nothing to see here...", new CelestialBody.EventID("message"))
                };
                cb.Name = "Planet_"+planetCount;
                planetCount++;
                cb.go = cb_go;
                var trigger_go = new GameObject("Trigger");
                trigger_go.layer = LayerMask.NameToLayer("Ignore Raycast");
                var trigger = trigger_go.AddComponent<CircleCollider2D>();
                trigger.isTrigger = true;
                trigger.radius = GameOptions.CelestialBody_VisibilityDistance;
                cb_go.transform.SetParent(trigger_go.transform);
                trigger_go.AddComponent<HandleTrigger>();
                trigger_go.transform.position = pos;
                cb_go.SetActive(active);
                return cb;
            }
            case CelestialBody.Type.STAR:
            {
                var starPrefab = GameOptions.StarPrefabs[
                    Random.Range(0, GameOptions.StarPrefabs.Length)];
                var cb_go = GameObject.Instantiate(starPrefab, Vector3.zero, Quaternion.identity);
                var cb = cb_go.AddComponent<Star>();
                cb.Name = "Star_"+starCount;
                starCount++;
                cb.go = cb_go;
                var trigger_go = new GameObject("Trigger");
                trigger_go.layer = LayerMask.NameToLayer("Ignore Raycast");
                var trigger = trigger_go.AddComponent<CircleCollider2D>();
                trigger.isTrigger = true;
                trigger.radius = GameOptions.CelestialBody_VisibilityDistance;
                cb_go.transform.SetParent(trigger_go.transform);
                trigger_go.AddComponent<HandleTrigger>();
                trigger_go.transform.position = pos;
                cb_go.SetActive(active);
                return cb;
            }
            default:
                L.og(L.Contexts.PLANET_MANAGER, "Invalid celestial body type");
                return null;
        }
    }
}
