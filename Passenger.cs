using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //Implementing the AI library

public class Passenger : MonoBehaviour
{
    BeaconTracker BT; //Literally the beacon - Come on!

    NavMeshAgent agent; //create variable for the passenger body

    //Targets- passenger walks to these targets
    public Transform Beacon; //Attach the target position in the Unity Editor
    public Transform Exit; //Attach the exit position in the Unity Editor
    public Transform ReaderIn; //Attach the Rejsekort Reader In position
    public Transform ReaderOut; //Attach the Rejsekort Reader Out position

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>(); //Get the AI agent component
        BT = GetComponent<BeaconTracker>();
    }

    // Start is called before the first frame update
    void Start()
    {
        EnterBus();
    }

    // Update is called once per frame
    void Update()
    {
        //Calculate distance
        float dist = Vector3.Distance(agent.transform.position, Beacon.transform.position);

        //Calculate distance to reader In
        float distToReaderIn = Vector3.Distance(agent.transform.position, ReaderIn.transform.position);
        //Calculate distance to reader Out
        float distToReaderOut = Vector3.Distance(agent.transform.position, ReaderOut.transform.position);

        if(distToReaderIn < 2)
        {
            //Get the Renderer component from the new cube
            var cubeRenderer = agent.GetComponent<Renderer>();

            //Call SetColor using the shader property name "_Color" and setting the color to red
            cubeRenderer.material.SetColor("_Color", Color.blue);

            //Stop for 1 second
            StartCoroutine(walkAfterPause()); //Calling pause method
            EnterBus();
            
        }
        else //Reset
        {
            //Get the Renderer component from the new cube
            var cubeRenderer = agent.GetComponent<Renderer>();

            //Call SetColor using the shader property name "_Color" and setting the color to red
            cubeRenderer.material.SetColor("_Color", Color.white);
        }

        if (distToReaderOut < 2)
        {
            //Get the Renderer component from the new cube
            var cubeRenderer = agent.GetComponent<Renderer>();

            //Call SetColor using the shader property name "_Color" and setting the color to red
            cubeRenderer.material.SetColor("_Color", Color.blue);

            //Stop for 1 second
            StartCoroutine(walkAfterPause()); //Calling pause method
            ExitBus();

        }
        else //Reset
        {
            //Get the Renderer component from the new cube
            var cubeRenderer = agent.GetComponent<Renderer>();

            //Call SetColor using the shader property name "_Color" and setting the color to red
            cubeRenderer.material.SetColor("_Color", Color.white);
        }



        if (dist < 7) //Start tracking the beacon time
        {
            BT.trackBeacon = true;
        }
        else { BT.trackBeacon = false; } //Turn off tracking

        if (dist < 1) //Leave the bus 
        {
            ExitBus(); //Leave the bus after 5 seconds
        }
    }

    void EnterBus() //Move to the beacon
    {
        agent.SetDestination(Beacon.position); //Move AI agent (passenger) to the bus 
    }
    void ExitBus() //Move to exit
    {
        agent.isStopped = false;
        agent.SetDestination(Exit.position); //Move AI agent (passenger) to the bus 
        print("Exit");
    }

    //Method that allows us to stop code or delay for 1 second
    IEnumerator walkAfterPause()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(1f); //Wait 1 second - then execute code under this line
        agent.isStopped = false;
    }

}
