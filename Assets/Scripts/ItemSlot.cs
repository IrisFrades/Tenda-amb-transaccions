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

    void UpdateUI()
    {
        if(itemInfoInstance == null)  return; 

        textNameItem.text = itemInfoInstance.nameItem;
        imageItem.sprite = itemInfoInstance.imageItem;
    }

    public void addQuantityItem()
    {
        
        itemInfoInstance.quantityItem += quantityToAddOrSubs;
        Debug.Log("Has añadido un elemento mas de: " + itemInfoInstance.nameItem);

        if(ItemresumeInstance == null)
        {
            GameObject obj = Instantiate(prefabItemResume, prefabInstantiate); //Instanciamos el gameObject a la lista de resumen
            ItemresumeInstance = obj.GetComponent<ItemResumePrefab>();
            ItemresumeInstance.SetItem(itemInfoInstance);
            Debug.Log("Instanciado prefab del resumen");
        }
        else
        {
            ItemresumeInstance.UpdateQuantity(itemInfoInstance.quantityItem);
        }
           
    }

    public void subsQuantityItem()
    {

        if (itemInfoInstance.quantityItem <= 0) //Cuando no tienes mas elementos
        {
            Debug.Log("No se pueden quitar mas, no tienes");
            return;
        }

        itemInfoInstance.quantityItem -= quantityToAddOrSubs; //quitamos un elemento
        Debug.Log("Has eliminado un elemento de: " + itemInfoInstance.nameItem);

        if(itemInfoInstance.quantityItem == 0) //si la cantidad es 0
        {
            if(ItemresumeInstance != null) //si el prefab del item no es null (o sea aun esta en escena)
            {
                Destroy(ItemresumeInstance.gameObject); //eliminamos su gameObject
                ItemresumeInstance = null; //seteamos la instancia a null
            }
        }
        else
        {
            ItemresumeInstance.UpdateQuantity(itemInfoInstance.quantityItem); //actualizamos la cantidad
        }

    }

}
