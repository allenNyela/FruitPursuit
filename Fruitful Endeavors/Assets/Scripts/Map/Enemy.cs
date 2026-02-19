using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public float baseSpeed = 10f;
    public float currSpeed;

    private Transform target;
    private int waypointIndex = 0;

    private void Start()
    {
        target = Waypoint.points[0];
        currSpeed = baseSpeed;
    }

    private void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * currSpeed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= .2f)
        {
            GetNextWayPoint();
        }
    }

    void GetNextWayPoint()
    {
        if (waypointIndex >= Waypoint.points.Length - 1) 
        { 
            Destroy(gameObject);
            return;
        }
        waypointIndex++;
        target = Waypoint.points[waypointIndex];
    }
}
