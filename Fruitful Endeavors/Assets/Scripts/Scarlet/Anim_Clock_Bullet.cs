using UnityEngine;

public class Anim_Clock_Bullet : MonoBehaviour
{
    public GameObject bullet_Prefab;
    public float spinSpeed = 180f;

    void Update()
    {
        if (bullet_Prefab != null)
            bullet_Prefab.transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f,  Space.Self);
    }
}
