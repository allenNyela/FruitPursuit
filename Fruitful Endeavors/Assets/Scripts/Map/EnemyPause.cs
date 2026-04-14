using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPause : MonoBehaviour
{

    public List<Transform> enemies = new List<Transform>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pauseAllEnemies()
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].GetComponent<Enemy>().pauseSpeed();
            }            
        }
    }

    public void resumeAllEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].GetComponent<Enemy>().resumeSpeed();
            }
        }
    }
}
