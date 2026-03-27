using UnityEngine;

public class Healthbar_Peach : MonoBehaviour
{
    public RectTransform healthCanvas;
    public RectTransform healthbar_fill;

    private PeachHeart peachHeart;
    private const float maxWidth = 0.8f;
    private float maxHealth;
    private float fillHeight;

    private void Start()
    {
        peachHeart = GetComponent<PeachHeart>();
        maxHealth = peachHeart.winHealth;
        fillHeight = healthbar_fill.sizeDelta.y != 0 ? healthbar_fill.sizeDelta.y : healthCanvas.sizeDelta.y;
    }

    private void Update()
    {
        float ratio = Mathf.Clamp01(peachHeart.health / maxHealth);
        healthbar_fill.sizeDelta = new Vector2(ratio * maxWidth, fillHeight);
        //Debug.Log(healthbar_fill.sizeDelta);
    }
}
