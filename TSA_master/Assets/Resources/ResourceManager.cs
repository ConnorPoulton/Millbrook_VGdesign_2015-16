using UnityEngine;
using System.Collections;

namespace TSA
{
    static public class ResourceManager
    {
        //event delegates
        public delegate void PlayerEventHandler();

        static public event PlayerEventHandler playerFound;
        static public event PlayerEventHandler playerClearedLevel;

        //constant values
        public static Vector3 OutOfBounds = new Vector3(0, -999, 0);

        public static void CallplayerFound()
        {
            if (playerFound != null)
                playerFound();
        }

        public static void CallplayerClearedLevel()
        {
            if (playerClearedLevel != null)
                playerClearedLevel();
        }
    }
}
