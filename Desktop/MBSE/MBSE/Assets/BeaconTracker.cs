using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public bool addPassenger = false;
    public bool removePassenger = false;
    public bool test = false;
    public bool test1 = false;
    float timer;

    static List<int> PassengerList = new List<int>();

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
        timer += 1 * Time.deltaTime;
        WriteString(gameObject.name, checkInTime, timer, PassengerList.Count);
        //WriteStringRejsekort(gameObject.name, timer, PassengerList.Count);
        if (addPassenger)
        {
            if (test == false)
            {
                PassengerList.Insert(0, 1);
                print("size of passenger list: " + PassengerList.Count);
                test = true;
            }
        }
        if (removePassenger)
        {
            
            if (test1 == false)
            {
                PassengerList.RemoveAt(0);
                print("size of passenger list: " + PassengerList.Count);
                test1 = true;
            }
        }

        if (trackBeacon) //Start tracking BeaconTime
        {
            
            
            if (timerBLE && bus.startDriving == false && currentTime > 23)
            {
                checkInTime += 1 * Time.deltaTime;
                currentTime += 1 * Time.deltaTime;
                countdownText.text = currentTime.ToString(gameObject.name + ": " + "In range " + Mathf.RoundToInt(currentTime) + " Check in: " + Mathf.RoundToInt(checkInTime));
                //WriteString(gameObject.name, checkInTime, timer, PassengerList.Count);
            }
            else if (timerBLE && bus.startDriving)
            {
                checkInTime += 1 * Time.deltaTime;
                currentTime += 1 * Time.deltaTime;
                countdownText.text = currentTime.ToString(gameObject.name + ": " + "In range " + Mathf.RoundToInt(currentTime) + " Check in: " + Mathf.RoundToInt(checkInTime));
                //WriteString(gameObject.name, checkInTime);
            }
            else if (timerBLE && bus.startDriving == false && currentTime < 15)
            {
                currentTime += 1 * Time.deltaTime;
                countdownText.text = currentTime.ToString(gameObject.name + ": " + "In range: " + Mathf.RoundToInt(currentTime));
                //WriteString(gameObject.name, checkInTime);
            }  
            else
            {
                currentTime += 1 * Time.deltaTime;
                countdownText.text = currentTime.ToString(gameObject.name + ": " + "Check-in: " + Mathf.RoundToInt(currentTime));
                
            }

            

            
        }
        else { return; } //Stop tracking BeaconTime
        
    }
    void WriteStringRejsekort(string name, float currentTime, int count)
    {
        string path = "Assets/Resources/test.txt";
        int cIT = (int)currentTime;
        StreamWriter writer = new StreamWriter(path, true);
        //name + ";" + 
        writer.WriteLine(name + ";" + cIT + ";" + count);
        writer.Close();


    }

    void WriteString(string name, float checkInTime, float timer, int count)
    {
        string path = "Assets/Resources/test.txt";
        int cIT = (int)checkInTime;
        int tT = (int)timer;
        StreamWriter writer = new StreamWriter(path, true);
        //name + ";" + 
        writer.WriteLine(name + ";" + cIT + ";" + tT + ";" + count);
        writer.Close();


    }
}
