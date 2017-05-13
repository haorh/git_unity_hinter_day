using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : Item{
    [Header("Data, should be private")]
    public bool detected;

    [Header("Refernce to other script")]
    public InventoryManager inventoryManager;

    [Header("Debug Purpose")]
    public bool debugMode = true;

    private void Update()
    {
        if (debugMode)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!detected)
                {
                    //Debug.Log("Selected this = " + gameObject.name + ", press E again to collect");
                    Detected();
                }
                else
                    CollectedItem();
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                Undetected();
            }
        }
    }
    public void Detected()
    {
        detected = true;
        Debug.Log("Selected this = " + gameObject.name + ", press E again to collect");
    }
    public void CollectedItem()
    {
        if(!detected)
        {
            Debug.LogError("Something wrong");
            return;
        }
        //--- save to inventory
        SaveToInventory();

        //------ play dying animation?
        Destroy(gameObject);
    }
    public void Undetected()
    {
        Debug.Log("Unselected this = " + gameObject.name + "+++++++++++++ undetect+++++++++");
        detected = false;
    }

    void SaveToInventory()
    {
        Debug.Log("Sending item = " + itemName + " with description = " + description);
        inventoryManager.Add(this);
    }
}
