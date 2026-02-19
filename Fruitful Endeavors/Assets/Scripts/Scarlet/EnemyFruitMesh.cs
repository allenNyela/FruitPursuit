using UnityEngine;


public class EnemyFruitMesh : MonoBehaviour
{
    [Tooltip("Drag fruit prefabs here")]
    public GameObject[] fruitPrefabs;

    private void Awake()
    {
        if (fruitPrefabs == null || fruitPrefabs.Length == 0)
        {
            Debug.LogWarning("EnemyFruitMesh: fruitPrefabs array is empty!", this);
            return;
        }

        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null) mr.enabled = false;

        GameObject chosen = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];
        Quaternion randomYRot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        Instantiate(chosen, transform.position, randomYRot, transform);
    }
}
