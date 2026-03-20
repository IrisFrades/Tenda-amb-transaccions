using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeShopManager : MonoBehaviour
{

    public DBManager dbManager;
    public List<Item> resumeItems;

    public void BuyItems()
    {
        foreach (Item item in resumeItems)
        {
            dbManager.InsertItemToDB(item);
        }

        Debug.Log("Compra guardada en la base de datos");
    }
}
