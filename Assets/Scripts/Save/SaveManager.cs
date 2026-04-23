using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI infoAboutSavingText;
    [SerializeField] private float infoDisplayTime;

    public static SaveManager Instance { get; private set; }

    private PlayerController player;
    private Inventory inventory;
    private List<ShopItem> shopItems;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        player = FindFirstObjectByType<PlayerController>();
        inventory = Inventory.Instance;
        shopItems = new List<ShopItem>(FindObjectsByType<ShopItem>(FindObjectsInactive.Include,
            FindObjectsSortMode.None));
    }

    public void Save() {
        infoAboutSavingText.gameObject.SetActive(true);
        PlayerPrefs.SetInt(PlayerPrefsKeys.health, player.GetHealth());
        PlayerPrefs.SetInt(PlayerPrefsKeys.invisibility, player.GetInvisibility());
        PlayerPrefs.SetInt(PlayerPrefsKeys.invisibilityDuration, PlayerUI.Instance.GetInvisibilityDuration());
        foreach (ShopItem shopItem in shopItems) {
            int invisibilityPrice = shopItem.GetInvisibilityPrice();
            if (invisibilityPrice > 0) {
                PlayerPrefs.SetInt(PlayerPrefsKeys.invisibilityPrice, invisibilityPrice);
                break;
            }
        }
        PlayerPrefs.SetInt(PlayerPrefsKeys.bone, inventory.GetBone());
        PlayerPrefs.SetInt(PlayerPrefsKeys.ruby, inventory.GetRuby());
        PlayerPrefs.SetInt(PlayerPrefsKeys.locationNumber, SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.Save();
    }
}
