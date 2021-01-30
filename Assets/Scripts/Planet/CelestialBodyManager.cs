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

    public CelestialBody CreateCelestialBody(CelestialBody.Type type)
    {
        switch (type)
        {
            case CelestialBody.Type.PLANET:
            {
                var planetPrefab = GameOptions.PlanetPrefabs[
                    Random.Range(0, GameOptions.PlanetPrefabs.Length)];
                var cb_go = GameObject.Instantiate(planetPrefab, Vector3.zero, Quaternion.identity);
                var cb = cb_go.AddComponent<Planet>();
                cb_go.AddComponent<CircleCollider2D>();
                cb.go = cb_go;
                return cb;
            }
            case CelestialBody.Type.STAR:
            {
                var starPrefab = GameOptions.StarPrefabs[
                    Random.Range(0, GameOptions.StarPrefabs.Length)];
                var cb_go = GameObject.Instantiate(starPrefab, Vector3.zero, Quaternion.identity);
                var cb = cb_go.AddComponent<Star>();
                cb_go.AddComponent<CircleCollider2D>();
                cb.go = cb_go;
                return cb;
            }
            default:
                L.og(L.Contexts.PLANET_MANAGER, "Invalid celestial body type");
                return null;
        }
    }
}
