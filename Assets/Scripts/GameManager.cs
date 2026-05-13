using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float timerMinutes = 1f;
    [SerializeField] private float timerSeconds = 0f;
    [SerializeField] private float timerMiliseconds = 0f;
    [SerializeField] private float timer = 0f;
    [SerializeField] private bool isTimerRunning = false;

    [Header("UI References")]
    [SerializeField] private GameObject UI;
    [SerializeField] private TMP_Text timerText;

    [Header("Player Reference")]
    [SerializeField] private PlayerController player;


    private GameObject DeathScreen;
    private GameObject WinScreen;
    private GameObject PauseScreen;
    private GameObject GameplayScreen;

    private void Start()
    {
        float totalSeconds = (timerMinutes * 60f) + timerSeconds + (timerMiliseconds / 100f);
        timer = totalSeconds;
        UpdateTimerUI();

        DeathScreen = UI.transform.Find("DeathScreen").gameObject;
        WinScreen = UI.transform.Find("WinScreen").gameObject;
        PauseScreen = UI.transform.Find("PauseScreen").gameObject;
        PauseScreen = UI.transform.Find("GameplayScreen").gameObject;

        DeathScreen.SetActive(false);
        WinScreen.SetActive(false);
        PauseScreen.SetActive(false);
        GameplayScreen.SetActive(true);
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            timer -= Time.deltaTime;
            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer % 60F);
        int milliseconds = Mathf.FloorToInt((timer * 100F) % 100F);
        timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        timer = 0f;
        UpdateTimerUI();
    }
    public void Pause()
    {
        StopTimer();
        PauseScreen.SetActive(true);
    }
    public void Unpause()
    {
        StartTimer();
        PauseScreen.SetActive(false);
    }
    public void Death()
    {
        StopTimer();
        DeathScreen.SetActive(true);
    }
    public void Win()
    {
        StopTimer();
        WinScreen.SetActive(true);
    }
    
}
