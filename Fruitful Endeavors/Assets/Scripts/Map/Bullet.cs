using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    public float speed = 50f;
    [SerializeField] private int bulletDamage = 10;
    [SerializeField] public BulletType type;
    [SerializeField] public float shieldTimer = 5f;
    [SerializeField] public float speedReducTimer = 5f;
    [SerializeField] public float newSpeed = 1f;

    public bool  followOnHit    = false;
    public float followDuration = 3f;
    private bool isFollowing    = false;

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

        if (isFollowing)
        {
            Vector3 offset = Vector3.zero;
            if (type == BulletType.Shield)
            {
                EnemyFruitHeight efh = target.GetComponentInChildren<EnemyFruitHeight>();
                float h = efh != null ? efh.height : 1f;
                offset = new Vector3(0f, h * 0.5f, 0f);
            }
            transform.position = target.position + offset;
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

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
            target.gameObject.GetComponent<Health>().ChangeSpeed(newSpeed);
            target.gameObject.GetComponent<Health>().speedChanged = true;
            target.gameObject.GetComponent<Health>().speedCountdown = speedReducTimer;
        }
        if (followOnHit)
        {
            isFollowing = true;
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
