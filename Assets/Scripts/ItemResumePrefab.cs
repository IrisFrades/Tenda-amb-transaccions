using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;

public class ItemResumePrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textNameItemResume;
    [SerializeField] private TextMeshProUGUI textQuantityItemResume;

    public ItemSlot itemSlotInstance;
    public Item itemInstance;
    

    public void SetItem(Item item) //seteamos un item
    {
        itemInstance = item;
        UpdateUI();
    }

    public void UpdateQuantity(int newQuantity) //se actualiza la cantidad del item
    {
        itemInstance.quantityItem = newQuantity;
        UpdateUI();
    }

    void UpdateUI() //actualizamos la UI
    {
        textNameItemResume.text = itemInstance.nameItem;
        textQuantityItemResume.text = itemInstance.quantityItem.ToString();
     
    }
}
