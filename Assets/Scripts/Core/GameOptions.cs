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
}
