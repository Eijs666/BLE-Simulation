using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeaconTracker : MonoBehaviour
{
    public float currentDistance;
    public float startingDistance = 0f;
    [SerializeField] Text countdownText; //Time Text
    public bool trackBeacon = false;
    public int TimeToCheckIn = 5;
    public bool trackPassenger = false;
    public bool timerBLE = true;
    public float distCalc = 0f;
    public float distPointA;
    public float distPointB;

    public BusMove bus;
    public Passenger passenger;

    // Start is called before the first frame update
    void Start()
    {
        currentDistance = startingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (trackBeacon) //Start tracking BeaconTime
        {
            // timerBLE = using BLE (rejsekort = false)
            // bus.startDriving == false ---> bus is not driving

            distCalc = ((12 - currentDistance) / 12) * 100;
            countdownText.text = currentDistance.ToString(gameObject.name + ": " + "Signal strength " + Mathf.RoundToInt(distCalc));
            
            /*if (timerBLE && bus.startDriving == false && currentTime > 23)
            {
                checkInTime += 1 * Time.deltaTime;
                currentTime += 1 * Time.deltaTime;
                countdownText.text = currentTime.ToString(gameObject.name + ": " + "In range " + Mathf.RoundToInt(distCalc) + " Check in: " + Mathf.RoundToInt(checkInTime));
            }
            else if (timerBLE && bus.startDriving)
            {
                checkInTime += 1 * Time.deltaTime;
                currentTime += 1 * Time.deltaTime;
                if (checkInTime > TimeToCheckIn)
                {
                    countdownText.text = currentTime.ToString(gameObject.name + ": " + "In range " + Mathf.RoundToInt(currentTime) + " Check in: " + Mathf.RoundToInt(checkInTime));
                }
                else
                {
                    countdownText.text = currentTime.ToString(gameObject.name + ": " + "In range " + Mathf.RoundToInt(currentTime));
                }
            }
            else if (timerBLE && bus.startDriving == false && currentTime < 15)
            {
                currentTime += 1 * Time.deltaTime;
                countdownText.text = currentTime.ToString(gameObject.name + ": " + "In range: " + Mathf.RoundToInt(currentTime));
            }  
            else
            {
                currentTime += 1 * Time.deltaTime;
                countdownText.text = currentTime.ToString(gameObject.name + ": " + "Check-in: " + Mathf.RoundToInt(currentTime));
            }*/
        }
        else { 
                currentDistance = 0;
                countdownText.text = currentDistance.ToString(gameObject.name + ": " + "Signal strength " + 0);
                return; } //Stop tracking BeaconTime

    }


}
