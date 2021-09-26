using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Each children is one waypoint
/// </summary>
public class WayPointPath : MonoBehaviour {
    
    /// <summary>
    /// dynmacly assigned waypoints
    /// </summary>
    [HideInInspector]
    public List<Vector3> Waypoints;

	// Use this for initialization
	void Start ()
    {
        RefreshChilden();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDrawGizmos()
    {
        RefreshChilden();

        //draw a black connection o------o
        Gizmos.color = Color.black;
        for (int i = 0; i < Waypoints.Count; i++)
        {
            if (i == 0)
                Gizmos.DrawLine(Waypoints[0], Waypoints[Waypoints.Count - 1]);
            else
                Gizmos.DrawLine(Waypoints[i - 1], Waypoints[i]);
            Gizmos.DrawSphere(Waypoints[i], 0.1f);
        }
    }

    void OnTransformChildrenChanged()
    {
        RefreshChilden();
    }

    private void RefreshChilden()
    {
        //clear waypoints
        if (Waypoints == null)
            Waypoints = new List<Vector3>();
        Waypoints.Clear();

        //add every child transform to waypoints
        foreach(var child in GetComponentsInChildren<Transform>())
        {
            if (child == transform)
                continue;
            Waypoints.Add(child.position);
        }

    }
}
