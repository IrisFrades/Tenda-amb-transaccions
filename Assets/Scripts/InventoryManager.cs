using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public GameObject IntentoryMenu;
    private bool menuActivated;

    public InventoryItemSlot[] itemSlot;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory") && menuActivated)
        {
            Time.timeScale = 1;
            IntentoryMenu.SetActive(false);
            menuActivated = false;
        }

        else if (Input.GetButtonDown("Inventory") && !menuActivated)
        {
            Time.timeScale = 0;
            IntentoryMenu.SetActive(true);
            menuActivated = true;
        }
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemIsSelected = false;
        }
    }

    public void AddItemToInventory(string name, int quantity, Sprite sprite, string description)
    {
        foreach(InventoryItemSlot slot in itemSlot)
        {
            if(slot.itemName == name)
            {
                slot.quantity += quantity;
                slot.quantityTextUI.text = slot.quantity.ToString();
                return;
            }
        }

        foreach(InventoryItemSlot slot in itemSlot)
        {
            if (string.IsNullOrEmpty(slot.itemName))
            {
                slot.itemName = name;
                slot.quantity = quantity;
                slot.itemSprite = sprite;
                slot.itemDescription = description;

                slot.itemImageUI.sprite = sprite;
                slot.itemImageUI.enabled = true;
                slot.quantityTextUI.text = quantity.ToString();

                return;
            }
        }

        Debug.LogWarning("Inventario lleno");
    }

}
