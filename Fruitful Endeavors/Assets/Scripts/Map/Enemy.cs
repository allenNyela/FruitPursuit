using UnityEngine;
using UnityEngine.UIElements;
using static Bullet;

public class Enemy : MonoBehaviour
{
    public float baseSpeed = 10f;
    public float currSpeed;

    public float bobAmplitude = 0.15f;
    public float bobSpeed = 2f;

    [Header("Anim")]
    public float animSpeedAmp = 0.2f;
    public float anim_speed_standard = 1.8f;

    private Transform target;
    private int waypointIndex = 0;
    private float baseY;
    private Animator animator;
    private bool hasSpeedParam;

    [SerializeField] public EnemyType type;

    private void Start()
    {
        target = Waypoint.points[0];
        currSpeed = baseSpeed;
        baseY = transform.position.y;

        animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            foreach (var p in animator.parameters)
            {
                if (p.name == "speed_multiplier") { hasSpeedParam = true; break; }
            }
        }
    }

    private void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * currSpeed * Time.deltaTime, Space.World);
        if (animator != null)
        {
            Vector3 flatDir = new Vector3(dir.x, 0f, dir.z);
            if (flatDir != Vector3.zero)
                transform.forward = flatDir.normalized;
        }

        if (animator != null && hasSpeedParam)
        {
            float speedMult = anim_speed_standard + (currSpeed - 5f) * animSpeedAmp;
            animator.SetFloat("speed_multiplier", speedMult);
        }

        if (animator == null)
        {
            Vector3 pos = transform.position;
            pos.y = baseY + (Mathf.Sin(Time.time * bobSpeed) + 1f) / 2f * bobAmplitude;
            transform.position = pos;
        }

        if (Vector3.Distance(transform.position, target.position) <= .2f)
        {
            GetNextWayPoint();
        }
    }

    void GetNextWayPoint()
    {
        if (waypointIndex >= Waypoint.points.Length - 1) 
        {
            WaveSpawner.onEnemyDestroy.Invoke();
            Destroy(gameObject);
            return;
        }
        waypointIndex++;
        target = Waypoint.points[waypointIndex];
    }

    public enum EnemyType
    {
        Unwanted,
        Wanted
    }
}
