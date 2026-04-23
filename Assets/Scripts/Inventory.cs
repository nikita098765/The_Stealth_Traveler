using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI boneText, rubyText;

    public static Inventory Instance {  get; private set; }

    private int numberOfBones, numberOfRubies;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        numberOfBones = PlayerPrefs.GetInt(PlayerPrefsKeys.bone);
        numberOfRubies = PlayerPrefs.GetInt(PlayerPrefsKeys.ruby);
        boneText.text = numberOfBones.ToString();
        rubyText.text = numberOfRubies.ToString();
    }

    public void ChangeAmountBones(int amount) {
        numberOfBones += amount;
        boneText.text = numberOfBones.ToString();
    }

    public void ChangeAmountRubies(int amount) {
        numberOfRubies += amount;
        rubyText.text = numberOfRubies.ToString();
    }

    public int GetBone() {
        return numberOfBones;
    }

    public int GetRuby() {
        return numberOfRubies;
    }
}
