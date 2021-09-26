using UnityEngine;
/*
 * Moving Platform
 * 
 * 1. Attach WayPointPath to an empty game object
 * 2. Create empty game object children (these will be handled as waypoints)
 * 3. Attach Platform component to a game object and assign the WayPointPath
 * 
 * 
 **/


/// <summary>
/// Moving Platform
/// </summary>
public class Platform : MonoBehaviour
{
    /// <summary>
    /// Path
    /// </summary>
    public WayPointPath Path;

    /// <summary>
    /// optional: Curve
    /// </summary>
    public AnimationCurve Animation;

    /// <summary>
    /// Moving Speed
    /// </summary>
    public float Speed = 1;

    /// <summary>
    /// Current Target
    /// </summary>
    protected int WaypointIndex;

    // Use this for initialization
    void Start()
    {
        if(Path != null && Path.Waypoints.Count > 0)
            transform.position = Path.Waypoints[0];
        if (Animation.keys.Length == 0)
            Animation = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {

        //follow waypoint path
        if (Path != null)
        {
            var prevTaget = (WaypointIndex > 0) ? Path.Waypoints[WaypointIndex - 1] : Path.Waypoints[Path.Waypoints.Count - 1];
            var currTarget = Path.Waypoints[WaypointIndex];
            var completeLength = Vector3.Distance(prevTaget, currTarget);
            var toTarget = (currTarget - transform.position);
            var goTo = toTarget.normalized * Time.deltaTime * Animation.Evaluate(toTarget.magnitude / completeLength) * Speed;
            if (toTarget.magnitude < 0.01f || goTo.magnitude > toTarget.magnitude)
            {
                goTo = goTo.normalized * toTarget.magnitude;
                WaypointIndex = (WaypointIndex + 1) % Path.Waypoints.Count;
            }
            transform.position += goTo;

        }
    }
}
