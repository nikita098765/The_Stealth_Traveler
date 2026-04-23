using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour {
    [Header("Item Type Settings")]
    [SerializeField] private Shop.Item itemType;

    [Header("Price Settings")]
    [SerializeField] private int price;
    [SerializeField] private TextMeshProUGUI priceText;

    [Header("Info Settings")]
    [SerializeField] private TextMeshProUGUI itemInfoText;

    [Header("Slot Settings")]
    [SerializeField] private GameObject slotForeground;

    [Header("Personal Settings")]
    [SerializeField] private int plusToInvisibilityDuration;

    private PlayerController player;
    private Inventory inventory;
    private bool isSuccessfulBuy = false;

    private void Start() {
        player = FindFirstObjectByType<PlayerController>();
        inventory = Inventory.Instance;

        if (itemType == Shop.Item.IncreaseInvisibility) {
            int playerPrefsInvisibilityPrice = PlayerPrefs.GetInt(PlayerPrefsKeys.invisibilityPrice);
            price = playerPrefsInvisibilityPrice > 0 ? playerPrefsInvisibilityPrice : price;

            string wordFormSecond = "";
            int last = plusToInvisibilityDuration % 10;
            if (last == 1) {
                wordFormSecond = "секунда";
            } else if (last >= 2 && last <= 4) {
                wordFormSecond = "секунды";
            } else {
                wordFormSecond = "секунд";
            }
            itemInfoText.text = "плюс " + plusToInvisibilityDuration + " " + wordFormSecond + "\nк невиди-\nмости";
        }

        priceText.text = price.ToString();
    }

    public void SetItemInfoActive() {
        slotForeground.SetActive(!slotForeground.activeSelf);
        itemInfoText.gameObject.SetActive(!itemInfoText.gameObject.activeSelf);
    }

    public void BuyItem() {
        bool isSuccess = false;

        if (itemType == Shop.Item.BuyRuby) {
            if (inventory.GetBone() >= price) {
                inventory.ChangeAmountBones(-price);
                inventory.ChangeAmountRubies(1);
                isSuccess = true;
            }
        } else if (itemType == Shop.Item.RestoreHealth) {
            if (inventory.GetRuby() >= price && player.GetHealth() < player.GetMaxHealth()) {
                inventory.ChangeAmountRubies(-price);
                player.ChangeHealth(1);
                isSuccess = true;
            }
        } else if (itemType == Shop.Item.IncreaseInvisibility) {
            if (inventory.GetRuby() >= price) {
                inventory.ChangeAmountRubies(-price);
                PlayerUI.Instance.ChangeInvisibilityDuration(plusToInvisibilityDuration);
                price *= 2;
                priceText.text = price.ToString();
                isSuccess = true;
            }
        }

        isSuccessfulBuy = isSuccess;
    }

    public bool GetIsSuccessfulBuy() {
        return isSuccessfulBuy;
    }

    public int GetInvisibilityPrice() {
        return itemType == Shop.Item.IncreaseInvisibility ? price : 0;
    }
}
