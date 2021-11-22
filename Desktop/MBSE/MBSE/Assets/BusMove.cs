using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BusMove : MonoBehaviour
{

    public Transform Bus; // Your mom
    public Transform Beacon;

    private bool checkPoint = false;
    public bool startDriving = false;
    public bool isDriving = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startDriving)
        {
            StartBus();
        }
        

    }

    public void StartBus()
    {
        if (Bus.position.z < 100 && checkPoint == false && startDriving)
        {
            StartCoroutine(DriveBus(100f)); // idk why I chose 100f
        }
        else if (Bus.position.z > 100 && startDriving)
        {
            checkPoint = true;
            Bus.transform.position = new Vector3(39.62f, 9.65f, -50.0f);
        }
        else if (Bus.position.z < 67 && checkPoint && startDriving)
        {
            StartCoroutine(DriveBus(100f));
        }
        else
        {
            startDriving = false;
        }
    }

    IEnumerator DriveBus(float waitTime)
    {
        Bus.transform.Translate(Vector3.forward * 7*Time.deltaTime);
        yield return new WaitForSeconds(waitTime); //Wait 1 second - then execute code under this line
    }


}
