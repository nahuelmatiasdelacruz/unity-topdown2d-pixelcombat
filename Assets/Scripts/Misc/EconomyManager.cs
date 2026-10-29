using TMPro;
using UnityEngine;

public class EconomyManager : Singleton<EconomyManager>
{
    private TextMeshProUGUI goldText;

    const string COIN_AMOUNT_TEXT = "Gold Amount Text";
    private int currentGold = 0;

    public void UpdateCurrentGold()
    {
        currentGold++;
        if(goldText == null)
        {
            goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TextMeshProUGUI>();
        }
        goldText.text = currentGold.ToString("D3");
    }
}
