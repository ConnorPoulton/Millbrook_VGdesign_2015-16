using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
//Script written by Connor Poulton
public class GruntAI : MonoBehaviour {
    //navigation system values
   public int[] nodes; //set value in the Unity editor
   public List<Transform> nodeTransforms;
   public NavMeshAgent agent;

    //state machine variables
    private enum States {PathToNextNode, WaitAtNode, InvestigateNoise, FoundPlayer};
    private States currentstate;
    private States laststate;

	void Start ()
    {

        //NavMeshAgent agent = GetComponent<NavMeshAgent>();
        //TODO develop a way to retrieve the Nav Mesh componenet for the current level
        //TODO develop a way to associate WaitAtNode instructions with nodes (time to wait, direction to turn)

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

    //call in the termination statement of an IEnumerator coroutine
    void NextState()
    {
        string stop = laststate.ToString();
        string start = currentstate.ToString();
        StopCoroutine(stop);
        StartCoroutine(start);
    }

    //call to trigger IEnumerator coroutine termination
    void NewState(States nextstate)
    {
        laststate = currentstate;
        currentstate = nextstate;
    }

    /*example state coroutine

    private IEnumerator PathToNextNode()
    {
        while (currentstate == States.PathToNextNode)
        {

            if (terminate)
            {
                States nextstate = States.WaitAtNode;  NewState (nextstate);
            }
            yield return null;
        }
        NextState ();
    }
    */
}
