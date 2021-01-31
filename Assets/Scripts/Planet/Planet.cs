using System;
using System.Collections;
using System.Collections.Generic;
using Logging;
using UnityEngine;

public class Planet : CelestialBody
{
    new void Start()
    {
        base.Start();
    }

    public override (string, EventID) Interact()
    {
        if(responses == null) return ("No response...", new EventID());
        if (responses.Count > timesInteracted)
        {
            var response = responses[timesInteracted];
            timesInteracted++;
            return response;
        }
        else
        {
            return ("No response...", new EventID());
        }
    }
}
