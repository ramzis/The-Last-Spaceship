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
        STAR,
        BEGINNER_PLANET,
        BEGINNER_STAR,
    }

    public struct EventID
    {
        public string ID;

        public EventID(string ID)
        {
            this.ID = ID;
        }
    }

    public string Name;
    public Type BodyType;
    public Sprite[] Sprites;
    public GameObject go;
    public List<(string, EventID)> responses;
    protected int timesInteracted;

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
            case Type.BEGINNER_STAR:
                return "Beginner Star";
            case Type.BEGINNER_PLANET:
                return "Beginner Planet";
            default:
                return "Unknown celestial body type: " + BodyType;
        }
    }
}
