using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeaconTracker : MonoBehaviour
{
    float currentTime;
    float checkInTime;
    public float startingTime = 0f;
    [SerializeField] Text countdownText; //Time Text
    public bool trackBeacon = false;
    public bool trackPassenger = false;
    public bool timerBLE = true;

    public bool beacon1 = false;
    public bool beacon2 = false;

    public BusMove bus;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;
        checkInTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (trackBeacon) //Start tracking BeaconTime
        {
            if (beacon1 && bus.startDriving == false)
            {
                currentTime += 1 * Time.deltaTime;
                countdownText.text = currentTime.ToString(gameObject.name + ": " + "beacon 1: " + Mathf.RoundToInt(currentTime));
            }
            if (beacon2 && bus.startDriving == false)
            {
                currentTime += 1 * Time.deltaTime;
                countdownText.text = currentTime.ToString(gameObject.name + ": " + "beacon 2: " + Mathf.RoundToInt(currentTime));
            }
            if (beacon1 && bus.startDriving)
            {
                checkInTime += 1 * Time.deltaTime;
                currentTime += 1 * Time.deltaTime;
                countdownText.text = currentTime.ToString(gameObject.name + ": " + "beacon 1: " + Mathf.RoundToInt(currentTime) + " Check in: " + Mathf.RoundToInt(checkInTime));
                if (checkInTime > 2)
                {
                    beacon1 = false;
                    countdownText.text = currentTime.ToString(gameObject.name + ": ERROR");
                }
            }
            if (beacon2 && bus.startDriving)
            {
                checkInTime += 1 * Time.deltaTime;
                currentTime += 1 * Time.deltaTime;
                countdownText.text = currentTime.ToString(gameObject.name + ": " + "beacon 2: " + Mathf.RoundToInt(currentTime) + " Check in: " + Mathf.RoundToInt(checkInTime));
            }


        }
        else { return; } //Stop tracking BeaconTime

    }


}
