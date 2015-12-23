using UnityEngine;
using System.Collections;

namespace TSA
{    
    static public class ResourceManager
    {
        //event delegates
        public delegate void PlayerEventHandler(GameObject player);

        static public event PlayerEventHandler playerFound;
        static public event PlayerEventHandler playerClearedLevel;

        //constant values
        public static Vector3 OutOfBounds = new Vector3(0,-999,0);

        public static void CallplayerFound(GameObject player)
        {
            if (playerFound != null)
                playerFound(player);
        }

        public static void CallplayerClearedLevel(GameObject player)
        {
            if (playerClearedLevel != null)
                playerClearedLevel(player);
        }
    }
}
