using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateShop : MonoBehaviour
{
    private bool shopActive;
    public GameObject shopMenu;

    public void ToggleShop()
    {
        shopActive = !shopActive;

        shopMenu.SetActive(shopActive);

        Time.timeScale = shopActive ? 0 : 1;//activar o desactivar el timescale segun como este la variable shopActive
    }
}
