using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public float baseSpeed = 10f;
    public float currSpeed;

    public float bobAmplitude = 0.15f;
    public float bobSpeed = 2f;

    private Transform target;
    private int waypointIndex = 0;
    private float baseY;

    private void Start()
    {
        target = Waypoint.points[0];
        currSpeed = baseSpeed;
        baseY = transform.position.y;
    }

    private void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * currSpeed * Time.deltaTime, Space.World);

        // Apply gentle vertical bob on top of path movement
        Vector3 pos = transform.position;
        pos.y = baseY + (Mathf.Sin(Time.time * bobSpeed) + 1f) / 2f * bobAmplitude;
        transform.position = pos;

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
