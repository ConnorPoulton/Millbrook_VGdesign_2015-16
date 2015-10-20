using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TSA;
//written by Connor Poulton

[System.Serializable] //serialize to allow compatibility in Unity editor
public class Node
{
    public int node;
    public int degreeTurn;
    public int waitTime;
}

[RequireComponent(typeof(NavMeshAgent))]
public class GruntAI : MonoBehaviour {
    //navigation system values
    public List<Node> nodes; //set value in the Unity editor
    NavMeshAgent agent;
    List<Transform> NodeTransforms = new List<Transform>();
    int CurrentNode = 0;
    int MaxNode; //stores last element in list, used to loop back to start of patrol
    Vector3 destination;
    //state machine variables
    enum States {PathToNextNode, WaitAtNode, InvestigateNoise, ReturnToPatrol, FoundPlayer};
    States currentstate;
    States laststate = 0;


    //----------------initialization---------------------------------------


	void Start ()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        GameObject levelGeometry = GameObject.FindWithTag("LevelGeometry"); //make sure you only have one object in your scene with this tag!
        destination = agent.destination;

        if (levelGeometry == null)
        { Debug.Log("please apply the LevelGeometry tag to your scenes level prefab"); }
        

        if (nodes.Count == 0) //if no nodes are attached to the Grunt, set the current position as a node so it can pathfind back to its starting position
        {
            NodeTransforms.Add(this.transform);
        }else {

            foreach (Node i in nodes)
            {
                NodeTransforms.Add(TSA.ResourceManager.NodesInScene[i.node]);
                try
                { NodeTransforms.Add(TSA.ResourceManager.NodesInScene[i.node]); }
                catch
                { Debug.Log("Invalid node path in enemy " + this.name); break; }
                
            }

            
               
                

        }

        this.transform.position = NodeTransforms[0].position;
        
        MaxNode = NodeTransforms.Count - 1;
        CurrentNode = 0;
        
        NewState(States.WaitAtNode);
        NextState();
        
    }


    //----------------------AI states------------------------------------------------------
    
    private IEnumerator WaitAtNode()
    {
        Debug.Log("enter WaitAtNode");
        transform.Rotate(new Vector3(0,nodes[CurrentNode].degreeTurn * Time.deltaTime,0) * Time.deltaTime);
        yield return new WaitForSeconds(nodes[CurrentNode].waitTime); //this may cause issues with state switches called from update (IE FoundPlayer)
        if (CurrentNode == MaxNode) //probably a more eleqeunt, less error prone way to do this, a task for later no doubt
        { CurrentNode = 0; }
        else
        { CurrentNode += 1; }
        Debug.Log("exit WaitAtNode");
        NewState(States.PathToNextNode);    
        NextState();
    }

    private IEnumerator PathToNextNode()
    {
        Debug.Log("enter PathToNextNode");
        while (currentstate == States.PathToNextNode)
        {
            destination = NodeTransforms[CurrentNode].position;
            agent.destination = destination;
            if (this.transform.position == NodeTransforms[CurrentNode].position)
            {
                States nextstate = States.WaitAtNode; NewState(nextstate);
            }
            yield return null;
        }
        Debug.Log("Exit PathToNextNode");
        NextState();
    }

    void Update()
    {
        agent.SetDestination(NodeTransforms[CurrentNode].position);
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
