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
        // 路径：同层级 Anim_ChatBubble → bulletPrefab → Bullet.followDuration
        Anim_ChatBubble chatBubble = GetComponent<Anim_ChatBubble>();
        if (chatBubble != null && chatBubble.bulletPrefab != null)
        {
            Bullet b = chatBubble.bulletPrefab.GetComponent<Bullet>();
            if (b != null) bubbleDuration = b.followDuration;
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

    // 在 bullet 命中时调用此方法启动倒计时
    public void StartCountdown()
    {
        remaining      = bubbleDuration;
        isCountingDown = true;
    }
}
