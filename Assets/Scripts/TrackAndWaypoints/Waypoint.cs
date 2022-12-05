using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public float AiAccelerate = 1f;

    private void OnTriggerEnter(Collider other)
    {
        AiCar findAI = other.GetComponent<AiCar>();
        if ( findAI != null )
        { // this is an AI car
            if ( findAI.thisTrack.Waypoints[findAI.currentWaypoint] == this)
                findAI.currentWaypoint++;
            if (findAI.currentWaypoint >= findAI.thisTrack.Waypoints.Length)
                findAI.currentWaypoint = 0;
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
