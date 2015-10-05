using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
//Script written by Connor Poulton
public class GruntAI : MonoBehaviour {

   public int[] nodes; //set value in the Unity editor
   public List<Transform> nodeTransforms;
   public NavMeshAgent agent;
	
	void Start ()
    {

        //NavMeshAgent agent = GetComponent<NavMeshAgent>();
        //TODO develop a way to retrieve the Nav Mesh componenet for the current level
        if (nodes.Length == 0) //if no nodes are attached to the Grunt, set the current position as a node so it can pathfind back to its starting position
        {
            nodeTransforms.Add(this.transform);
        }else {

            foreach (int node in nodes)
            {
                Debug.Log(node.ToString());
                string nodeName = "node" + nodes[node].ToString();
               
                GameObject currentNode = GameObject.Find(nodeName);
                try
                { nodeTransforms.Add(currentNode.transform); }
                catch
                { Debug.Log("You have mislabled node" + nodes[node].ToString() + " in enemy " + this.name);  break; }

            }

        }

	}


    void Update()
    {
        
    }
	
	
}
