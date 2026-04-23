using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour {
    [Header("Health Settings")]
    [SerializeField] private List<GameObject> hearts;

    [Header("Invisibility Settings")]
    [SerializeField] private List<GameObject> invisibilityTokens;
    [SerializeField] private GameObject activateInvisibilityButton;
    [SerializeField] private TextMeshProUGUI activeInvisibilityText;
    [SerializeField] private int invisibilityDuration;
    [SerializeField, Range(0f, 1f)] private float opacity;

    public static PlayerUI Instance { get; private set; }

    private PlayerController player;
    private int maxInvisibility;

    private const float zero = 0f, one = 1f;

    private void Awake() {
        Instance = this;
    }

    public void Initialize() {
        player = GetComponent<PlayerController>();
        int playerPrefsInvisibilityDuration = PlayerPrefs.GetInt(PlayerPrefsKeys.invisibilityDuration);
        invisibilityDuration = playerPrefsInvisibilityDuration > 0 ? playerPrefsInvisibilityDuration : invisibilityDuration;
        maxInvisibility = player.GetMaxInvisibility();
        UpdateHeartsUI();
        UpdateInvisibilityUI();
        CheckInvisibilityReady();
    }

    public void UpdateHeartsUI() {
        UpdateUI(hearts, player.GetHealth());
    }

    public void UpdateInvisibilityUI() {
        UpdateUI(invisibilityTokens, player.GetInvisibility());
    }

    public void CheckInvisibilityReady() {
        if (player.GetInvisibility() == maxInvisibility) {
            activateInvisibilityButton.SetActive(true);
        }
    }

    public void ActivateInvisibility() {
        activateInvisibilityButton.SetActive(false);
        GetComponent<SpriteRenderer>().color = new Color(one, one, one, opacity);
        player.ChangeInvisibility(-player.GetInvisibility());
        StartCoroutine(ActiveInvisibility());
    }

    private IEnumerator ActiveInvisibility() {
        player.SetIsActiveInvisibility(true);
        activeInvisibilityText.gameObject.SetActive(true);
        int remainingInvisibilityTime = invisibilityDuration;
        while (remainingInvisibilityTime != 0) {
            activeInvisibilityText.text = "невидимости закончится через " + remainingInvisibilityTime.ToString();
            remainingInvisibilityTime--;
            yield return new WaitForSeconds(1f);
        }
        player.SetIsActiveInvisibility(false);
        activeInvisibilityText.gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void UpdateUI(List<GameObject> items, int activeCount) {
        for (int i = 0; i < items.Count; i++) {
            SpriteRenderer spriteRenderer = items[i].GetComponent<SpriteRenderer>();
            if (i < activeCount) {
                spriteRenderer.color = Color.white;
            } else {
                spriteRenderer.color = new Color(zero, zero, zero, 0.5f);
            }
        }
    }

    public void ChangeInvisibilityDuration(int amount) {
        invisibilityDuration += amount;
    }

    public int GetInvisibilityDuration() {
        return invisibilityDuration;
    }

    public int GetHeartsCount() {
        return hearts.Count;
    }

    public int GetInvisibilityTokensCount() {
        return invisibilityTokens.Count;
    }

    public float GetOpacity() {
        return opacity;
    }
}
