using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class TimeManager : MonoBehaviour
{

    [Header("Time Settings")]
    [SerializeField] float maxTime;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI timeText;

    private float currentTime;
    private bool isRunning = true;

    void Start()
    {
        currentTime = maxTime;
        UpdateTimeUI();
    }

    private void Awake()
    {
        Invoke("FindTimeText", 0.1f);
        currentTime = maxTime;
        isRunning = true;
        UpdateTimeUI();
    }

    void FindTimeText()
    {
        if (timeText == null)
        {
            GameObject textObj = GameObject.Find("Canvas/Time/TimeCount");
            if (textObj != null)
            {
                timeText = textObj.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                Debug.LogError("Không tìm thấy TimeCount! Hãy kiểm tra lại.");
            }
        }
    }

    void Update()
    {
        if (isRunning)
        {
            UpdateTimer();
        }
    }

    private void UpdateTimer()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimeUI();
        }
        else
        {
            StopTimer();
        }
    }

    private void UpdateTimeUI()
    {
        if (timeText != null)
        {
            timeText.text = "Time: " + GetTime().ToString()+ "s";
        }
    }

    public int GetTime() => (int) currentTime;

    public float GetCurrentTime()
    {
        return maxTime - currentTime; // Thời gian hoàn thành
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
