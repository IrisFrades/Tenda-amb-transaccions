using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResumeShopManager : MonoBehaviour
{

    public DBManager dbManager;//instancia
    public List<Item> resumeItems;//lista de productos en el resumen

    public InventoryManager inventoryManager;//instancia
    public MoneyManager moneyManager;//instancia

    public TextMeshProUGUI totalPriceResumeText;

    public Transform resumeContainer; // container del scroll view
    public ShopManager shopManager; //instancia


    public void BuyItems()
    {
        float totalCost = 0; //El coste total de todos los items en el resumen

        foreach (Item item in resumeItems)
        {
            totalCost += item.money * item.quantityItem; //el coste total es la suma del dinero y la cantidad de ese item
        }

        float money = dbManager.GetUserMoney(); //se obtiene el dinero del usuario

        if (money >= totalCost) //si el dinero que tiene es mayor o igual al coste total
        {
            float newMoney = money - (float)totalCost; //le quitamos el dinero

            dbManager.UpdateUserMoney(newMoney); //actualizamos el dinero en la base de datos

            moneyManager.money = newMoney;
            moneyManager.UpdateUI(); //y actualizamos la UI

            //aqui guardamos cada item que se ha comprado a la base de datos
            foreach (Item item in resumeItems)
            {
                dbManager.InsertItemToDB(item);

                inventoryManager.AddItemToInventory(item.nameItem, item.quantityItem, item.imageItem, item.descriptionItem);
            }

            //limpiamos la zona del resumen de la lista
            ClearResume();
            shopManager.ResetShopQuantities(); //y reseteamos las cantidades a 0 de nuevo

            Debug.Log("Se ha echo la compra");
        }
        else
        {
            Debug.Log("No tienes suficiente dinero");
        }
    }

    //Aqui se actualiza el precio total del resumen
    public void UpdateTotalPrice()
    {


        float total = 0;

        foreach (Item item in resumeItems)
        {
            total += item.money * item.quantityItem;
        }

        totalPriceResumeText.text = "Total: " + total.ToString("F2") + " €";
    }

    //Metodo para limpiar la lista del resumen en cuanto compras 
    public void ClearResume()
    {
        // Vaciamos la lista del resumen
        resumeItems.Clear();

        // Borramos todos los prefabs que haya
        foreach (Transform child in resumeContainer)
        {   
            Destroy(child.gameObject);
        }

        // Y reiniciamos el texto del precio total
        totalPriceResumeText.text = "Total:" + "0";
    }


}
