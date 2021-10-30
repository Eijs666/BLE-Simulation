using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeaconTracker : MonoBehaviour
{
    float currentTime;
    public float startingTime = 0f;
    [SerializeField] Text countdownText; //Time Text
    public bool trackBeacon = false;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (trackBeacon) //Start tracking BeaconTime
        {
            currentTime = currentTime + 1 * Time.deltaTime;
            countdownText.text = currentTime.ToString(gameObject.name + ": "  + currentTime);
        }
        else { return; } //Stop tracking BeaconTime
        }
}
