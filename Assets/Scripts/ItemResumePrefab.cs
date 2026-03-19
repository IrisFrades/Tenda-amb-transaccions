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
    

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateUI()
    {
        textNameItemResume.text = itemInstance.nameItem;
        textQuantityItemResume.text = itemInstance.quantityItem.ToString();
     
    }
}
