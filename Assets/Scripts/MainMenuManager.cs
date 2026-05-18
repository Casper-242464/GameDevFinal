using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject volumePanel;
    [SerializeField] private GameObject controlsPanel;

    [Header("Volume")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Text volumeValueText;

    [Header("Keybinds")]
    [SerializeField] private TMP_Text jumpKeyText;
    [SerializeField] private TMP_Text leftKeyText;
    [SerializeField] private TMP_Text rightKeyText;

    private KeyCode jumpKey = KeyCode.Space;
    private KeyCode leftKey = KeyCode.A;
    private KeyCode rightKey = KeyCode.D;
    private TMP_Text waitingForKeyText = null;

    private void Start()
    {
        ShowMainMenu();
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        UpdateVolumeUI();
        UpdateKeybindUI();
    }

    // Main Menu Buttons
    public void OnStartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void OnSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        volumePanel.SetActive(false);
        controlsPanel.SetActive(false);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        volumePanel.SetActive(false);
        controlsPanel.SetActive(false);
    }

    // Settings Panel Buttons
    public void OnVolumePanel()
    {
        volumePanel.SetActive(true);
        controlsPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void OnControlsPanel()
    {
        controlsPanel.SetActive(true);
        volumePanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void OnBackFromSettings()
    {
        ShowMainMenu();
    }

    // Volume Control
    private void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        UpdateVolumeUI();
    }

    private void UpdateVolumeUI()
    {
        if (volumeValueText != null)
            volumeValueText.text = Mathf.RoundToInt(volumeSlider.value * 100).ToString();
    }

    // Key Remapping
    public void OnChangeJumpKey()
    {
        waitingForKeyText = jumpKeyText;
        jumpKeyText.text = "Press any key...";
    }

    public void OnChangeLeftKey()
    {
        waitingForKeyText = leftKeyText;
        leftKeyText.text = "Press any key...";
    }

    public void OnChangeRightKey()
    {
        waitingForKeyText = rightKeyText;
        rightKeyText.text = "Press any key...";
    }

    private void Update()
    {
        if (waitingForKeyText != null && Input.anyKeyDown)
        {
            foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                {
                    if (waitingForKeyText == jumpKeyText) jumpKey = kcode;
                    else if (waitingForKeyText == leftKeyText) leftKey = kcode;
                    else if (waitingForKeyText == rightKeyText) rightKey = kcode;
                    waitingForKeyText = null;
                    UpdateKeybindUI();
                    break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowMainMenu();
        }

        PlayerPrefs.SetInt("JumpKey", (int)jumpKey);
        PlayerPrefs.SetInt("LeftKey", (int)leftKey);
        PlayerPrefs.SetInt("RightKey", (int)rightKey);
        PlayerPrefs.Save();
    }

    private void UpdateKeybindUI()
    {
        jumpKeyText.text = jumpKey.ToString();
        leftKeyText.text = leftKey.ToString();
        rightKeyText.text = rightKey.ToString();
    }

    public KeyCode GetJumpKey() => jumpKey;
    public KeyCode GetLeftKey() => leftKey;
    public KeyCode GetRightKey() => rightKey;
}
