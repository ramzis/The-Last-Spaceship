using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logging
{
    // Logging system.
    // Must call L.SetOptions() or default values will be set.
    public class L : MonoBehaviour
    {
        // Should any logging be done at all?
        private static bool doLogging;
        // Should all contexts be logged even if they are not specified?
        private static bool overrideEnableAllContexts;
        // The logging contexts that should be logged if not overwritten
        private static List<string> loggingContexts;

        private void Awake()
        {
            // Set default values
            L.SetOptions(true, true, new List<string>() { });
        }

        // Log a value to the console.
        // context - a value providing info for the origin of the log
        // message - the value to log to the console
        public static void og(string context, string message)
        {
            // Log message if override is enabled or the context is enabled
            if (doLogging && (overrideEnableAllContexts || loggingContexts.Contains(context)))
            {
                Debug.Log($"[{context}]: {message}");
            }
        }

        // Specify the logging parameters to use such as:
        // Should logging be done, should all or only specified contexts be logged
        public static void SetOptions(bool doLogging, bool overrideEnableAllContexts, List<string> loggingContexts)
        {
            L.doLogging = doLogging;
            L.overrideEnableAllContexts = overrideEnableAllContexts;
            L.loggingContexts = loggingContexts;
        }

        // Define contexts for providing information of the origin of a log
        public static class Contexts
        {
            public static readonly string GAME_MANAGER = "GAME_MANAGER";
            public static readonly string EFFECTS = "EFFECTS";
            public static readonly string CELESTIAL_BODY = "CELESTIAL_BODY";
            public static readonly string PLANET_MANAGER = "PLANET_MANAGER";
            public static readonly string POSITION_LOOPER = "POSITION_LOOPER";
            public static readonly string SECTOR_RESOLVER = "SECTOR_RESOLVER";
            public static readonly string SHIP_CONTROLLER = "SHIP_CONTROLLER";
        }
    }
}

