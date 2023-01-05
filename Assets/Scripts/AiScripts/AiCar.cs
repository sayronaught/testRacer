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
        cCRightEye = Instantiate(centerEye, transform.position, transform.rotation, transform);
        cCRightEye.gameObject.name = "cCRight";
        cCLeftEye = Instantiate(centerEye, transform.position, transform.rotation, transform);
        cCLeftEye.gameObject.name = "cCLeft";

        eyeDistance.Add(centerEyeDistance);
        eyeDistance.Add(leftEyeDistance);
        eyeDistance.Add(rightEyeDistance);
        eyeDistance.Add(centerRightEyeDistance);
        eyeDistance.Add(centerLeftEyeDistance);
        eyeDistance.Add(backEyeDistance);
        eyeDistance.Add(cCRightEyeDistance);
        eyeDistance.Add(cCLeftEyeDistance);
    }

    public Transform centerEye;
    private Transform leftEye;
    private Transform rightEye;
    private Transform centerLeftEye;
    private Transform centerRightEye;
    private Transform backEye;
    private Transform cCLeftEye;
    private Transform cCRightEye;

    private float centerEyeDistance;
    private float leftEyeDistance;
    private float rightEyeDistance;
    private float centerLeftEyeDistance;
    private float centerRightEyeDistance;
    private float backEyeDistance;
    private float cCLeftEyeDistance;
    private float cCRightEyeDistance;
    private float minEyeDistance;
    private int turnEye;
    private List<float> eyeDistance = new List<float>();

    private Vector3 randomVector;

    private float targetDistance;

    public float slowdown = 1;

    private Vector3 rayOffset = new Vector3(0,0.5f,0);

    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        thisTrack = GameObject.FindObjectOfType<Track>();
        doIturnSetup();
    }
    public RaycastHit stuckRay;
    public RaycastHit reverseRay;
    void amIstuck()
    {
        if (Physics.Raycast(transform.position + rayOffset, thisTrack.Waypoints[currentWaypoint].transform.position - transform.position, out stuckRay, targetDistance ))
        {
            currentWaypoint--;
        }
        if (currentWaypoint < 0f)
        {
            currentWaypoint = thisTrack.Waypoints.Length - 1;
        }
        Debug.DrawRay(transform.position + rayOffset, thisTrack.Waypoints[currentWaypoint].transform.position - transform.position, Color.red);
    }
    public RaycastHit forwardRay;
    public RaycastHit backRay;
    public RaycastHit leftRay;
    public RaycastHit rightRay;
    void awarenessUpdate()
    {

        if (Physics.Raycast(transform.position + rayOffset, transform.forward, out forwardRay, 5))
        {
            Debug.DrawRay(transform.position + rayOffset, transform.forward * forwardRay.distance, Color.yellow);
            slowdown = forwardRay.distance * 0.1f;
        }
        else slowdown = 1f;
        if (Physics.Raycast(transform.position + rayOffset, transform.right, out rightRay, 2))
        {
            Debug.DrawRay(transform.position + rayOffset, transform.right * rightRay.distance, Color.yellow);
            myRB.AddRelativeTorque(transform.up * Time.fixedDeltaTime * -TurnSpeed * 0.8f);
        }
        if (Physics.Raycast(transform.position + rayOffset, -transform.right, out leftRay, 2))
        {
            Debug.DrawRay(transform.position + rayOffset, -transform.right * leftRay.distance, Color.yellow);
            myRB.AddRelativeTorque(transform.up * Time.fixedDeltaTime * TurnSpeed * 0.8f);
        }
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
        cCLeftEye.transform.localPosition = new Vector3(Mathf.Sin(-1 / 57.5f) * targetDistance, 0, Mathf.Cos(-1 / 57.5f) * targetDistance);
        cCRightEye.transform.localPosition = new Vector3(Mathf.Sin(1 / 57.5f) * targetDistance, 0, Mathf.Cos(1 / 57.5f) * targetDistance);
        doIturnOrNot();
    }

    private void doIturnOrNot()
    {
        randomVector = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        eyeDistance[0] = Vector3.Distance(centerEye.transform.position, thisTrack.Waypoints[currentWaypoint].transform.position + randomVector);
        eyeDistance[1] = Vector3.Distance(leftEye.transform.position, thisTrack.Waypoints[currentWaypoint].transform.position + randomVector);
        eyeDistance[2] = Vector3.Distance(rightEye.transform.position, thisTrack.Waypoints[currentWaypoint].transform.position + randomVector);
        eyeDistance[3] = Vector3.Distance(centerRightEye.transform.position, thisTrack.Waypoints[currentWaypoint].transform.position + randomVector);
        eyeDistance[4] = Vector3.Distance(centerLeftEye.transform.position, thisTrack.Waypoints[currentWaypoint].transform.position + randomVector);
        eyeDistance[5] = Vector3.Distance(backEye.transform.position, thisTrack.Waypoints[currentWaypoint].transform.position + randomVector);
        eyeDistance[6] = Vector3.Distance(cCLeftEye.transform.position, thisTrack.Waypoints[currentWaypoint].transform.position + randomVector);
        eyeDistance[7] = Vector3.Distance(cCRightEye.transform.position, thisTrack.Waypoints[currentWaypoint].transform.position + randomVector);

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
                    myRB.AddRelativeTorque(transform.up * Time.fixedDeltaTime * -TurnSpeed);
                    break;
                case 2: // right
                    myRB.AddRelativeTorque(transform.up * Time.fixedDeltaTime * TurnSpeed);
                    break;
                case 3: // centerRight
                    myRB.AddRelativeTorque(transform.up * Time.fixedDeltaTime * TurnSpeed * 0.7f);
                    break;
                case 4: // centerLeft
                    myRB.AddRelativeTorque(transform.up * Time.fixedDeltaTime * -TurnSpeed * 0.7f);
                    break;
                case 5: // back
                    break;
                case 6: // center center left
                    myRB.AddRelativeTorque(transform.up * Time.fixedDeltaTime * -TurnSpeed * 0.3f);
                    break;
                case 7: // center center right
                    myRB.AddRelativeTorque(transform.up * Time.fixedDeltaTime * TurnSpeed * 0.3f);
                    break;
                default: // fuck
                    break;
            }
            //myRB.AddRelativeTorque(transform.up * Time.fixedDeltaTime * TurnSpeed * Mathf.Sin(1 * Time.fixedTime) * 0.1f);
        }
    }

    public void doIReverse()
    {
        reverseTimer -= Time.fixedDeltaTime;
        reverseCooldown -= Time.fixedDeltaTime;
        if (Physics.Raycast(transform.position + rayOffset, transform.forward, out reverseRay, 3))
        {
            reverseTimer = 2;
            reverse = true;
        }
        Debug.DrawRay(transform.position + rayOffset, transform.forward * reverseRay.distance, Color.blue);
        if (reverse && grounded)
        {
            reverseCooldown = 1f;
            myRB.AddForce(-transform.forward * Time.fixedDeltaTime * StandardSpeed);
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

    void FixedUpdate()
    {
        
        awarenessUpdate();
        if (thisTrack == null) return;

        doIReverse();
        if (!reverse && grounded) myRB.AddForce(transform.forward * Time.fixedDeltaTime * StandardSpeed * thisTrack.Waypoints[currentWaypoint].AiAccelerate * slowdown);
        doIturnUpdate();
        amIstuck();
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
