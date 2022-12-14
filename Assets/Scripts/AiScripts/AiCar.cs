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

    private float reverseTimer = 0;
    private float reverseCooldown = 2f;
    private bool reverse = false;

    public bool grounded = true;

    public Transform myWaypoint;

    void doIturnSetup()
    {
        leftEye = Instantiate(centerEye, transform.position, transform.rotation, transform);
        leftEye.gameObject.name = "left";
        rightEye = Instantiate(centerEye, transform.position, transform.rotation, transform);
        rightEye.gameObject.name = "right";
        centerRightEye = Instantiate(centerEye, transform.position, transform.rotation, transform);
        centerRightEye.gameObject.name = "centerRight";
        centerLeftEye = Instantiate(centerEye, transform.position, transform.rotation, transform);
        centerLeftEye.gameObject.name = "centerLeft";
        backEye = Instantiate(centerEye, transform.position, transform.rotation, transform);
        backEye.gameObject.name = "back";

        eyeDistance.Add(centerEyeDistance);
        eyeDistance.Add(leftEyeDistance);
        eyeDistance.Add(rightEyeDistance);
        eyeDistance.Add(centerRightEyeDistance);
        eyeDistance.Add(CenterLeftEyeDistance);
        eyeDistance.Add(backEyeDistance);
    }

    public Transform centerEye;
    private Transform leftEye;
    private Transform rightEye;
    private Transform centerLeftEye;
    private Transform centerRightEye;
    private Transform backEye;

    private float centerEyeDistance;
    private float leftEyeDistance;
    private float rightEyeDistance;
    private float CenterLeftEyeDistance;
    private float centerRightEyeDistance;
    private float backEyeDistance;
    private float minEyeDistance;
    private int turnEye;
    private List<float> eyeDistance = new List<float>();

    private Vector3 randomVector;

    private float targetDistance;

    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        thisTrack = GameObject.FindObjectOfType<Track>();
        doIturnSetup();
    }

    void doIturnUpdate()
    {
        targetDistance = Vector3.Distance(transform.position, thisTrack.Waypoints[currentWaypoint].transform.position);
        centerEye.transform.position = transform.position + transform.forward * targetDistance;
        centerLeftEye.transform.localPosition = new Vector3(Mathf.Sin(-3 / 57.5f) * targetDistance, 0, Mathf.Cos(-3 / 57.5f) * targetDistance);
        centerRightEye.transform.localPosition = new Vector3(Mathf.Sin(3 / 57.5f) * targetDistance, 0, Mathf.Cos(3 / 57.5f) * targetDistance);
        leftEye.transform.localPosition =  new Vector3(Mathf.Sin(-15 / 57.5f) * targetDistance, 0, Mathf.Cos(-15 / 57.5f) * targetDistance);
        rightEye.transform.localPosition = new Vector3(Mathf.Sin(15 / 57.5f) * targetDistance, 0, Mathf.Cos(15 / 57.5f) * targetDistance);
        backEye.transform.position = transform.position + -transform.forward * targetDistance;
        doIturnOrNot();
    }

    private void doIturnOrNot()
    {
        randomVector = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2));
        eyeDistance[0] = Vector3.Distance(centerEye.transform.position, thisTrack.Waypoints[currentWaypoint].transform.position + randomVector);
        eyeDistance[1] = Vector3.Distance(leftEye.transform.position, thisTrack.Waypoints[currentWaypoint].transform.position + randomVector);
        eyeDistance[2] = Vector3.Distance(rightEye.transform.position, thisTrack.Waypoints[currentWaypoint].transform.position + randomVector);
        eyeDistance[3] = Vector3.Distance(centerRightEye.transform.position, thisTrack.Waypoints[currentWaypoint].transform.position + randomVector);
        eyeDistance[4] = Vector3.Distance(centerLeftEye.transform.position, thisTrack.Waypoints[currentWaypoint].transform.position + randomVector);
        eyeDistance[5] = Vector3.Distance(backEye.transform.position, thisTrack.Waypoints[currentWaypoint].transform.position + randomVector);
        minEyeDistance = 10000;
        int count =-1;
        foreach(float eye in eyeDistance)
        {
            count++;
            if (eye < minEyeDistance) 
            {
                minEyeDistance = eye;
                turnEye = count;
            }
        }
        if (grounded)
        {
            switch (turnEye)
            {
                case 0: // center
                    break;
                case 1: // left
                    myRB.AddRelativeTorque(transform.up * Time.deltaTime * -TurnSpeed);
                    break;
                case 2: // right
                    myRB.AddRelativeTorque(transform.up * Time.deltaTime * TurnSpeed);
                    break;
                case 3: // centerRight
                    myRB.AddRelativeTorque(transform.up * Time.deltaTime * TurnSpeed * 0.7f);
                    break;
                case 4: // centerLeft
                    myRB.AddRelativeTorque(transform.up * Time.deltaTime * -TurnSpeed * 0.7f);
                    break;
                case 5: // back
                    break;
                default: // fuck
                    break;
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

    void Update()
    {
        if (thisTrack == null) return;

        doIReverse();
        if (!reverse && grounded ) myRB.AddForce(transform.forward * Time.deltaTime * StandardSpeed * thisTrack.Waypoints[currentWaypoint].AiAccelerate);
        doIturnUpdate();
        flip();

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
