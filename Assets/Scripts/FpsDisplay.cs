using System.Collections;
using TMPro;
using UnityEngine;

public class FpsDisplay : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI fpsText;

    public static FpsDisplay Instance { get; private set; }

    private float deltaTime = 0.0f, fps;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
    }

    private IEnumerator UpdateFps() {
        while (true) {
            fpsText.text = Mathf.Ceil(fps).ToString() + " FPS";
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }

    public void Show() {
        gameObject.SetActive(true);
        StartCoroutine(UpdateFps());
    }

    public void Hide() {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}