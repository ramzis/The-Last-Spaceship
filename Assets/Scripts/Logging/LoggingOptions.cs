using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logging;

public class LoggingOptions : MonoBehaviour
{
    public List<string> contexts;

    void Start()
    {
        L.SetOptions(true, false, contexts);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
