using UnityEngine;
using System;

[Serializable]

public class Turret : MonoBehaviour
{
    public Transform target;

    [Header("Attributes")]
    public float range = 15f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    public float turnSpeed = 10f;

    [Header("References")]
    public Transform partToRotate;
    public GameObject turretPrefab;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public GameObject rangeSphere;
    public Plot plot;

    public string enemyTag = "Enemy";  
    
    
    

    public Turret (GameObject prefab)
    {
        turretPrefab = prefab;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Anim_ChatBubble anim;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, .5f);
        anim = GetComponent<Anim_ChatBubble>();
        if (rangeSphere != null)
        {
            float parentScale = transform.lossyScale.x;
            rangeSphere.transform.localScale = Vector3.one * range * 2f / parentScale;
        }
    }

    [HideInInspector] public bool isPreview = false;

    public void ShowRange(bool show)
    {
        if (rangeSphere != null) rangeSphere.SetActive(show);
    }

    public float hoverPixelRadius = 2500f;
    private bool wasHovered = false;

    void CheckHover()
    {
        if (isPreview) return;
        if (Camera.main == null) { Debug.LogError("Camera.main is null!"); return; }
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        float dist = Vector2.Distance(new Vector2(screenPos.x, screenPos.y), new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        bool hovered = dist < hoverPixelRadius;
        //Debug.Log($"dist: {dist}, hovered: {hovered}");
        if (hovered != wasHovered)
        {
            wasHovered = hovered;
            ShowRange(hovered);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckHover();
        if (target == null) { return; }

        if (anim == null || anim.isTargeting)
        {
            Vector3 dir = transform.position - target.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }

        if (fireCountdown <= 0f)
        {
            if (anim != null && bulletPrefab.GetComponent<Bullet>().type == Bullet.BulletType.Shield)
            {
                return;
            }
            if ((!(target.gameObject.GetComponent<EnemyFruitMesh>().chosenPrefab == 2)) && (bulletPrefab.GetComponent<Bullet>().type == Bullet.BulletType.Shield || bulletPrefab.GetComponent<Bullet>().type == Bullet.BulletType.Healing))
            {
                return;
            } else
            {
                if (bulletPrefab.GetComponent<Bullet>().type == Bullet.BulletType.Shield && target.GetComponent<Health>().shielded) {
                    return;
                }
                Shoot();
                fireCountdown = 1f / fireRate;
            }                    
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        //Debug.Log("Shoot!");
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    void UpdateTarget() 
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance) 
            { 
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range) 
        { 
            target = nearestEnemy.transform;
        } else
        {
            target = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
