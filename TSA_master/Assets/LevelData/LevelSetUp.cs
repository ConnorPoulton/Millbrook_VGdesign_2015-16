using UnityEngine;
using System.Collections;
using TSA;

public class LevelSetUp : MonoBehaviour {
    private string nodeName;

    void Start()
    {
        for (int i = 0; i <= this.transform.childCount; i++)
        {
            nodeName = ("node" + i);
            foreach (Transform item in this.transform)
            {
                if (item.name == nodeName)
                { TSA.ResourceManager.AddNodeInScene(item);}
            }
        }
        
    }
	
}
