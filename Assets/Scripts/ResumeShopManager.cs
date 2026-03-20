using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeShopManager : MonoBehaviour
{

    public DBManager dbManager;
    public List<Item> resumeItems;

    public InventoryManager inventoryManager;

    public void BuyItems()
    {
        foreach (Item item in resumeItems)
        {
            dbManager.InsertItemToDB(item);

            inventoryManager.AddItemToInventory(item.nameItem, item.quantityItem, item.imageItem, item.descriptionItem);
        }

        Debug.Log("Compra guardada en la base de datos");
    }
}
