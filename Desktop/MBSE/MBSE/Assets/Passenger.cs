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
    public Transform ReaderIn; //Attach the Rejsekort Reader In position
    public Transform ReaderOut; //Attach the Rejsekort Reader Out position
    
    bool checkIn = false;

    public int counter = 0;
    public bool nearBeacon = false;
    public bool driveBus = false;
    public bool thisIsAMess = false;

    static List<int> passengersInBus = new List<int>();

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

        if (distToReaderIn < 2)
        {
            /*
            if (checkIn == false)
            {
                checkIn = true;
                passengersInBus.Insert(0, 1);
                print("Size of list " + passengersInBus.Count);
                if (passengersInBus.Count == 5)
                {
                    bus.startDriving = true;
                    print("Bus should drive");
                    
                }
            }
            */

            //Get the Renderer component from the new cube
            var cubeRenderer = agent.GetComponent<Renderer>();

            //Call SetColor using the shader property name "_Color" and setting the color to red
            cubeRenderer.material.SetColor("_Color", Color.blue);

            //Stop for 1 second
            StartCoroutine(walkAfterPause(2f)); //Calling pause method
            EnterBus();

        }
        /*
        else //Reset
        {
            //Get the Renderer component from the new cube
            var cubeRenderer = agent.GetComponent<Renderer>();

            //Call SetColor using the shader property name "_Color" and setting the color to red
            cubeRenderer.material.SetColor("_Color", Color.white);
        }
        */
        if (distToReaderOut < 2)
        {
            //Get the Renderer component from the new cube
            //var cubeRenderer = agent.GetComponent<Renderer>();

            //Call SetColor using the shader property name "_Color" and setting the color to red
            //cubeRenderer.material.SetColor("_Color", Color.blue);

            //Stop for 1 second
            StartCoroutine(walkAfterPause(2f)); //Calling pause method
            ExitBus();

        }
        if(distToReaderOut > 2 && dist > 7)
        {
            var cubeRenderer = agent.GetComponent<Renderer>();
            cubeRenderer.material.SetColor("_Color", Color.white);
            
        }
        /*
        else //Reset
        {
            //Get the Renderer component from the new cube
            var cubeRenderer = agent.GetComponent<Renderer>();

            //Call SetColor using the shader property name "_Color" and setting the color to red
            cubeRenderer.material.SetColor("_Color", Color.white);
        }
        */


        if (dist < 7) //Start tracking the beacon time
        {
            BT.trackBeacon = true;
        }
        else { BT.trackBeacon = false; } //Turn off tracking

        if (dist < 2 && nearBeacon == false) //Leave the bus 
        {
            
            nearBeacon = true;
            passengersInBus.Insert(0, 1);
            print("Size of list " + passengersInBus.Count);
            if (passengersInBus.Count > 0 && driveBus == false)
            {
                
                
                
                agent.isStopped = true;
                agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
                agent.transform.position = new Vector3(agent.transform.position.x, agent.transform.position.y, 63);
                /*
                foreach (GameObject obj in objects) {
                    obj.GetComponent<NavMeshAgent>().isStopped = true;
                    obj.GetComponent<NavMeshAgent>().obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
                    //agent.isStopped = true;
                    //walkAfterPause(20f);
                    obj.GetComponent<NavMeshAgent>().transform.position = new Vector3(agent.transform.position.x, agent.transform.position.y, 68);
                    //agent.transform.position = new Vector3(agent.transform.position.x, agent.transform.position.y, 68);
                }
                */
                if (passengersInBus.Count == 5)
                {
                    bus.startDriving = true;
                    print("Bus should drive");
                    driveBus = true;
                }
                
            }
            //float number = Random.Range(1.0f, 10f);
            //StartCoroutine(walkAfterPause(20f));
            //ExitBus(); //Leave the bus after 5 seconds
        }

        if (driveBus && thisIsAMess == false)
        {
            drive();
            
        }
    }

    void drive()
    {
        var objects = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (GameObject obj in objects)
        {
            
            var projected = obj.GetComponent<NavMeshAgent>().velocity;
            projected.y = 0f;
            obj.GetComponent<NavMeshAgent>().transform.rotation = Quaternion.LookRotation(projected);
            obj.GetComponent<NavMeshAgent>().transform.Translate(Vector3.forward * 7 * Time.deltaTime);
            BT.trackBeacon = true;

            var cubeRenderer = obj.GetComponent<NavMeshAgent>().GetComponent<Renderer>();

            //Call SetColor using the shader property name "_Color" and setting the color to red
            cubeRenderer.material.SetColor("_Color", Color.blue);

            if (obj.GetComponent<NavMeshAgent>().transform.position.z > 90)
            {
                BT.trackBeacon = true;
                obj.GetComponent<NavMeshAgent>().transform.position = new Vector3(obj.GetComponent<NavMeshAgent>().transform.position.x, obj.GetComponent<NavMeshAgent>().transform.position.y, -62f);
                //Get the Renderer component from the new cube

            }
            if (bus.startDriving == false)
            {
                obj.GetComponent<NavMeshAgent>().obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
                thisIsAMess = true;
                obj.GetComponent<NavMeshAgent>().isStopped = false;
                obj.GetComponent<NavMeshAgent>().SetDestination(Exit.position);
                //ExitBus();
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