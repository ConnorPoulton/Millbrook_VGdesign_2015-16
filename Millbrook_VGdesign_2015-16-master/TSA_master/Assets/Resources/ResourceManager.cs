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
        public static readonly Vector3 OUTOFBOUNDS = new Vector3(0, -999, 0);

        //scene values
        public static Vector3 SceneLLBounds;
        public static Vector3 SceneURBounds;
        

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

        public static Vector3 MakeCameraVecInBounds(Vector3 target)
        {
            float CamXOffSet = 0;
            float CamZOffSet = 0;

            Vector3 CamLLBound = Camera.main.ViewportToWorldPoint(new Vector3(0, target.y, 0));
            Vector3 CamURBound = Camera.main.ViewportToWorldPoint(new Vector3(1, target.y, 1));

            CamXOffSet = Mathf.Abs(CamLLBound.x / 2); //gets distance in world space from edge of camera view to camera origin
            CamZOffSet = Mathf.Abs(CamLLBound.z / 2);

            float ZMin = ResourceManager.SceneLLBounds.z + 8;
            float ZMax = ResourceManager.SceneURBounds.z - 8;
            float XMax = ResourceManager.SceneURBounds.x - 8;
            float XMin = ResourceManager.SceneLLBounds.x - 8;

            target.x = (target.x < XMin) ? XMin : (target.x > XMax) ? XMax : target.x;
            target.z = (target.z < ZMin) ? ZMin : (target.z > ZMax) ? ZMax : target.z;

            return target;
        }
    }
}
