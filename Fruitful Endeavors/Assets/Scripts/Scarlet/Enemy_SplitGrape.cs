using UnityEngine;

public class Enemy_SplitGrape : MonoBehaviour
{
    public GameObject grape2Prefab;
    public int spawnCount = 4;

    public void Split()
    {
        Vector3 pos = transform.position;
        Enemy parentEnemy = transform.root.GetComponent<Enemy>();
        int currentWaypoint = parentEnemy != null ? parentEnemy.waypointIndex : 0;

        int spawned = 0;
        for (int i = 0; i < spawnCount; i++)
        {
            if (grape2Prefab == null) continue;
            Vector3 offset = new Vector3(Random.Range(-0.3f, 0.3f), 0f, Random.Range(-0.3f, 0.3f));
            GameObject g = Instantiate(grape2Prefab, pos + offset, Quaternion.identity);
            Enemy e = g.GetComponent<Enemy>();
            if (e != null) e.waypointIndex = currentWaypoint;
            spawned++;
        }
        Debug.Log($"[Split] spawned {spawned} grape2s");

        Destroy(transform.root.gameObject);
    }
}
