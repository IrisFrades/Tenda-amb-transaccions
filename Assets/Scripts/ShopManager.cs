using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    public List<Item> allShopItems;

    //Reseteamos las cantidades al comprar a 0 de nuevo
    public void ResetShopQuantities()
    {
        foreach (Item item in allShopItems)
        {
            item.quantityItem = 0;
        }
    }


}
