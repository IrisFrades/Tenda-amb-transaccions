using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public TextMeshProUGUI textNameItem;
    public Image imageItem;
    public Button addButton;
    public Button subsButton;

    public Item itemInfoInstance;

    private int quantityToAddOrSubs = 1;

    [SerializeField] GameObject prefabItemResume;
    [SerializeField] Transform prefabInstantiate;
    // Start is called before the first frame update
    void Start()
    {
        if (!Application.isPlaying) return;

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void UpdateUI()
    {
        if(itemInfoInstance == null)  return; 

        textNameItem.text = itemInfoInstance.nameItem;
        imageItem.sprite = itemInfoInstance.imageItem;
    }

    public void addQuantityItem()
    {
        Debug.Log("Has añadido un elemento mas de: " + itemInfoInstance.nameItem);
        itemInfoInstance.quantityItem += quantityToAddOrSubs;

        if (itemInfoInstance.quantityItem > 0)
        {   
            Instantiate(prefabItemResume, prefabInstantiate);
            Debug.Log("Instanciado");
        }
           
    }

    public void subsQuantityItem()
    {

        if (itemInfoInstance.quantityItem <= 0)
        {
            Debug.Log("No se pueden quitar mas, no tienes");
            Destroy(prefabItemResume);
        }
        else
        {
            Debug.Log("Has eliminado un elemento de: " + itemInfoInstance.nameItem);
            itemInfoInstance.quantityItem -= quantityToAddOrSubs;
        }

    }

}
