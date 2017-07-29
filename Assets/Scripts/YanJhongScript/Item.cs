using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;



public class Item : MonoBehaviour
{
    [Header("Attribute")]
    public int id;
    public string itemName;
    public string description;
    public Sprite sprite;

    public bool canCombine;
    public bool canUse;
    public bool canInspect;

    public bool canDiscard;
    public bool canDisassemble;

    public Item combineWith;
    public List<Item> combineResult;

    public List<Item> disassembleResult;

    [Space(10)]
    bool block;
    // Use this for initialization

    public void Initialize(Item otherItem)
    {
        //Debug.Log("Base initialize");
        id = otherItem.id;
        itemName = otherItem.itemName;
        description = otherItem.description;
        sprite = otherItem.sprite;

        canCombine= otherItem.canCombine;
        canUse = otherItem.canUse;
        canInspect = otherItem.canInspect;

        canDiscard = otherItem.canDiscard;
        canDisassemble = otherItem.canDisassemble;

        combineWith = otherItem.combineWith;

        combineResult = new List<Item>();
        foreach (Item item in otherItem.combineResult)
            combineResult.Add(item);

        disassembleResult = new List<Item>();
        foreach (Item item in otherItem.disassembleResult)
            disassembleResult.Add(item);

        MonoBehaviour[] list = gameObject.GetComponents<MonoBehaviour>();
                
        if (otherItem.GetComponent<IOnUse>() != null)
        {
            //worked
            //EasyNS.Easy.CopyComponent(gameObject.AddComponent(component.GetType()), component);

            //worked too
            EasyNS.Easy.AddComponent(gameObject, otherItem.GetComponent(typeof(IOnUse)));
        }
        else
            Debug.LogError("No item baheviour detected");


        OnInitialize(otherItem);
    }

    protected virtual void OnInitialize(Item item)
    {

    }

    public bool CheckCombineItem(Item otherItem)
    {
        if (otherItem.combineWith == this)
            return true;
        return false;
    }

    public void DebugLog()
    {
        if (GetComponent<IOnUse>() != null)
            Debug.Log("I, " + itemName + ", have component");
        else
            Debug.Log("I, " + itemName + ", DO NOT have component");
    }
}
