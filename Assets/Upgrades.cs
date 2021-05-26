using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrades : MonoBehaviour
{
    Knight knight;
    public float healthPrice;
    public Text healthPriceText;
    public TextMeshProUGUI healthAmountText;
    public float manaPrice;
    public Text manaPriceText;
    public TextMeshProUGUI manaAmountText;
    private void Start()
    {
        knight = Knight.instance;
    }
    private void Update()
    {
        UpdateUI();
    }
    public void UpgradeHealth()
    {
        if (GameManager.instance.currentBits >= healthPrice)
        {
            knight.stats.maxHealth.IncreaseValue(10);
            GameManager.instance.currentBits -= healthPrice;
        }
    }
    public void UpgradeMana()
    {
        if (GameManager.instance.currentBits >= manaPrice)
        {
            knight.stats.maxMana.IncreaseValue(10);
            GameManager.instance.currentBits -= manaPrice;
        }
    }

    public void UpdateUI()
    {
        healthPriceText.text = healthPrice.ToString();
        healthAmountText.text = knight.stats.maxHealth.GetValue().ToString();
    }
}
