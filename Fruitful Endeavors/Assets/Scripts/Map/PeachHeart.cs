using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

public class PeachHeart : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int health = 100;
    //[SerializeField] private SpriteRenderer sr;
    [SerializeField] GameObject gameOverscreen;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject UI;

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
        if (!(collision.gameObject.GetComponent<EnemyFruitMesh>().chosenPrefab == 3)) {
            health -= collision.gameObject.GetComponent<Health>().damage;
            // sr.color = Color.gray;
            // GetComponent<SoundEffectPlayer>().PlaySoundEffect();
            StartCoroutine(showCastleDamage());

            Anim_Peach anim = GetComponentInChildren<Anim_Peach>();
            if (anim != null) anim.PlayHitAnim();
        } else
        {
            health += collision.gameObject.GetComponent<Health>().damage;
        }
        
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
            UI.SetActive(false);
        } else if (health >= 150)
        {
            winScreen.SetActive(true);
            UI.SetActive(false);
        }

    }
}
