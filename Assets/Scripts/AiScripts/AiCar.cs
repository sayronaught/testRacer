using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCar : MonoBehaviour
{

    public Track thisTrack;

    private Rigidbody myRB;
    public int currentWaypoint = 0;
    public float StandardSpeed = 1000000;
    public float TurnSpeed = 350000;

    private float holderDistanceTarget;
    private float holderDistanceLeft;
    private float holderDistanceCenter;
    private float holderDistanceRight;
    public float holderDirection;
    private Vector3 targetDirection;

    public Vector3 distance;

    private float reverseTimer = 0;
    private float reverseCooldown = 2f;
    private bool reverse = false;

    public bool grounded = true;

    public Transform myWaypoint;


    private void doIturnOrNot()
    {
        holderDistanceTarget = Vector3.Distance(transform.position, thisTrack.Waypoints[currentWaypoint].transform.position);
        targetDirection = thisTrack.Waypoints[currentWaypoint].transform.position - transform.position;
        holderDistanceLeft = Vector3.Distance(
            transform.position + -transform.right *3,
            thisTrack.Waypoints[currentWaypoint].transform.position);
        holderDistanceRight = Vector3.Distance(
            transform.position + transform.right *3,
            thisTrack.Waypoints[currentWaypoint].transform.position);
        holderDirection = Vector3.Angle(transform.position, targetDirection);
       
        distance = new Vector3 (holderDistanceLeft, holderDistanceTarget , holderDistanceRight);

        if (grounded)
        {
            if (holderDistanceLeft < holderDistanceTarget && holderDirection > 10f)
            { // target is to the left, we turn left
                myRB.AddRelativeTorque(transform.up * Time.deltaTime * -TurnSpeed);
            }
            if (holderDistanceRight < holderDistanceTarget && holderDirection > 10f)
            { // target is to the left, we turn left
                myRB.AddRelativeTorque(transform.up * Time.deltaTime * TurnSpeed);
            }
        }
    }

    public void doIReverse()
    {
        reverseTimer -= Time.deltaTime;
        reverseCooldown -= Time.deltaTime;
        //Debug.Log(gameObject.name + " " + myRB.velocity.magnitude + " " + reverseCooldown);
        if (myRB.velocity.magnitude <= 0.3f && reverseCooldown < 0 )
        {
            reverseTimer = 2;
            reverse = true;
        }
        if (reverse && grounded)
        {
            reverseCooldown = 0.5f;
            myRB.AddForce(-transform.forward * Time.deltaTime * StandardSpeed);
        }
        if (reverseTimer < 0)
        {
            reverse = false;
        }
    }

    private void flip()
    {
        if (transform.up.y < 0.2f && grounded)
        {
            transform.localRotation = Quaternion.Euler(0, transform.rotation.y - 90, transform.rotation.z);
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
        doIReverse();
        if (!reverse && grounded ) myRB.AddForce(transform.forward * Time.deltaTime * StandardSpeed * thisTrack.Waypoints[currentWaypoint].AiAccelerate);
        doIturnOrNot();
        flip();
        //transform.LookAt(thisTrack.Waypoints[currentWaypoint].transform.position);

    }
    private void OnCollisionStay(Collision collision)
    {
        grounded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
    }
}
