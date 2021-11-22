using System.Collections;
using UnityEngine;

public class BusMove : MonoBehaviour
{

    public Transform Bus; // Your mom
    public Transform Beacon;

    private bool checkPoint = false;
    public bool startDriving = false;
    public bool isDriving = false;
    public bool Rejsekort = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // When bus if full (all passengers near beacon) this will become true
        if (startDriving)
        {
            StartBus();
        }
        

    }

    public void StartBus()
    {
        // Drives to the right 
        if (Bus.position.z < 100 && checkPoint == false && startDriving)
        {
            StartCoroutine(DriveBus(100f)); // idk why I chose 100f
        }
        // Resets bus at position outside of camera view
        else if (Bus.position.z > 100 && startDriving)
        {
            checkPoint = true;
            Bus.transform.position = new Vector3(39.62f, 9.65f, -50.0f);
        }
        // After bus is reset it should continue to drive again
        else if (Bus.position.z < 67 && checkPoint && startDriving)
        {
            StartCoroutine(DriveBus(100f));
        } // bus not driving
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
