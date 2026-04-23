using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shop : MonoBehaviour {
    public enum Item { BuyRuby, RestoreHealth, IncreaseInvisibility }

    [Header("Shop Info Settings")]
    [SerializeField] private TextMeshProUGUI shopEntranceInfoText;
    [SerializeField] private GameObject shopMenu;

    [Header("Info About Buy Settings")]
    [SerializeField] private TextMeshProUGUI infoAboutBuyText;
    [SerializeField] private float infoAboutBuyDisplayTime;

    private int counterSuccessfulBuy = 0, counterUnsuccessfulBuy = 0;
    private bool isActive = false, shopOpen = false;

    private const int maxDisplayCounterBuy = 1000;

    private void Start() {
        SetShopMenuActive(false);
    }

    private void Update() {
        if (isActive && !shopOpen && Input.GetKeyDown(KeyCode.E)) {
            shopEntranceInfoText.gameObject.SetActive(false);
            SetShopMenuActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        SetInfoTextActive(collision, true);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        SetInfoTextActive(collision, false);
        SetShopMenuActive(false);
        CancelInvoke(nameof(HideInfoAboutBuy));
        HideInfoAboutBuy();
    }

    private void SetInfoTextActive(Collider2D collision, bool isActive) {
        if (collision.gameObject.GetComponent<PlayerController>() != null) {
            shopEntranceInfoText.gameObject.SetActive(isActive);
            this.isActive = isActive;
        }
    }

    private void SetShopMenuActive(bool isActive) {
        shopMenu.SetActive(isActive);
        shopOpen = isActive;
    }

    public void SetItemInfoActive(BaseEventData data) {
        (data as PointerEventData).pointerEnter.GetComponentInParent<ShopItem>().SetItemInfoActive();
    }

    public void Buy(BaseEventData data) {
        ShopItem shopItem = (data as PointerEventData).pointerEnter.GetComponentInParent<ShopItem>();
        shopItem.BuyItem();

        infoAboutBuyText.gameObject.SetActive(true);

        if (shopItem.GetIsSuccessfulBuy()) {
            counterSuccessfulBuy++;
            counterUnsuccessfulBuy = 0;
            infoAboutBuyText.text = "успешная покупка!" + GetNumberOfBuy(counterSuccessfulBuy);
        } else {
            counterUnsuccessfulBuy++;
            counterSuccessfulBuy = 0;
            infoAboutBuyText.text = "неуспешная покупка!" + GetNumberOfBuy(counterUnsuccessfulBuy);
        }

        CancelInvoke(nameof(HideInfoAboutBuy));
        Invoke(nameof(HideInfoAboutBuy), infoAboutBuyDisplayTime);
    }

    private string GetNumberOfBuy(int counter) {
        if (counter > maxDisplayCounterBuy) {
            return " x(>" + maxDisplayCounterBuy + ")";
        } else if (counter > 1) {
            return " x" + counter;
        } else {
            return "";
        }
    }

    private void HideInfoAboutBuy() {
        infoAboutBuyText.gameObject.SetActive(false);
        counterSuccessfulBuy = 0;
        counterUnsuccessfulBuy = 0;
    }
}
