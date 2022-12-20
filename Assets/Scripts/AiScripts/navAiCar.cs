using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navAiCar : MonoBehaviour
{
    public Track thisTrack;
    public int currentWaypoint = 0;
    public Transform myWaypoint;

    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = thisTrack.Waypoints[currentWaypoint].transform.position;
    }
}
