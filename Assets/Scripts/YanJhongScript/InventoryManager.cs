using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {
    [Header("Attribute")]
    public bool rememberPreviousItem = false;

    [Header("Prefab")]
    public InventoryItem inventoryItemPrefab;

    [Header("Data, should be private")]
    List<InventoryItem> inventoryItemList;
    InventoryItem prevSelectedItem = null;
    bool changeSelectedItem = false;
    bool canUseInventory = true;
    bool showInventory = false;

    [Header("Reference to own Obj")]
    public GameObject inventoryObj;
    public GameObject inventoryContent;
    public Text itemTitle;
    public Text itemDescription;

    [Header("Debug Purpose")]
    public bool debugMode = true;

    private void Start()
    {
        inventoryItemList = new List<InventoryItem>();
        HideInventory();
    }
    private void Update()
    {
        if (debugMode)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                showInventory = !showInventory;
                if (showInventory)
                    DisplayInventory();
                else
                    HideInventory();
            }
        }
    }

    public void DisplayInventory()
    {
        showInventory = true;
        inventoryObj.SetActive(true);
    }

    public void HideInventory()
    {
        showInventory = false;
        inventoryObj.SetActive(false);
        DeselectAll();
    }

	/// <summary>
    /// Add new item into inventory
    /// </summary>
	public void Add(Item item)
    {
        InventoryItem inventoryItemTemp = Instantiate(inventoryItemPrefab, inventoryContent.transform);
        inventoryItemTemp.gameObject.transform.localPosition = new Vector3(0, 0, 0);
        inventoryItemTemp.Initialize(item);

        inventoryItemTemp.button.onClick.AddListener(()=>
        { 
            ValidateSelectItem(inventoryItemTemp);
            PlaySound();
            DeselectOldItem();
            SelectItem(inventoryItemTemp);
            UpdateInventoryUI(inventoryItemTemp);
            UpdatePrevItem(inventoryItemTemp);            
            UpdateChangeSelectedItem();
        });

        inventoryItemList.Add(inventoryItemTemp);

        if (rememberPreviousItem)
            if (prevSelectedItem == null)//only enter here when this is inventory 1st item
            {
                ValidateSelectItem(inventoryItemTemp);
                SelectItem(inventoryItemTemp);
                UpdateInventoryUI(inventoryItemTemp);
                UpdatePrevItem(inventoryItemTemp);
                UpdateChangeSelectedItem();
            }
    }

    void DeselectAll()
    {
        if(!rememberPreviousItem)
        {
            changeSelectedItem = true;
            DeselectOldItem();
            UpdateInventoryUI(null);
            UpdatePrevItem(null);
            UpdateChangeSelectedItem();
        }
    }

    void ValidateSelectItem(InventoryItem selectedItem)
    {
        if(prevSelectedItem!=null)
            if (prevSelectedItem == selectedItem)
            {
                changeSelectedItem = false;
                return;
            }
        changeSelectedItem = true;
    }
    
    void DeselectOldItem()//--- deselect old object
    {
        if (changeSelectedItem)
        {
            if(prevSelectedItem!=null)//need this because no item scenerio
                prevSelectedItem.Deselected();
        }
    }

    void SelectItem(InventoryItem selectedItem)//-- update new object
    {
        if (changeSelectedItem)
        {
            selectedItem.Selected();
        }
    }

    void UpdateInventoryUI(InventoryItem selectedItem)//--- update inventory ui
    {
        if (changeSelectedItem)
        {
            if (selectedItem != null)
            {
                itemTitle.text = selectedItem.itemName;
                itemDescription.text = selectedItem.description;
            }
            else
            {

                Debug.Log("there");

                itemTitle.text = "";
                itemDescription.text = "";
            }
        }
    }

    void UpdatePrevItem(InventoryItem selectedItem)
    {
        if (changeSelectedItem)
        {
            prevSelectedItem = selectedItem;
        }
    }

    void UpdateChangeSelectedItem()
    {
        changeSelectedItem = false;
    }

    void PlaySound()
    {

    }


}
