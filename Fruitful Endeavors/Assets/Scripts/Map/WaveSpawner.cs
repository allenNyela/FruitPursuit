using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public Transform[] enemyPrefab;

    public Transform spawnPoint;

    [SerializeField] public DialogueFunctions dialogueFunc;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = .5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = .75f;
    [SerializeField] public EnemyPause enemyPause;

    //public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    private int waveNumber = 1;
    //int numOfEnemies;

    private float timeSinceLastSpawn;
    private int enemiesAlive;
    public int enemiesLeftToSpawn;
    private bool isSpawning = false;
    private bool startCountdown = false;
    public TMP_Text countdownText;
    public GameObject startButton;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();
    public static UnityEvent onEnemyAdded = new UnityEvent();

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
        onEnemyAdded.AddListener(EnemyAdded);
    }

    private void Start()
    {
        //StartCoroutine(StartWave());
    }

    public void startRound()
    {
        startButton.SetActive(false);
        StartCoroutine(StartWave());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }

        //if(startCountdown)
        //{
        //    countdownText.text = $"{timeBetweenWaves.ToString("f0")}";
        //    if ()
        //}
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemyPerWave();
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        waveNumber++;
        StartCoroutine(StartWave());
    }

    //IEnumerator SpawnWave()
    //{
    //    numOfEnemies = waveNumber * waveNumber + 1;
    //    for (int i = 0; i < numOfEnemies; i++) 
   //     {
    //        SpawnEnemy();
    //        yield return new WaitForSeconds(.5f);
    //    }
    //    waveNumber++;
    //}

    public void SpawnEnemy()
    {
        int randomInt = Random.Range(0, enemyPrefab.Length);
        enemyPause.enemies.Add(Instantiate(enemyPrefab[0], spawnPoint.position, spawnPoint.rotation));
        //Instantiate(enemyPrefab[0], spawnPoint.position, spawnPoint.rotation);
    }

    private int EnemyPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(waveNumber, difficultyScalingFactor));
    }

    private void EnemyAdded()
    {
        enemiesAlive++;
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
        if (dialogueFunc != null)
        {
            dialogueFunc.TutorialTimeline.Resume();
        }
    }

    public void SpawnEnemy1()
    {
        enemyPause.enemies.Add(Instantiate(enemyPrefab[1], spawnPoint.position, spawnPoint.rotation));
    }

    public void SpawnEnemy2()
    {
        enemyPause.enemies.Add(Instantiate(enemyPrefab[0], spawnPoint.position, spawnPoint.rotation));
    }

    public void SpawnBananaSuitor()
    {
        enemyPause.enemies.Add(Instantiate(enemyPrefab[2], spawnPoint.position, spawnPoint.rotation));
    }
}
