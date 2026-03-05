using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
    float updateInterval = 1.0f;   //当前时间间隔
    private float accumulated = 0.0f;  //在此期间累积 
    private float frames = 0;    //在间隔内绘制的帧 
    private float timeRemaining;   //当前间隔的剩余时间
    private float fps = 15.0f;    //当前帧 Current FPS
    private float lastSample;
    public string appVersion;
    public long delayTime; //延迟
    public bool showFPS = false;
    public bool showPING = false;
    public TMP_Text fpsText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ++frames;
        float newSample = Time.realtimeSinceStartup;
        float deltaTime = newSample - lastSample;
        lastSample = newSample;
        timeRemaining -= deltaTime;
        accumulated += 1.0f / deltaTime;

        if (timeRemaining <= 0.0f)
        {
            fps = accumulated / frames;
            timeRemaining = updateInterval;
            accumulated = 0.0f;
            frames = 0;
        }

        fpsText.text = $"FPS: {fps.ToString("f0")}";
    }
}
