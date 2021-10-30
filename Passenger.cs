using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //Implementing the AI library

public class Passenger : MonoBehaviour
{
    BeaconTracker BT;

    NavMeshAgent agent; //create variable for the passenger body
    public Transform Beacon; //Attach the target position in the Unity Editor
    public Transform Exit; //Attach the exit position in the Unity Editor


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
        agent.SetDestination(Exit.position); //Move AI agent (passenger) to the bus 
        print("Exit");
    }

}
