using UnityEngine;

public class Click_Sound : MonoBehaviour
{
    public AudioClip clickClip;
    public float volume = 1f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (clickClip != null)
                AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position, volume);
        }
    }
}
