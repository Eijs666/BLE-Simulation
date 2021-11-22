using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Beacon : MonoBehaviour
{
    //Beacon Radius
    public int beaconRadius = 7;

    //Time
    float currentTime;
    public float startingTime = 0f;
    [SerializeField] Text countdownText; //Time Text
    public bool trackTime = true;


    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (trackTime) //Start tracking time
        {
            currentTime = currentTime + 1  * Time.deltaTime;
            countdownText.text = currentTime.ToString();
        }else
        {
            return; //Stop tracking time
        }

    }

    //Draw the radius wiresphere - visual representation of beacon
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 7);
    }
}
