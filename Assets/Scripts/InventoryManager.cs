using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public GameObject IntentoryMenu;
    private bool menuActivated;

    public InventoryItemSlot[] itemSlot;

    [SerializeField] MoneyManager moneyManager;

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

    public void AddItemToInventory(string name, int quantity, Sprite sprite, string description, float price)
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
                slot.itemPrice = price;

                slot.itemImageUI.sprite = sprite;
                slot.itemImageUI.enabled = true;
                slot.quantityTextUI.text = quantity.ToString();

                return;
            }
        }

        Debug.LogWarning("Inventario lleno");
    }

    public void SellItems()
    {
        InventoryItemSlot selectedSlot = null;

        foreach(var slot in itemSlot)
        {
            if(slot.thisItemIsSelected)
            {
                selectedSlot = slot;
                break;
            }
        }

        if(selectedSlot == null)
        {
            Debug.Log("No se ha seleccionado ningun item");
            return;
        }

        float moneyToReturn = selectedSlot.itemPrice * selectedSlot.quantity; //precio del item x la cantidad del item que tenemos

        DBManager db = FindObjectOfType<DBManager>();

        float currentMoney = db.GetUserMoney(); //Sacamos el dinero que tiene el usuario

        float newMoney = currentMoney + moneyToReturn; //el dinero nuevo sera el dinero actual mas el dinero que hay que devolver

        db.UpdateUserMoneyWhenSelling(newMoney); //Actualizamos el dinero de la base con el nuevo que se ha calculado
        moneyManager.money = newMoney;
        moneyManager.UpdateUI(); //actualizamos la UI

        //vaciar el slot del item vendido
        selectedSlot.itemName = "";
        selectedSlot.quantity = 0;
        selectedSlot.itemSprite = null;
        selectedSlot.itemDescription = "";
        selectedSlot.itemPrice = 0;

        selectedSlot.itemImageUI.enabled = false;
        selectedSlot.quantityTextUI.text = "";
        selectedSlot.selectedShader.SetActive(false);
        selectedSlot.thisItemIsSelected = false;

        Debug.Log("Item venut correctament");


    }

    public void LoadInventoryFromDB()
    {
        DBManager db = FindObjectOfType<DBManager>();
        List<Item> items = db.LoadPlayerItems();

        foreach (Item item in items)
        {
            AddItemToInventory(
                item.nameItem,
                item.quantityItem,
                item.imageItem,
                item.descriptionItem,
                item.money
            );
        }
    }


}
