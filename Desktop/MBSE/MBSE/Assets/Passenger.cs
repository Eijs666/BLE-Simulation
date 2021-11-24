using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI; //Implementing the AI library

public class Passenger : MonoBehaviour
{
    public BeaconTracker BT; //Literally the beacon - Come on!
    public NavMeshAgent agent; //create variable for the passenger body
    public BusMove bus;

    //Targets- passenger walks to these targets
    public Transform Beacon; //Attach the target position in the Unity Editor
    public Transform Exit; //Attach the exit position in the Unity Editor
    public Transform CheckIn; //Attach the Rejsekort Reader In position
    public Transform CheckOut; //Attach the Rejsekort Reader Out position
    public Transform PointA;
    public Transform PointB;
    public Transform PointC;

    public bool nearBeacon = false; // Used to only add one passenger 
    public bool driveBus = false; // Whether bus is driving or not
    public bool thisIsAMess = false; // Can't remember why
    public bool goToExit = false; // For BLE, just ensure they go towards exit
    public bool enterBus = false; // For BLE, to ensure EnterBus() is only called once 
    public bool test = false;
    public bool test1 = false;
    public bool test2 = false;

    static List<int> passengersInBus = new List<int>(); // List of passengers in bus (near beacon)

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>(); //Get the AI agent component
        BT = GetComponent<BeaconTracker>(); //Get BT component
    }

    // Start is called before the first frame update
    void Start()
    {
        
        if(bus.Rejsekort) // if Rejsekort is used go towards front door
        {
            agent.SetDestination(CheckIn.position);
            BT.timerBLE = false;
        } else // else enter from any door
        {
            EnterBus();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Calculate distance
        float dist = Vector3.Distance(agent.transform.position, Beacon.transform.position);
        float distPointA = Vector3.Distance(agent.transform.position, PointA.transform.position);
        float distPointB = Vector3.Distance(agent.transform.position, PointB.transform.position);
        float distPointC = Vector3.Distance(agent.transform.position, PointC.transform.position);
        //Calculate distance to reader In
        float distToReaderIn = Vector3.Distance(agent.transform.position, CheckIn.transform.position);
        //Calculate distance to reader Out
        float distToReaderOut = Vector3.Distance(agent.transform.position, CheckOut.transform.position);

        if(distPointA < 2)
        {
            test = true;
            agent.SetDestination(PointB.position); //Move AI agent (passenger) to the bus 
        }
        if (test)
        {
            if (distPointB < 2 && dist > 7 && test2 == false)
            {
                if (bus.startDriving)
                {
                    test2 = true;
                    
                }
                
            } else if (test2)
            {
                agent.SetDestination(PointC.position);
                //agent.SetDestination(PointA.position);
                var cubeRenderer = agent.GetComponent<Renderer>();

                //Call SetColor using the shader property name "_Color" and setting the color to red
                cubeRenderer.material.SetColor("_Color", Color.white);
                BT.trackBeacon = false;
            }  
            else
            {
                if(distPointB < 2 && dist < 7)
                {
                    BT.trackBeacon = true;
                    var cubeRenderer = agent.GetComponent<Renderer>();

                    //Call SetColor using the shader property name "_Color" and setting the color to red
                    cubeRenderer.material.SetColor("_Color", Color.blue);
                }
                agent.SetDestination(PointB.position); //Move AI agent (passenger) to the bus 
            }
        }

        // This is the check-in of a passenger. Will only be called if Rejsekort is used. 
        if (distToReaderIn < 2 && bus.Rejsekort)
        {
            //Get the Renderer component from the new cube
            var cubeRenderer = agent.GetComponent<Renderer>();

            //Call SetColor using the shader property name "_Color" and setting the color to red
            cubeRenderer.material.SetColor("_Color", Color.blue);

            //Stop for 2 seconds
            StartCoroutine(walkAfterPause(2f)); //Calling pause method
            EnterBus();

        }

        // This is the check-out of a passenger. Will only be called if Rejsekort is used. 
        if (distToReaderOut < 2 && bus.Rejsekort)
        {
           
            //Stop for 2 seconds
            StartCoroutine(walkAfterPause(3f)); //Calling pause method
            
            ExitBus();

        }

        // This ensures that the passenger will turn white again once outside of bus.
        if (distToReaderOut > 2 && dist > 7 && bus.Rejsekort)
        {
            var cubeRenderer = agent.GetComponent<Renderer>();
            cubeRenderer.material.SetColor("_Color", Color.white);
            BT.trackBeacon = false;
        }

        // This ensures that the passenger will turn white again when BLE is used and outside bus.
        if (dist > 7 && bus.Rejsekort == false && bus.startDriving == false)
        {
            var cubeRenderer = agent.GetComponent<Renderer>();
            cubeRenderer.material.SetColor("_Color", Color.white);
            BT.trackBeacon = false;
        }

        // This ensures that passenger will head towards middle of bus for BLE when inside bus
        if (dist < 7 && bus.Rejsekort == false && enterBus == false)
        {
            enterBus = true;
            EnterBus();
            BT.trackBeacon = true;
            var cubeRenderer = agent.GetComponent<Renderer>();
            cubeRenderer.material.SetColor("_Color", Color.blue);

        }

        // This ensures that the passenger will head towards exit when bus isn't driving and is inside beacon range
        if (dist < 7 && bus.Rejsekort == false && goToExit) 
        {
            
            
            var objects = GameObject.FindGameObjectsWithTag("Player"); // get obj
            
            BT.trackBeacon = true;
            foreach (GameObject obj in objects)
            {
                
                obj.GetComponent<NavMeshAgent>().SetDestination(Exit.position);
            }

        }

        if (dist < 7 && bus.Rejsekort) //Start tracking the beacon time
        {
            BT.trackBeacon = true;
        } 
        //else { BT.trackBeacon = false; } //Turn off tracking

        // Only triggered when near beacon i.e. driving of bus.
        if (dist < 2 && nearBeacon == false) // near beacon
        {
            nearBeacon = true; // to ensure that this statement is only triggered once per passenger
            passengersInBus.Insert(0, 1); // Inserts the passengers into a list
            print("Size of list " + passengersInBus.Count);
            if (passengersInBus.Count > 0 && driveBus == false) // do this for every passenger while bus is not driving
            {
                agent.isStopped = true; // stops movement of agent
                agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance; // since they stack near each other,
                // they need to have no collision or else they can't get near the beacon nor exit the bus.
                agent.transform.position = new Vector3(agent.transform.position.x, agent.transform.position.y, 63); // On top of beacon ish
                // This wont be a problem since there's no collision.
                if (passengersInBus.Count == 5) // ALL ABOARD LETS GOOO
                {
                    bus.startDriving = true; // Tell the bus to start driving
                    print("Bus should drive"); // It is in fact driving
                    driveBus = true; // Call drive() method 
                }
                
            }
        }
        // if driving and not sure why thisIsAMess but it works so who cares and who is even looking at this comment except the spaghetti programmer
        if (driveBus && thisIsAMess == false)
        {
            drive(); 
        }
    }

    void drive()
    {
        // Get access to all the passengers
        var objects = GameObject.FindGameObjectsWithTag("Player"); // get obj
        
        // For each passenger do something
        foreach (GameObject obj in objects)
        {
            if(obj.name != "Obstacle")
            {
                // something
                var projected = obj.GetComponent<NavMeshAgent>().velocity; 
                projected.y = 0f;
                obj.GetComponent<NavMeshAgent>().transform.rotation = Quaternion.LookRotation(projected);
                //  basically ensures that the passenger is facing the same way as the bus is driving
                obj.GetComponent<NavMeshAgent>().transform.Translate(Vector3.forward * 7 * Time.deltaTime);
                // same speed as bus is driving
                BT.trackBeacon = true; // keep tracking time
                if (bus.Rejsekort)
                {
                    var cubeRenderer = obj.GetComponent<NavMeshAgent>().GetComponent<Renderer>();
                    //Call SetColor using the shader property name "_Color" and setting the color to red
                    cubeRenderer.material.SetColor("_Color", Color.blue);
                } else
                {
                    var cubeRenderer = obj.GetComponent<NavMeshAgent>().GetComponent<Renderer>();
                    //Call SetColor using the shader property name "_Color" and setting the color to red
                    cubeRenderer.material.SetColor("_Color", Color.green);

                }
                if (obj.GetComponent<NavMeshAgent>().transform.position.z > 90) // when bus resets then do the same for passengers
                {
                    BT.trackBeacon = true;
                    obj.GetComponent<NavMeshAgent>().transform.position = new Vector3(obj.GetComponent<NavMeshAgent>().transform.position.x, obj.GetComponent<NavMeshAgent>().transform.position.y, -62f);
                    // reset position
                }
                if (bus.startDriving == false) // once it is done driving
                {
                    thisIsAMess = true; // yep, checks out
                    obj.GetComponent<NavMeshAgent>().obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
                    // add collision again
                    obj.GetComponent<NavMeshAgent>().isStopped = false; // you are free to move again 
                    obj.GetComponent<NavMeshAgent>().SetDestination(Exit.position); // head home, young one
                    goToExit = true; // yea sure, now I can call another method idk
                }
            }
        }
    }

    void EnterBus() //Move to the beacon
    {
        agent.SetDestination(Beacon.position); //Move AI agent (passenger) to the bus 
    }

    void ExitBus() //Move to exit
    {
        //agent.isStopped = false;
        agent.SetDestination(Exit.position); //Move AI agent (passenger) to the bus 
        //print("Exit");
    }

    //Method that allows us to stop code or delay for 1 second
    IEnumerator walkAfterPause(float waitTime)
    {
        
        agent.isStopped = true;
        yield return new WaitForSeconds(waitTime); //Wait 1 second - then execute code under this line
        agent.isStopped = false;
    }


}