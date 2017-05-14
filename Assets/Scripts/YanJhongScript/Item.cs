using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Attribute")]
    public int id;
    public string itemName;
    public string description;
    public Sprite sprite;
    //[Space(10)]    
    // Use this for initialization

    public void Initialize(Item item)
    {
        //Debug.Log("Base initialize");
        id = item.id;
        itemName = item.itemName;
        description = item.description;
        sprite = item.sprite;

        OnInitialize(item);
    }
    protected virtual void OnInitialize(Item item)
    {
        
    }
}
