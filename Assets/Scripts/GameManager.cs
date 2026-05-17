using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float timerMinutes = 1f;
    [SerializeField] private float timerSeconds = 0f;
    [SerializeField] private float timerMiliseconds = 0f;
    [SerializeField] private float time = 0f;
    [SerializeField] private float timer;
    [SerializeField] private bool isTimerRunning = false;

    [Header("UI References")]
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private TMP_Text timerText;

    [Header("Player Reference")]
    [SerializeField] private PlayerController player;

    private void Start()
    {
        float totalSeconds = (timerMinutes * 60f) + timerSeconds + (timerMiliseconds / 100f);
        time = totalSeconds;
        timer = time;
        UpdateTimerUI();
        isTimerRunning = false;
    }

    private void Update()
    {
        if (!isTimerRunning)
        {
            if (Input.anyKeyDown)
            {
                StartTimer();
            }
        } else
        {
            timer -= Time.deltaTime;
            UpdateTimerUI();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isTimerRunning)
            {
                menuManager.Pause();
                StopTimer();
            }
            else
            {
                menuManager.Unpause();
                StartTimer();
            }
        }

        if (timer <= 0f || player.health <= 0f)
        {
            StopTimer();
            timer = 0f;
            menuManager.ActivateDeathScreen();
        }

        if (player.winState)
        {
            StopTimer();
            menuManager.ActivateWinScreen();
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
        timer = time;
        UpdateTimerUI();
    }
}
