using UnityEngine;

public class Bubble_Countdown : MonoBehaviour
{
    public RectTransform healthCanvas;
    public RectTransform healthbar_fill;

    private float bubbleDuration;
    private float remaining;
    private bool  isCountingDown = false;
    private const float maxWidth = 0.8f;

    private void Start()
    {
        Anim_ChatBubble chatBubble = GetComponent<Anim_ChatBubble>() ?? GetComponentInParent<Anim_ChatBubble>();
        if (chatBubble != null && chatBubble.bulletPrefab != null)
        {
            Bullet b = chatBubble.bulletPrefab.GetComponent<Bullet>();
            if (b != null) { bubbleDuration = b.followDuration; Debug.Log($"bubbleDuration = {bubbleDuration}"); }
        }
    }

    private void Update()
    {
        if (!isCountingDown) return;

        remaining -= Time.deltaTime;

        if (remaining <= 0f)
        {
            remaining = 0f;
            isCountingDown = false;
        }

        float ratio = remaining / bubbleDuration;
        healthbar_fill.sizeDelta = new Vector2(ratio * maxWidth, healthbar_fill.sizeDelta.y);
    }

    public void StartCountdown()
    {
        remaining      = bubbleDuration;
        isCountingDown = true;
        Debug.Log($"StartCountdown called | bubbleDuration={bubbleDuration} | healthbar_fill={healthbar_fill} | healthCanvas={healthCanvas}");
    }
}
