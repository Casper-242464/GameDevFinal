using UnityEngine;
using TMPro;

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
    [SerializeField] private float abyssYThreshold = -6f; // Порог бездны приподняли для надежности

    private void Start()
    {
        float totalSeconds = (timerMinutes * 60f) + timerSeconds + (timerMiliseconds / 100f);
        time = totalSeconds;
        timer = time;
        UpdateTimerUI();
        
        isTimerRunning = true; // ТАЙМЕР ВКЛЮЧАЕТСЯ СРАЗУ АВТОМАТИЧЕСКИ ПРИ СТАРТЕ!
        Time.timeScale = 1f;

        if (player == null)
        {
            player = Object.FindFirstObjectByType<PlayerController>();
        }
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            timer -= Time.deltaTime;
            UpdateTimerUI();
        }

        // Пауза
        if (Input.GetKeyDown(KeyCode.Escape) && menuManager != null)
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

        // Проверка падения в бездну или отсутствия здоровья
        bool isDeadByAbyss = (player != null && player.transform.position.y < abyssYThreshold);
        bool isDeadByHealth = (player != null && player.health <= 0f);

        if (timer <= 0f || isDeadByHealth || isDeadByAbyss)
        {
            StopTimer();
            timer = 0f;
            if (player != null) player.health = 0f;
            
            if (menuManager != null)
            {
                menuManager.ActivateDeathScreen();
            }
        }

        // Проверка победы
        if (player != null && player.winState)
        {
            StopTimer();
            if (menuManager != null)
            {
                menuManager.ActivateWinScreen();
            }
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer % 60F);
        int milliseconds = Mathf.FloorToInt((timer * 100F) % 100F);
        timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }

    public void StartTimer() { isTimerRunning = true; }
    public void StopTimer() { isTimerRunning = false; }
    public void ResetTimer()
    {
        timer = time;
        UpdateTimerUI();
    }
}