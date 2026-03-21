using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] public float money;
    [SerializeField] public TextMeshProUGUI moneyText;

    public DBManager dbManager;

    private void Start()
    {
        money = dbManager.GetUserMoney(); 
        UpdateUI();
    }


    public void UpdateUI()
    {
        moneyText.text = money.ToString();
    }
}
