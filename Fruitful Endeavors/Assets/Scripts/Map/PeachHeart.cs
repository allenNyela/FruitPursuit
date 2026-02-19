using UnityEngine;
using System.Collections;

public class PeachHeart : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int health = 100;
    //[SerializeField] private SpriteRenderer sr;
    [SerializeField] GameObject gameOverscreen;

    private Color startColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //startColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        TriggerGameOverScreen();
    }

    private void OnCollisionEnter(Collision collision)
    {
        health -= collision.gameObject.GetComponent<Health>().damage;
       // sr.color = Color.gray;
       // GetComponent<SoundEffectPlayer>().PlaySoundEffect();
        StartCoroutine(showCastleDamage());
    }


    public IEnumerator showCastleDamage()
    {
        yield return new WaitForSeconds(.2f);
        //sr.color = startColor;
    }


    public void TriggerGameOverScreen()
    {
        if (health <= 0)
        {
            gameOverscreen.SetActive(true);
        }

    }
}
