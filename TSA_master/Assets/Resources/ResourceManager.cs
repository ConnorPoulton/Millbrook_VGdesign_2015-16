using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TSA
{
    public static class ResourceManager
    {
        public static List<Transform> NodesInScene = new List<Transform>();

        public static void ClearList()
        {
            NodesInScene.Clear();
        }

        public static void AddNodeInScene(Transform item)
        {
            NodesInScene.Add(item);
        }
    }
}