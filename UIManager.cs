using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text coinsText;
    public Text levelText;

    public void UpdateUI(int coins, int level) {
        if (coinsText) coinsText.text = "Coins: " + coins;
        if (levelText) levelText.text = "Level: " + level;
    }
}