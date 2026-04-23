using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [Header("Pause Settings")]
    [SerializeField] private GameObject beforePauseImage;
    [SerializeField] private GameObject afterPauseImage;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;

    [Header("Lose Settings")]
    [SerializeField] private GameObject losePanel;

    [Header("Load Scene Settings")]
    [SerializeField] private GameObject sceneLoadingWindow;

    public static GameManager Instance { get; private set; }

    private FpsDisplay fpsDisplay;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        fpsDisplay = FpsDisplay.Instance;
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.fpsDisplayValue) == 1) {
            fpsDisplay.Show();
        } else {
            fpsDisplay.Hide();
        }
    }

    public void Pause() {
        Time.timeScale = 0f;
        beforePauseImage.SetActive(false);
        afterPauseImage.SetActive(true);
        pausePanel.SetActive(true);
    }

    public void Resume() {
        Time.timeScale = 1f;
        beforePauseImage.SetActive(true);
        afterPauseImage.SetActive(false);
        pausePanel.SetActive(false);
    }

    public void Lose() {
        Time.timeScale = 0f;
        losePanel.SetActive(true);
    }

    public void Restart() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void LoadScene(int sceneIndex) {
        sceneLoadingWindow.SetActive(true);
        SceneManager.LoadScene(sceneIndex);
    }

    public void Win() {
        Time.timeScale = 0f;
        winPanel.SetActive(true);
    }
}
