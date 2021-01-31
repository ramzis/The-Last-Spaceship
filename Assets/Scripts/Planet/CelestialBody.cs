using System;
using System.Collections;
using System.Collections.Generic;
using Logging;
using UnityEngine;


public abstract class CelestialBody : MonoBehaviour
{
    public enum Type
    {
        PLANET,
        STAR
    }

    public struct EventID
    {
        public string ID;
    }

    public string Name;
    public Type BodyType;
    public Sprite[] Sprites;
    public GameObject go;

    protected void Start()
    {
        L.og(L.Contexts.CELESTIAL_BODY, String.Format($"{BodyType} {Name} is alive", BodyType, Name));
    }

    public abstract (string, EventID) Interact();

    public new string ToString()
    {
        switch (BodyType)
        {
            case Type.STAR:
                return "Star";
            case Type.PLANET:
                return "Planet";
            default:
                return "Unknown celestial body type: " + BodyType;
        }
    }
}
