using UnityEngine;

public class Healthbar_Enemy : MonoBehaviour
{
    public RectTransform healthCanvas;
    public RectTransform healthbar_fill;

    private Health health;
    private float canvasWidth;

    private void Start()
    {
        health = GetComponentInParent<Health>();
        canvasWidth = healthCanvas.rect.width;

        EnemyFruitHeight fruitHeight = health.GetComponentInChildren<EnemyFruitHeight>();

        if (fruitHeight != null)
        {
            Vector3 pos = healthCanvas.localPosition;
            pos.y = fruitHeight.height;
            healthCanvas.localPosition = pos;
        }
    }

    private void Update()
    {
        healthCanvas.forward = Camera.main.transform.forward;

        float ratio = (float)health.CurrentHealth / health.maxHealth;
        healthbar_fill.sizeDelta = new Vector2(ratio * canvasWidth, healthbar_fill.sizeDelta.y);
    }
}
