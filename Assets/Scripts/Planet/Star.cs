using System;
using System.Collections;
using System.Collections.Generic;
using Logging;
using UnityEngine;

public class Star : CelestialBody
{
    new void Start()
    {
        base.Start();
    }

    public override (string, EventID) Interact()
    {
        return ($"{name} greets you!", new EventID() {ID = ""});
    }
}
