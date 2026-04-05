using System.Collections;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    public enum LevelSelect { Level1, Level2 }

    public LevelSelect selectedLevel = LevelSelect.Level1;
    public GameObject pathingPrefab;
    public float speed = 12f;

    [Header("Level 1")]
    public Transform spawnPoint1;
    public Transform waypointParent1;

    [Header("Level 2")]
    public Transform spawnPoint2;
    public Transform waypointParent2A;

    private void Start()
    {
        if (selectedLevel == LevelSelect.Level1)
            StartCoroutine(RunPath(spawnPoint1, waypointParent1));
        else
            StartCoroutine(RunPath(spawnPoint2, waypointParent2A));
    }

    private IEnumerator RunPath(Transform spawnPoint, Transform waypointParent)
    {
        yield return null;

        if (waypointParent == null) yield break;

        int count = waypointParent.childCount;
        if (count == 0) yield break;

        Vector3 startPos = spawnPoint != null ? spawnPoint.position : waypointParent.GetChild(0).position;
        GameObject cube = Instantiate(pathingPrefab, startPos, Quaternion.identity);

        TrailRenderer trail = cube.GetComponentInChildren<TrailRenderer>();
        if (trail != null) trail.emitting = false;
        yield return null;
        trail?.Clear();
        if (trail != null) trail.emitting = true;


        for (int i = 0; i < count; i++)
        {
            Vector3 target = waypointParent.GetChild(i).position;
            while (Vector3.Distance(cube.transform.position, target) > 0.05f)
            {
                cube.transform.position = Vector3.MoveTowards(cube.transform.position, target, speed * Time.deltaTime);
                yield return null;
            }
            cube.transform.position = target;
        }

        Destroy(cube);
    }
}
