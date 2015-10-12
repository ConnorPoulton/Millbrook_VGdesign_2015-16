using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
//Script written by Connor Poulton
public class GruntAI : MonoBehaviour {
    //navigation system values
   public int[] Nodes; //set value in the Unity editor
   public List<Transform> NodeTransforms;
   public NavMeshAgent agent;
   private int CurrentNode = 0;
   private Transform CurrentNodeTransform;
   private int NodeTransformsMax; //stores last element in list, used to loop back to start of patrol
    //state machine variables
    private enum States {PathToNextNode, WaitAtNode, InvestigateNoise, ReturnToPatrol, FoundPlayer};
    private States currentstate;
    private States laststate = 0;


    //----------------initialization---------------------------------------


	void Start ()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        { Debug.Log(this.name.ToString() + " has no NavMeshAgent"); }
        

        
        //TODO develop a way to associate WaitAtNode instructions with nodes (time to wait, direction to turn)

        if (Nodes.Length == 0) //if no nodes are attached to the Grunt, set the current position as a node so it can pathfind back to its starting position
        {
            NodeTransforms.Add(this.transform);
        }else {

            for (int node = 0; node <= Nodes.Length - 1; node++ )
            {
               
            
                string nodeName = "node" + Nodes[node].ToString();
               
                GameObject currentNode = GameObject.Find(nodeName);
               
                try
                { Debug.Log(nodeName);  NodeTransforms.Add(currentNode.transform); }
                catch
                { Debug.Log("You have mislabled node" + Nodes[node].ToString() + " in enemy " + this.name);  break; }

            }

        }

        NodeTransformsMax = NodeTransforms.Count - 1;
        this.transform.position = NodeTransforms[0].position;
	}


    //----------------------AI states------------------------------------------------------

    private IEnumerator PathToNextNode()
    {
        CurrentNodeTransform = NodeTransforms[CurrentNode];
        while (currentstate == States.PathToNextNode)
        {
            agent.destination = NodeTransforms[CurrentNode].position;

            if (this.transform.position == NodeTransforms[CurrentNode].position)
            {
                if(CurrentNode == NodeTransformsMax)
                {
                    CurrentNode = 0;
                }
                else{
                    CurrentNode += 1;
                }

                States nextstate = States.WaitAtNode; NewState(nextstate);
            }
            yield return null;
        }
        NextState();
    }


    //---------------state machine functions-----------------------------------------


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
