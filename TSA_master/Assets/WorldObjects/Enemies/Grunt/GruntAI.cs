using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//written by Connor Poulton

[System.Serializable] //serialize to allow compatibility in Unity editor
public class Node
{
    public Transform target;
    public int degreeTurn;
    public int waitTime;
}

[RequireComponent(typeof(NavMeshAgent))]
public class GruntAI : MonoBehaviour {

    //navigation system values
    public List<Node> nodes; //set value in the Unity editor
	NavMeshAgent agent;
    int CurrentNode = 0; //since 1 is added each iteration, set to -1 to start at zero
    int MaxNode; //stores last element in list, used to loop back to start of patrol
    
    //state machine variables
    enum States {PathToNextNode, WaitAtNode, InvestigateNoise, ReturnToPatrol, FoundPlayer};
    States currentstate;
    States laststate = 0;

    //refrence variables
    Transform ChildTransform;


    //----------------initialization---------------------------------------


	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        ChildTransform = this.transform.GetChild(0); //get the body of the grunt
        GameObject levelGeometry = GameObject.FindWithTag("LevelGeometry"); //make sure you only have one object in your scene with this tag!
        
        if (levelGeometry == null)
        { Debug.Log("please apply the LevelGeometry tag to your scenes level prefab"); }


        try
        { this.transform.position = nodes[0].target.position; }
        catch
        { Debug.Log(this.name + " has no nodes attached"); }

        MaxNode = nodes.Count - 1;
        
        NewState(States.WaitAtNode);
        NextState();
        
    }


    //----------------------AI states------------------------------------------------------
    
    //----------WaitAtNode
    
    private IEnumerator WaitAtNode()
    {
        
        StartCoroutine(RotateGrunt(nodes[CurrentNode].degreeTurn));
        yield return new WaitForSeconds(nodes[CurrentNode].waitTime + );
        StopCoroutine(RotateGrunt(nodes[CurrentNode].degreeTurn));

        if (CurrentNode == MaxNode)
        { CurrentNode = 0; }
        else
        { CurrentNode += 1; }
        
        NewState(States.PathToNextNode);    
        NextState();

        yield return null;
    }

    //------------PathToNextNode

    private IEnumerator PathToNextNode()
    {
       
        while (currentstate == States.PathToNextNode)
        {
            
            agent.destination = nodes[CurrentNode].target.position;

            if (ChildTransform.position.x == nodes[CurrentNode].target.position.x & ChildTransform.position.z == nodes[CurrentNode].target.position.z) 
            {
                
                States nextstate = States.WaitAtNode; NewState(nextstate);
            }
            yield return null;
        }
        
        NextState();
    }


    //---------------coroutines-----------------------------------------------------

    public IEnumerator RotateGrunt(int targetRotation)
    {
        Debug.Log(targetRotation);
        if()
        float ElapsedTime = 0f;
        float angle;
        float time = 1f;

        while (ElapsedTime <= time)
        {
            ElapsedTime += Time.deltaTime;
            float perc = (ElapsedTime / time);
            angle = Mathf.LerpAngle(this.transform.rotation.y, targetRotation, perc);
            transform.eulerAngles = new Vector3(0, angle, 0);
            Debug.Log(transform.rotation.y);
            yield return new WaitForEndOfFrame();
        }
        yield return null;  
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
