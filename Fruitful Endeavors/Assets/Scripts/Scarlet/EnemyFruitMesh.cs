using UnityEngine;


public class EnemyFruitMesh : MonoBehaviour
{
    [Tooltip("Drag fruit prefabs here")]
    public GameObject[] fruitPrefabs;
    [Tooltip("Vertical offset to place fruit on the ground")]
    public float yOffset = 0f;
    public int chosenPrefab;

    private void Awake()
    {
        if (fruitPrefabs == null || fruitPrefabs.Length == 0)
        {
            Debug.LogWarning("EnemyFruitMesh: fruitPrefabs array is empty!", this);
            return;
        }

        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null) mr.enabled = false;
        chosenPrefab = Random.Range(0, fruitPrefabs.Length);
        GameObject chosen = fruitPrefabs[chosenPrefab];
        bool hasAnim = chosen.GetComponent<Animator>() != null || chosen.GetComponentInChildren<Animator>() != null;
        Quaternion rot = hasAnim ? Quaternion.identity : Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        Vector3 spawnPos = transform.position + new Vector3(0f, yOffset, 0f);
        Instantiate(chosen, spawnPos, rot, transform);
    }
}
