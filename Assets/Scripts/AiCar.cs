using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCar : MonoBehaviour
{

    public Track thisTrack;

    private Rigidbody myRB;
    public int currentWaypoint = 0;
    public float StandardSpeed = 1000000;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        thisTrack = GameObject.FindObjectOfType<Track>();
    }

    // Update is called once per frame
    void Update()
    {
        if (thisTrack == null) return;
        myRB.AddForce(transform.forward * Time.deltaTime * StandardSpeed * thisTrack.Waypoints[currentWaypoint].AiAccelerate);
        transform.LookAt(thisTrack.Waypoints[currentWaypoint].transform.position);

    }
}
