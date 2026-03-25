using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateShop : MonoBehaviour
{
    public GameObject shopMenu;
    public GameObject imageShop;

    public GameObject inventoryMenu;
    public GameObject imageInventory;

    public Button ExitButton;
    private void Awake()
    {
        imageShop.SetActive(false);
        imageInventory.SetActive(false);
    }

    public void ToggleShop()
    {
        shopMenu.SetActive(true);
        imageShop.SetActive(true);
    }

    public void ToggleInventory()
    {
        inventoryMenu.SetActive(true);
        imageInventory.SetActive(true);
    }

    public void NonToggleShop()
    {
        shopMenu.SetActive(false);
        imageShop.SetActive(false);
    }
    public void NonToggleInventory()
    {
        inventoryMenu.SetActive(false);
        imageInventory.SetActive(false);
    }

    public void exitButton()
    {
        Application.Quit();
    }

}
