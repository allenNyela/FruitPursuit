using System.Threading;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attritbute")]
    [SerializeField] private int health = 12;
    [HideInInspector] public int maxHealth;
    public int CurrentHealth => health;
    [SerializeField] private int currencyWorth = 10;
    [SerializeField] public bool shielded = false;
    [SerializeField] public bool speedChanged = false;
    [SerializeField] public float shieldCountdown = 2f;
    [SerializeField] public float speedCountdown = 2f;
    [SerializeField] public int damage = 10;
    [SerializeField] public GameObject deathVfx;

    private bool isDestroyed = false;

    private void Awake()
    {
        maxHealth = health;
    }

    private void Start()
    {
        Enemy_Stats stats = GetComponentInChildren<Enemy_Stats>();
        if (stats != null) { health = stats.health; maxHealth = stats.health; damage = stats.damage; }
    }

    void Update()
    {
        if (shielded && shieldCountdown >= 0)
        {
            shieldCountdown -= Time.deltaTime;
        } else if (shieldCountdown <= 0) 
        {
            shielded = false;
        }
        if (speedChanged && speedCountdown >= 0)
        {
            speedCountdown -= Time.deltaTime;
        }
        else if (speedCountdown <= 0)
        {
            speedChanged = false;
            this.gameObject.GetComponent<Enemy>().currSpeed = this.gameObject.GetComponent<Enemy>().baseSpeed;
        }

    }

    public void TakeDamage(int dmg)
    {
        if (!shielded)
        {
            health -= dmg;
            //Debug.Log(health);
        }     

        if (health <= 0 && !isDestroyed) KillEnemy();
    }

    public void GiveHealth(int amt)
    {
        health += amt;
        if (health >= maxHealth) health = maxHealth;
        if (health <= 0 && !isDestroyed) KillEnemy();
    }

    void KillEnemy()
    {
        // GetComponent<SoundEffectPlayer>().PlayNow();
        if (deathVfx != null)
        {
            Enemy enemy = GetComponent<Enemy>();
            Vector3 spawnPos = enemy != null ? enemy.AimPosition : transform.position;
            //Debug.Log($"[KillEnemy] Spawning VFX at {spawnPos}");
            GameObject vfxInstance = Instantiate(deathVfx, spawnPos, Quaternion.identity);
            ParticleSystem ps = vfxInstance.GetComponentInChildren<ParticleSystem>();
            float duration = ps != null ? ps.main.duration + ps.main.startLifetime.constantMax : 2f;
            Destroy(vfxInstance, duration);
        }
        else
        {
            Debug.LogWarning("[KillEnemy] deathVfx is null — assign it in the Inspector on the enemy prefab");
        }
        WaveSpawner.onEnemyDestroy.Invoke();
        if (this.gameObject.GetComponent<Enemy>().type == Enemy.EnemyType.Unwanted)
        {
            LevelManager.main.IncreaseCurrency(currencyWorth);
        }      
        isDestroyed = true;
        Enemy_SplitGrape split = GetComponentInChildren<Enemy_SplitGrape>();
        if (split != null) split.Split();
        else Destroy(gameObject);
    }

    public void ChangeSpeed(float tempSpeed)
    {
        this.gameObject.GetComponent<Enemy>().currSpeed = tempSpeed;
    }
}
