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

    private ItemResumePrefab ItemresumeInstance;

    [SerializeField] GameObject prefabItemResume;
    [SerializeField] Transform prefabInstantiate;

    void Start()
    {
        if (!Application.isPlaying) return;

        UpdateUI();
    }

    //Actualizamos la UI
    void UpdateUI()
    {
        if(itemInfoInstance == null)  return; 

        textNameItem.text = itemInfoInstance.nameItem;
        imageItem.sprite = itemInfoInstance.imageItem;
    }


    //Metodo para añadir productos de la lista
    public void addQuantityItem()
    {
        
        itemInfoInstance.quantityItem += quantityToAddOrSubs;//se suma 1 a la cantidad 
        Debug.Log("Has añadido un elemento mas de: " + itemInfoInstance.nameItem);

        ResumeShopManager resume = FindObjectOfType<ResumeShopManager>();

        if (ItemresumeInstance == null)
        {
            // Instanciar el prefab del resumen
            GameObject obj = Instantiate(prefabItemResume, prefabInstantiate);
            ItemresumeInstance = obj.GetComponent<ItemResumePrefab>();
            ItemresumeInstance.SetItem(itemInfoInstance);

            // añadimos el item a la lista de resumen
            resume.resumeItems.Add(itemInfoInstance);

            Debug.Log("Instanciado prefab del resumen y añadido a resumeItems");
        }
        else
        {
            // Actualizar cantidad en el prefab del resumen
            ItemresumeInstance.UpdateQuantity(itemInfoInstance.quantityItem);
        }

        // Actualizar precio total
        resume.UpdateTotalPrice();


    }


    //Metodo para quitar productos de la lista
    public void subsQuantityItem()
    {

        if (itemInfoInstance.quantityItem <= 0)
        {
            Debug.Log("No se pueden quitar mas, no tienes");
            return;
        }

        itemInfoInstance.quantityItem -= quantityToAddOrSubs;
        Debug.Log("Has eliminado un elemento de: " + itemInfoInstance.nameItem);

        ResumeShopManager resume = FindObjectOfType<ResumeShopManager>();

        if (itemInfoInstance.quantityItem == 0)
        {
            // Si llega a 0, eliminar el prefab del resumen
            if (ItemresumeInstance != null)
            {
                Destroy(ItemresumeInstance.gameObject);
                ItemresumeInstance = null;

                // quitamos el item de la lista del resumen
                resume.resumeItems.Remove(itemInfoInstance);

                Debug.Log("Item eliminado del resumen y de resumeItems");
            }
        }
        else
        {
            // Actualizar cantidad en el prefab del resumen
            ItemresumeInstance.UpdateQuantity(itemInfoInstance.quantityItem);
        }

        // Actualizar precio total
        resume.UpdateTotalPrice();



    }

}
