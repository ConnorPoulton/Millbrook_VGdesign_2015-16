﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//written by Connor Poulton

[System.Serializable] //serialize to allow compatibility in Unity editor
public class Node
{
    public Transform target;
    public float degreeTurn;
    public int waitTime;
}

[RequireComponent(typeof(NavMeshAgent))]
public class GruntAI : MonoBehaviour
{

    //navigation system values
    public List<Node> nodes; //set value in the Unity editor
    NavMeshAgent agent;
    int CurrentNode = 0; //since 1 is added each iteration, set to -1 to start at zero
    int MaxNode; //stores last element in list, used to loop back to start of patrol
    float timeRotating; //used to store the exact time passed while exectuing the rotateGrunt coroutine, keeps timing consistent

    //state machine variables
    enum States { PathToNextNode, WaitAtNode, InvestigateSound, ReturnToPatrol, FoundPlayer };
    States currentstate;
    States laststate = 0;

    //refrence variables
    Transform ChildTransform;


    //----------------initialization---------------------------------------


    void Start()
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
        Debug.Log("waiting");
        if (nodes[CurrentNode].degreeTurn != 1)
        {
            StartCoroutine(RotateGrunt(nodes[CurrentNode].degreeTurn));
        }

        yield return new WaitForSeconds(nodes[CurrentNode].waitTime + timeRotating); //wait for specified time plus time it takes to rotate
        StopCoroutine(RotateGrunt(nodes[CurrentNode].degreeTurn));

        StartCoroutine(RotateGrunt(GetYAngleToward(nodes[IterateToNextNode()].target.position))); //rotate to next target
        yield return new WaitForSeconds(timeRotating + 0.5f); //wait for rotation to finish
        StopCoroutine(RotateGrunt(GetYAngleToward(nodes[IterateToNextNode()].target.position)));

        CurrentNode = IterateToNextNode();
        NewState(States.PathToNextNode);
        NextState();

        yield return null;
    }



    //------------PathToNextNode

    private IEnumerator PathToNextNode()
    {
        Debug.Log(CurrentNode);
        agent.destination = nodes[CurrentNode].target.position;
        while (currentstate == States.PathToNextNode)
        {
            if (ChildTransform.position.x == nodes[CurrentNode].target.position.x & ChildTransform.position.z == nodes[CurrentNode].target.position.z)
            {

                States nextstate = States.WaitAtNode; NewState(nextstate);
            }
            yield return null;
        }
        Debug.Log("ran");
        NextState();
    }



    //------------InvestigateSound

    private IEnumerator InvestigateSound()
    {       
        StartCoroutine(RotateGrunt(GetYAngleToward(agent.destination)));
        yield return new WaitForSeconds(timeRotating + 0.5f);
        StopCoroutine(RotateGrunt(GetYAngleToward(agent.destination)));

        while (currentstate == States.InvestigateSound)
        {
            if (ChildTransform.position.x == agent.destination.x | ChildTransform.position.y == agent.destination.y)
            {
                yield return new WaitForSeconds(1);
                StartCoroutine(RotateGrunt(GetYAngleToward(nodes[CurrentNode].target.position))); //rotate to next target
                yield return new WaitForSeconds(timeRotating + 0.5f); //wait for rotation to finish
                StopCoroutine(RotateGrunt(GetYAngleToward(nodes[CurrentNode].target.position)));

                States nextstate = States.PathToNextNode; NewState(nextstate);
            }
        }
        yield return null;
        
        NextState();
    }



    //---------------state machine triggers-----------------------------------------

    void OnCollisionEnter(Collision col) //check to see if noise has been heard
    {
        Debug.Log("heard it");
        if (col.gameObject.tag == "SoundField") //to save on processing, simply have SoundFileds spawn/despawn instead of checking for an active trigger
        {
            agent.destination = col.gameObject.transform.parent.position;//path to parent of SoundField
            States nextstate = States.InvestigateSound; NewState(nextstate);
            NextState();
        }
        
    }


    //---------------coroutines-----------------------------------------------------

    public IEnumerator RotateGrunt(float targetRotation)
    {
        float ElapsedTime = 0f;
        float angle;
        float time = .5f;
        timeRotating = 0f;
        while (ElapsedTime <= time)
        {
            ElapsedTime += Time.deltaTime;
            float perc = (ElapsedTime / time);
            angle = Mathf.LerpAngle(this.transform.eulerAngles.y, targetRotation, perc);
            transform.eulerAngles = new Vector3(0, angle, 0);
            yield return new WaitForEndOfFrame();
        }
        timeRotating = ElapsedTime;
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



    //-------------------functions---------------------------

    public float GetYAngleToward(Vector3 target)
    {
        var trans = this.transform.position;
        Vector3 direction = (target - trans).normalized;
        Quaternion q = Quaternion.LookRotation(direction);
        Vector3 angle = q.eulerAngles;
        return angle.y;
    }

    public int IterateToNextNode() //returns next Node in nodes
    {
        int returnVale;
        returnVale = (CurrentNode == MaxNode) ? 0 : returnVale = CurrentNode + 1;
        return returnVale;
    }

    
}

