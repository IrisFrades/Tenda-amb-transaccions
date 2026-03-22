using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemSlot : MonoBehaviour, IPointerClickHandler
{
    
    public GameObject selectedShader;
    public bool thisItemIsSelected;

    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public string itemDescription;

    public Image itemImageUI;
    public TextMeshProUGUI quantityTextUI;

    public Image itemDescriptionImage;
    public TextMeshProUGUI itemDescriptionName;
    public TextMeshProUGUI itemDescriptionText;


    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }    
    }

    private void OnLeftClick()
    {
        inventoryManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisItemIsSelected = true;

        itemDescriptionImage.sprite = itemSprite;
        itemDescriptionName.text = itemName;
        itemDescriptionText.text = itemDescription;
    }

    
}
