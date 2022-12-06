using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCar : MonoBehaviour
{

    public Track thisTrack;

    private Rigidbody myRB;
    public int currentWaypoint = 0;
    public float StandardSpeed = 1000000;

    private float holderDistanceTarget;
    private float holderDistanceLeft;
    private float holderDistanceCenter;
    private float holderDistanceRight;

    public Transform myWaypoint;

    private void doIturnOrNot()
    {
        holderDistanceTarget = Vector3.Distance(transform.position, thisTrack.Waypoints[currentWaypoint].transform.position);
        holderDistanceCenter = Vector3.Distance(
            transform.position + ( transform.forward * holderDistanceTarget ),
            thisTrack.Waypoints[currentWaypoint].transform.position);
        holderDistanceLeft = Vector3.Distance(
            transform.position + ( transform.forward * holderDistanceTarget ) + 
            ( transform.right * holderDistanceTarget * -0.25f ),
            thisTrack.Waypoints[currentWaypoint].transform.position);
        holderDistanceRight = Vector3.Distance(
            transform.position + (transform.forward * holderDistanceTarget ) +
            (transform.right * holderDistanceTarget * 0.25f),
            thisTrack.Waypoints[currentWaypoint].transform.position);
        if ( holderDistanceLeft < holderDistanceCenter )
        { // target is to the left, we turn left
            myRB.AddRelativeTorque(transform.up * Time.deltaTime * -350000);
        }
        if (holderDistanceRight < holderDistanceCenter)
        { // target is to the left, we turn left
            myRB.AddRelativeTorque(transform.up * Time.deltaTime * 350000);
        }
    }

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
        doIturnOrNot();
        //transform.LookAt(thisTrack.Waypoints[currentWaypoint].transform.position);

    }
}
