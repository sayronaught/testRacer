using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public float AiAccelerate = 1f;

    private void OnTriggerStay(Collider other)
    {
        AiCar findAI = other.GetComponent<AiCar>();
        if ( findAI != null )
        { // this is an AI car
            if ( findAI.thisTrack.Waypoints[findAI.currentWaypoint] == this)
                findAI.currentWaypoint++;
            if (findAI.currentWaypoint >= findAI.thisTrack.Waypoints.Length)
                findAI.currentWaypoint = 0;
            findAI.myWaypoint = findAI.thisTrack.Waypoints[findAI.currentWaypoint].transform;
        }
        navAiCar findNavAI = other.GetComponent<navAiCar>();
        if (findNavAI != null)
        { // this is an AI car
            if (findNavAI.thisTrack.Waypoints[findNavAI.currentWaypoint] == this)
                findNavAI.currentWaypoint++;
            if (findNavAI.currentWaypoint >= findNavAI.thisTrack.Waypoints.Length)
                findNavAI.currentWaypoint = 0;
            findNavAI.myWaypoint = findNavAI.thisTrack.Waypoints[findNavAI.currentWaypoint].transform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
