using System.Threading;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attritbute")]
    [SerializeField] private int health = 12;
    [SerializeField] private int currencyWorth = 10;
    [SerializeField] public bool shielded = false;
    [SerializeField] public bool speedChanged = false;
    [SerializeField] public float shieldCountdown = 2f;
    [SerializeField] public float speedCountdown = 2f;
    [SerializeField] public int damage = 10;

    private bool isDestroyed = false;

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
        }     

        if (health <= 0 && !isDestroyed)
        {

           // GetComponent<SoundEffectPlayer>().PlayNow();
            WaveSpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(currencyWorth);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }

    public void GiveHealth(int amt)
    {
        health += amt;

        if (health <= 0 && !isDestroyed)
        {

            // GetComponent<SoundEffectPlayer>().PlayNow();
            //EnemySpawner.onEnemyDestroy.Invoke();
            //LevelManager.main.IncreaseCurrency(currencyWorth);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }

    public void ChangeSpeed(float tempSpeed)
    {
        this.gameObject.GetComponent<Enemy>().currSpeed = tempSpeed;
    }
}
