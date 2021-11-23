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
            if (bus.startDriving == false && timerBLE)
            {
                currentTime += 1 * Time.deltaTime;
                countdownText.text = currentTime.ToString(gameObject.name + ": " + "In range " + Mathf.RoundToInt(currentTime));
                
            }
            else if (timerBLE && bus.startDriving)
            {
                checkInTime += 1 * Time.deltaTime;
                currentTime += 1 * Time.deltaTime;
                countdownText.text = currentTime.ToString(gameObject.name + ": " + "In range " + Mathf.RoundToInt(currentTime) + " Check in: " + Mathf.RoundToInt(checkInTime));
            }
            else
            {
                currentTime += 1 * Time.deltaTime;
                countdownText.text = currentTime.ToString(gameObject.name + ": " + "In range " + Mathf.RoundToInt(currentTime));
            }
        }
        else { return; } //Stop tracking BeaconTime

    }


}
