using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{

    [SerializeField] private int IDItem;
    [SerializeField] public string nameItem;
    [SerializeField] public string descriptionItem;
    [SerializeField] public Sprite imageItem;
    [SerializeField] public float money;
    public int quantityItem = 0;



}
