using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {
    [Header("Attribute")]
    public bool rememberPreviousItem = false;
    public int itemLimit = -1;

   [Header("Prefab")]
    public InventoryItem inventoryItemPrefab;

    [Header("For Control")]
    public Vector2 selectedPosition;
    public Vector2 newestPosition;
    public int gridXLimit;
   

    [Header("Data, should be private")]
    public List<InventoryItem> inventoryItemList;
    InventoryItem prevSelectedItem = null;
    bool changeSelectedItem = false;
    public bool canUseInventory = true;
    public bool showInventory = false;

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
        newestPosition = new Vector2(-1, 0);//must be -1
        selectedPosition = new Vector2(-1, -1);

        HideInventory();
    }

    public void ToggleInventory()
    {
        if (canUseInventory)
        {
            showInventory = !showInventory;
            if (showInventory)
                DisplayInventory();
            else
                HideInventory();
        }
    }

    void DisplayInventory()
    {
        showInventory = true;
        inventoryObj.SetActive(true);
    }

    void HideInventory()
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
        //inventoryItemTemp.SetPosition(newestPosition);

        StartCoroutine(LateInitialize(inventoryItemTemp));

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
            if (prevSelectedItem == null)//only enter here when this is inventory 1st item, without playing sound
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

                if (selectedItem.inventoryPosition.x != -1)
                    selectedPosition = selectedItem.inventoryPosition;
                else
                    selectedPosition = newestPosition;//will be false when item is 1st added with mustRememberOld, because position haven't initialize
            }
            else
            {
                
                itemTitle.text = "";
                itemDescription.text = "";

                selectedPosition = new Vector2(-1, -1);
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

    IEnumerator LateInitialize(InventoryItem inventoryItemTemp)//we do not know that is the gridlayout until game placed the item
    {
        yield return new WaitForEndOfFrame();//grid only update item position at the end of frame
        
        //------ update grid layout
        UpdateGridXLimit();

        UpdateNewestPosition();

        inventoryItemTemp.inventoryPosition = newestPosition;
        //Debug.Log("Save position = " + newestPosition);
    }
    void UpdateGridXLimit()
    {
        int rowLimit = 1;
        float positionY = 0;
        bool foundNextRow = false;
        for (int x = 0; x < inventoryItemList.Count; x++)
        {
            if (x == 0)
                positionY = inventoryItemList[0].gameObject.GetComponent<RectTransform>().localPosition.y;//save 1st position
            else if (inventoryItemList[x].gameObject.GetComponent<RectTransform>().localPosition.y != positionY)//compare it with subsequent items
            {
                rowLimit = x;
                foundNextRow = true;
                break;
            }
        }

        if (foundNextRow)
            gridXLimit = rowLimit;
        else
            gridXLimit = -1;
    }
    void UpdateNewestPosition()
    {
        if (gridXLimit < 0)//dunno what is the limit, just ++
            newestPosition.x++;
        else if (++newestPosition.x >= gridXLimit)
        {
            newestPosition.x = 0;
            newestPosition.y++;
        }
    }

    public InventoryItem FindInventoryItem(Vector2 position)
    {
        foreach (InventoryItem item in inventoryItemList)
            if (item.inventoryPosition == position)
                return item;

        return null;
    }

    public void SelectItemViaKeyboard(InventoryItem item)
    {
        var pointer = new PointerEventData(EventSystem.current); // pointer event for Execute
        ExecuteEvents.Execute(item.button.gameObject, pointer, ExecuteEvents.submitHandler);
    }
}
