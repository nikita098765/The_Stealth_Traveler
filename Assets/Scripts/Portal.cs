using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI nextLocationInfoText;

    private int numberOfScenes, currentSceneIndex, nextSceneIndex;
    private bool isActive;

    private GameManager gameManager;

    private void Start() {
        gameManager = GameManager.Instance;
        numberOfScenes = SceneManager.sceneCountInBuildSettings;
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        nextSceneIndex = currentSceneIndex + 1;
    }

    private void Update() {
        if (isActive && Input.GetKeyDown(KeyCode.E)) {
            if (nextSceneIndex < numberOfScenes) {
                SaveManager.Instance.Save();
                gameManager.LoadScene(nextSceneIndex);
            } else {
                gameManager.Win();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        SetNextLocationInfoActive(collision, true);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        SetNextLocationInfoActive(collision, false);
    }

    private void SetNextLocationInfoActive(Collider2D collision, bool isActive) {
        if (collision.gameObject.GetComponent<PlayerController>() != null) {
            nextLocationInfoText.gameObject.SetActive(isActive);
            this.isActive = isActive;
        }
    }
}
