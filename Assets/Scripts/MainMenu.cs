using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [Header("Main Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject sceneLoadingWindow;

    [Header("Menu Settings Settings")]
    [SerializeField] private GameObject mainSettingsPanel;
    [SerializeField] private TextMeshProUGUI onOffVsyncText;
    [SerializeField] private TextMeshProUGUI onOffFpsDisplayText;

    [Header("Reset Saves Settings")]
    [SerializeField] private GameObject confirmResetSavesPanel;
    [SerializeField] private TextMeshProUGUI infoAboutResetSaves;
    [SerializeField] private float infoAboutResetSavesDisplayTime;

    private FpsDisplay fpsDisplay;

    private void Start() {
        fpsDisplay = FpsDisplay.Instance;
        QualitySettings.vSyncCount = PlayerPrefs.GetInt(PlayerPrefsKeys.vsyncValue);
        UpdateOnOffVsyncText(QualitySettings.vSyncCount);

        UpdateOnOffFpsDisplayTextAndGameObject(PlayerPrefs.GetInt(PlayerPrefsKeys.fpsDisplayValue));
    }

    public void StartGame() {
        sceneLoadingWindow.SetActive(true);
        int locationNumber = PlayerPrefs.GetInt(PlayerPrefsKeys.locationNumber);
        if (locationNumber == 0) {
            locationNumber = 1;
        }
        SceneManager.LoadScene(locationNumber);
    }

    public void OpenSettings() {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void ClickResetSavesButton() {
        mainSettingsPanel.SetActive(false);
        confirmResetSavesPanel.SetActive(true);
    }

    public void ConfirmResetSaves(bool value) {
        if (value) {
            int currentVsync = PlayerPrefs.GetInt(PlayerPrefsKeys.vsyncValue);
            int currentFpsDisplay = PlayerPrefs.GetInt(PlayerPrefsKeys.fpsDisplayValue);

            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            PlayerPrefs.SetInt(PlayerPrefsKeys.vsyncValue, currentVsync);
            PlayerPrefs.SetInt(PlayerPrefsKeys.fpsDisplayValue, currentFpsDisplay);
            PlayerPrefs.Save();

            infoAboutResetSaves.gameObject.SetActive(true);
            CancelInvoke(nameof(HideInfoAboutResetSaves));
            Invoke(nameof(HideInfoAboutResetSaves), infoAboutResetSavesDisplayTime);
        }
        confirmResetSavesPanel.SetActive(false);
        mainSettingsPanel.SetActive(true);
    }

    public void ToggleFpsDisplay() {
        int fpsDisplayValue = PlayerPrefs.GetInt(PlayerPrefsKeys.fpsDisplayValue) == 0 ? 1 : 0;
        PlayerPrefs.SetInt(PlayerPrefsKeys.fpsDisplayValue, fpsDisplayValue);
        PlayerPrefs.Save();
        UpdateOnOffFpsDisplayTextAndGameObject(PlayerPrefs.GetInt(PlayerPrefsKeys.fpsDisplayValue));
    }

    public void ToggleVSync() {
        QualitySettings.vSyncCount = QualitySettings.vSyncCount == 0 ? 1 : 0;
        PlayerPrefs.SetInt(PlayerPrefsKeys.vsyncValue, QualitySettings.vSyncCount);
        PlayerPrefs.Save();
        UpdateOnOffVsyncText(QualitySettings.vSyncCount);
    }

    public void BackToMenu() {
        CancelInvoke(nameof(HideInfoAboutResetSaves));
        HideInfoAboutResetSaves();
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    private void UpdateOnOffFpsDisplayTextAndGameObject(int currentFpsDisplay) {
        onOffFpsDisplayText.text = currentFpsDisplay == 0 ? "включить отображение fps" :
            "выключить отображение fps";
        if (currentFpsDisplay == 1) {
            fpsDisplay.Show();
        } else {
            fpsDisplay.Hide();
        }
    }

    private void UpdateOnOffVsyncText(int currentVsync) {
        onOffVsyncText.text = currentVsync == 0 ? "включить vsync" : "выключить vsync";
    }

    private void HideInfoAboutResetSaves() {
        infoAboutResetSaves.gameObject.SetActive(false);
    }
}
