using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

public class PeachHeart : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] public int health = 100;
    public int dieHealth = 0;
    public int winHealth = 150;
    //[SerializeField] private SpriteRenderer sr;
    [SerializeField] GameObject gameOverscreen;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject UI;
    public GameObject vfx;

    private Color startColor;
    private Coroutine vfxCoroutine;
    private float vfxTimer;

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
        Enemy_Stats stats = collision.gameObject.GetComponentInChildren<Enemy_Stats>();
        if (stats == null || !stats.isBubbleTarget) {
            health -= collision.gameObject.GetComponent<Health>().damage;
            // sr.color = Color.gray;
            // GetComponent<SoundEffectPlayer>().PlaySoundEffect();
            StartCoroutine(showCastleDamage());

            Anim_Peach anim = GetComponentInChildren<Anim_Peach>();
            if (anim != null) anim.PlayHitAnim();
        } else
        {
            health += collision.gameObject.GetComponent<Health>().damage;
            ShowVfx();
        }
        
    }


    void ShowVfx()
    {
        if (vfx == null) return;
        vfxTimer = 2f;
        if (vfxCoroutine == null)
        {
            vfx.SetActive(true);
            vfxCoroutine = StartCoroutine(VfxTimeout());
        }
    }

    IEnumerator VfxTimeout()
    {
        while (vfxTimer > 0f)
        {
            vfxTimer -= Time.deltaTime;
            yield return null;
        }
        vfx.SetActive(false);
        vfxCoroutine = null;
    }

    public IEnumerator showCastleDamage()
    {
        yield return new WaitForSeconds(.2f);
        //sr.color = startColor;
    }


    public void TriggerGameOverScreen()
    {
        if (health <= dieHealth)
        {
            gameOverscreen.SetActive(true);
            UI.SetActive(false);
        } else if (health >= winHealth)
        {
            winScreen.SetActive(true);
            UI.SetActive(false);
        }

    }
}
