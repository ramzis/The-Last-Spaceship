using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Game Options")]
public class GameOptions : ScriptableObject
{
    // Logging
    public bool doLogging;
    public bool overrideEnableAllContexts;
    public List<string> loggingContexts;

    // Ship Controller
    public float Ship_Speed;
    public float Ship_RotationSpeed;
    public float Ship_InteractionRadius;

    // Map
    public float Map_SizeX;
    public float Map_SizeY;
    public float Map_SectorCountX;
    public float Map_SectorCountY;

    // Celestial Bodies
    public int CelestialBody_VisibilityDistance;

    // Ship Cannon
    public float ShipCannon_Distance;

    // Planet Manager
    public GameObject[] PlanetPrefabs;
    public GameObject[] StarPrefabs;
}
