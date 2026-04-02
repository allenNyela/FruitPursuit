using System.Collections;
using UnityEngine;

public class Enemy_Grape2Stats : MonoBehaviour
{
    public float maxRandomDelay = 0.5f;
    public float minSpeed = 8f;
    public float maxSpeed = 14f;

    void Start()
    {
        float delay = Random.Range(0f, maxRandomDelay);
        StartCoroutine(DelayStart(delay));
    }

    IEnumerator DelayStart(float delay)
    {
        Enemy enemy = GetComponent<Enemy>();
        if (enemy != null) enemy.enabled = false;
        if (delay > 0f) yield return new WaitForSeconds(delay);
        else yield return null;
        if (enemy != null)
        {
            enemy.enabled = true;
            float speed = Random.Range(minSpeed, maxSpeed);
            enemy.baseSpeed = speed;
            enemy.currSpeed = speed;
        }
    }
}
