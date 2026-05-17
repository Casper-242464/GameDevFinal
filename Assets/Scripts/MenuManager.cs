using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject UI;

    private GameObject DeathScreen;
    private GameObject WinScreen;
    private GameObject PauseScreen;
    private GameObject GameplayScreen;

    private void Start()
    {
        DeathScreen = UI.transform.Find("DeathScreen").gameObject;
        WinScreen = UI.transform.Find("WinScreen").gameObject;
        PauseScreen = UI.transform.Find("PauseScreen").gameObject;
        GameplayScreen = UI.transform.Find("GameplayScreen").gameObject;

        DeathScreen.SetActive(false);
        WinScreen.SetActive(false);
        PauseScreen.SetActive(false);
        GameplayScreen.SetActive(true);
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        PauseScreen.SetActive(true);
    }
    public void Unpause()
    {
        Time.timeScale = 1f;
        PauseScreen.SetActive(false);
    }
    public void ActivateDeathScreen()
    {
        Time.timeScale = 0f;
        DeathScreen.SetActive(true);
    }
    public void ActivateWinScreen()
    {
        Time.timeScale = 0f;
        WinScreen.SetActive(true);
    }

    public void loadNextLevel(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }

    public void retryCurrentLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void loadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void quitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
