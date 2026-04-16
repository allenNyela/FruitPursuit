using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [Header("References")]
    public TMP_Text timeText;
    public Image slider;

    private bool startTimer = false;

    public float timeStart = 5;
    float time;
    float multiplierFactor;

    [Space]
    public UnityEvent OnComplete;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTimer = true;
        time = timeStart;
        multiplierFactor = 1f / timeStart;
        slider.fillAmount = time * multiplierFactor;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startTimer) return;

        if (time > 0)
        {
            time -= Time.deltaTime;
            timeText.text = Mathf.CeilToInt(time).ToString();
            slider.fillAmount = time * multiplierFactor;
        } else
        {
            startTimer = false;
            ResetTimer();
            OnComplete?.Invoke();
        }
    }

    public void StartTimer()
    {
        startTimer = true;
    }

    public void ResetTimer()
    {
        startTimer = true;
        time = timeStart;
        multiplierFactor = 1f / timeStart;
        timeText.text = Mathf.CeilToInt(time).ToString();
        slider.fillAmount = time * multiplierFactor;
    }
}
