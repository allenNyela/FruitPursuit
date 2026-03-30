using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    public float speed = 50f;
    [SerializeField] private int bulletDamage = 10;
    [SerializeField] public BulletType type;
    [SerializeField] public float shieldTimer = 5f;
    [SerializeField] public float speedReducTimer = 5f;
    public float slowdownFactor = 0.333f;

    public bool  followOnHit    = false;
    public float followDuration = 3f;
    public float overlapRadius  = 1.5f;
    private bool isFollowing    = false;

    private static readonly System.Collections.Generic.List<Bullet> followingBullets = new();

    private void OnDestroy() { followingBullets.Remove(this); }

    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) { Destroy(gameObject); return; }

        Enemy_Stats efh = target.GetComponentInChildren<Enemy_Stats>();
        float h = efh != null ? efh.height * 0.5f : 0f;
        Vector3 targetPos = target.position + new Vector3(0f, h, 0f);

        if (isFollowing)
        {
            transform.position = targetPos;
            return;
        }

        Vector3 dir = targetPos - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.rotation = Quaternion.LookRotation(dir.normalized);
    }

    void HitTarget()
    {
        if (type == BulletType.Shield)
        {
            target.gameObject.GetComponent<Health>().shielded = true;
            target.gameObject.GetComponent<Health>().shieldCountdown = shieldTimer;
        } else if (type == BulletType.Healing)
        {
            target.gameObject.GetComponent<Health>().GiveHealth(bulletDamage);
        } else if (type == BulletType.Attack)
        {
            target.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);
        } else if (type == BulletType.Slowdown)
        {
            target.gameObject.GetComponent<Health>().ChangeSpeed(target.GetComponent<Enemy>().baseSpeed * slowdownFactor);
            target.gameObject.GetComponent<Health>().speedChanged = true;
            target.gameObject.GetComponent<Health>().speedCountdown = speedReducTimer;
        }
        if (followOnHit)
        {
            foreach (Bullet other in followingBullets)
            {
                if (Vector3.Distance(transform.position, other.transform.position) < overlapRadius)
                {
                    Destroy(gameObject);
                    return;
                }
            }
            isFollowing = true;
            followingBullets.Add(this);
            Destroy(gameObject, followDuration);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public enum BulletType
    {
        Shield,
        Attack,
        Healing,
        Slowdown
    }
}
