using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, INotifyPropertyChanged
{
    [Header("Attribute")]
    [Tooltip("True: Always highlight previous item selected by user. False : New item will always be highlighted")]
    //exception, if true, 1st item will automatic be highlighted, if false, 1st item will not be highlighted
    public bool rememberPreviousItem = false;
    public int itemLimit = -1;
    public bool spawnAtLastDestroy = true;
    public Vector2 lastDeletePosition;


    [Header("Prefab")]
    public InventoryItem inventoryItemPrefab;

    [Header("For Control")]
    //Vector2 highlightPosition;//highlightPosition follow user control, normally it follow currentItem unless user is not selecting any currentItem
    Vector2 newestEmptyPosition;
    int gridXLimit;
    bool maxInventory = false;

    [Header("Data, should be private")]
    public List<InventoryItem> inventoryItemList;
    public InventoryItem currentSelectedItem;//
    bool changeSelectedItem = false;

    public bool canUseInventory = true;

    bool showInventory = false;

    [Header("Reference to own Obj")]
    public GameObject inventoryContent;

    [Header("Mode")]
    public InventoryMode mode = InventoryMode.NA;
    public enum InventoryMode { NA, Combine, Inspect };

    [Header("Combine Mode")]
    public InventoryItem currentCombineItem;//currentCombineItem = currentSelectedItem, all new item is currentSelectedItem
    //bool combineMode = false;
    public enum ItemAction { Use, Inspect, Combine, Discard, Disassemble };

    string inventoryMessage;

    [Header("Debug Purpose")]
    public bool debugMode = true;

    private void Start()
    {
        inventoryItemList = new List<InventoryItem>();
        newestEmptyPosition = new Vector2(0, 0);//must be -1
        //highlightPosition = new Vector2(-1, -1);

        lastDeletePosition = new Vector2(-1, -1);
        HideInventory();
        OnPropertyChanged("all");

    }

    public bool ShowInventory
    {
        get
        {
            return showInventory;
        }
        set
        {
            showInventory = value;
            OnPropertyChanged("ShowInventory");
        }
    }
    public InventoryItem CurrentSelectedItem
    {
        get
        {
            return currentSelectedItem;
        }
        set
        {
            currentSelectedItem = value;
            OnPropertyChanged("CurrentSelectedItem");
        }
    }
    public InventoryItem CurrentCombineItem
    {
        get
        {
            return currentCombineItem;
        }
        set
        {
            currentCombineItem = value;
            OnPropertyChanged("SelectedCombineItem");
        }
    }
    public InventoryMode Mode
    {
        get
        {
            return mode;
        }
        set
        {
            mode = value;
            OnPropertyChanged("Mode");
        }
    }
    public string InventoryMessage
    {
        get { return inventoryMessage; }
        set
        {
            inventoryMessage = value;
            OnPropertyChanged("InventoryMessage");
        }
    }
    //public Vector2 HighlightPosition
    //{
    //    get
    //    {
    //        return highlightPosition;
    //    }
    //    set
    //    {
    //        highlightPosition = value;
    //        OnPropertyChanged("HighlightPosition");
    //    }
    //}


    protected void OnPropertyChanged(string propertyName)
    {
        OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

    protected void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        //Debug.Log("propertyname is = " + e.PropertyName);
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
            handler(this, e);
    }

    public event PropertyChangedEventHandler PropertyChanged;

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
        ShowInventory = true;
    }

    void HideInventory()
    {
        ShowInventory = false;
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

        inventoryItemTemp.button.onClick.AddListener(() =>
        {
            ValidateSelectItem(inventoryItemTemp);
            PlaySound();
            DeselectOldItem();
            SelectItem(inventoryItemTemp);

            //UpdateHighlightPosition(inventoryItemTemp);

            UpdatePrevItem(inventoryItemTemp);
            UpdateChangeSelectedItem();


        });

        inventoryItemList.Add(inventoryItemTemp);


        if (rememberPreviousItem)
            if (currentSelectedItem == null)//only enter here when this is inventory 1st item, without playing sound
            {
                ValidateSelectItem(inventoryItemTemp);
                SelectItem(inventoryItemTemp);

                //UpdateHighlightPosition(inventoryItemTemp);

                UpdatePrevItem(inventoryItemTemp);
                UpdateChangeSelectedItem();

            }
    }
    public void Remove(InventoryItem inventoryItem)
    {
        if (inventoryItemList.Remove(inventoryItem))
            Destroy(inventoryItem.gameObject);

    }

    void DeselectAll()
    {
        if (!rememberPreviousItem)
        {
            changeSelectedItem = true;
            DeselectOldItem();

            //UpdateHighlightPosition(null);
            UpdatePrevItem(null);
            changeSelectedItem = false;
        }
    }

    void ValidateSelectItem(InventoryItem selectedItem)
    {
        if (currentSelectedItem == null)
        {
            changeSelectedItem = true;
            return;
        }

        if (mode == InventoryMode.NA)
        {
            if (currentSelectedItem == selectedItem)
                changeSelectedItem = false;
            else
                changeSelectedItem = true;
        }
        else if (mode == InventoryMode.Combine)
            changeSelectedItem = true;
    }

    void DeselectOldItem()//--- deselect old object
    {
        if (changeSelectedItem)
        {
            if (mode == InventoryMode.NA)
            {
                if (currentSelectedItem != null)//need this because no item scenerio
                    currentSelectedItem.Deselected();
            }
            else if (mode == InventoryMode.Combine)
            {
                if (currentSelectedItem != currentCombineItem)//if previous item is original highilight item, don deselect it
                    currentCombineItem.Deselected();
            }
        }
    }

    void SelectItem(InventoryItem selectedItem)//-- update new object
    {
        if (changeSelectedItem)
        {
            selectedItem.Selected();
        }
    }

    //void UpdateHighlightPosition(InventoryItem selectedItem)
    //{
    //    if (changeSelectedItem)
    //    {
    //        if (!combineMode)
    //        {
    //            if (selectedItem != null)
    //            {
    //                if (selectedItem.inventoryPosition.x != -1)
    //                    HighlightPosition = selectedItem.inventoryPosition;
    //                else
    //                    HighlightPosition = newestEmptyPosition;//will be false when item is 1st added with mustRememberOld, because position haven't initialize
    //            }
    //            else
    //            {
    //                HighlightPosition = new Vector2(-1, -1);
    //            }
    //        }            
    //    }
    //}


    void UpdatePrevItem(InventoryItem selectedItem)
    {
        if (changeSelectedItem)
        {
            if (mode == InventoryMode.NA)
                CurrentSelectedItem = selectedItem;
            else if (mode == InventoryMode.Combine)
                CurrentCombineItem = selectedItem;
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

        if (mode == InventoryMode.NA)
            inventoryItemTemp.inventoryPosition = newestEmptyPosition;
        else if (mode == InventoryMode.Combine)
        {
            if (spawnAtLastDestroy)
            {
                PushIn(inventoryItemTemp, lastDeletePosition);//use sibiling index

            }
            else
                inventoryItemTemp.inventoryPosition = newestEmptyPosition;

        }
        newestEmptyPosition = NextPosition(newestEmptyPosition);
        //Debug.Log("Save position = " + newestPosition);
    }

    void PushIn(InventoryItem inventoryItemTemp, Vector2 inventPosition)
    {
        int insertIndex = -1;
        for (int x = 0; x < inventoryItemList.Count; x++)
            if (inventoryItemList[x].inventoryPosition == inventPosition)
            {
                insertIndex = x;
                break;
            }
        if (insertIndex == 0)
            insertIndex = inventoryItemList.Count;


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

        if (gridXLimit != -1 && newestEmptyPosition.x >= gridXLimit)
        {
            newestEmptyPosition.x = 0;
            newestEmptyPosition.y++;
        }
    }

    Vector2 NextPosition(Vector2 inputPosition)
    {
        var nextPosition = inputPosition;

        if (gridXLimit < 0)//dunno what is the limit, just ++
            nextPosition.x++;
        else if (++nextPosition.x > gridXLimit)
        {
            nextPosition.x = 0;
            nextPosition.y++;
        }

        return nextPosition;
    }

    Vector2 PrevPosition(Vector2 inputPosition)
    {
        var prevPosition = inputPosition;

        if (--prevPosition.x <= -2 || prevPosition.y < 0)//invalid
        {
            prevPosition.x = -1;
            prevPosition.y = -1;
        }
        else if (prevPosition.x < 0)
        {
            if (prevPosition.y == 0)//invalid
                prevPosition.y = -1;
            else//valid
            {
                prevPosition.x = gridXLimit;
                prevPosition.y -= 1;
            }
        }

        return prevPosition;
    }

    public InventoryItem FindInventoryItem(Vector2 position)
    {
        foreach (InventoryItem item in inventoryItemList)
            if (item.inventoryPosition == position)
                return item;

        return null;
    }

    public InventoryItem FindNextInventoryItem(Vector2 position)
    {
        if (inventoryItemList.Count < 2)
            return null;
        if (position == Vector2.zero)
            return null;
        if (position.x > gridXLimit)
            return null;
        if (position.y > newestEmptyPosition.y)
            return null;

        var nextPosition = NextPosition(position);

        foreach (InventoryItem item in inventoryItemList)
            if (item.inventoryPosition == nextPosition)
                return item;

        return null;
    }

    public InventoryItem FindPrevInventoryItem(Vector2 position)
    {
        if (inventoryItemList.Count < 2)
            return null;
        if (position == Vector2.zero)
            return null;
        if (position.x > gridXLimit)
            return null;
        if (position.y > newestEmptyPosition.y)
            return null;

        var prevPosition = NextPosition(position);

        foreach (InventoryItem item in inventoryItemList)
            if (item.inventoryPosition == prevPosition)
                return item;

        return null;
    }

    public void SelectItemViaKeyboard(InventoryItem item)
    {
        var pointer = new PointerEventData(EventSystem.current); // pointer event for Execute
        ExecuteEvents.Execute(item.button.gameObject, pointer, ExecuteEvents.submitHandler);
    }

    public void SelectAction(ItemAction action)
    {
        if (!ValidateAction(action))
        {
            Debug.Log("Invalid action for action = " + action.ToString());
            return;
        }

        if (action == ItemAction.Use)
        {
            OnUse();
        }
        else if (action == ItemAction.Inspect)
        {
            OnInspect();
        }
        else if (action == ItemAction.Combine)
        {
            OnCombine();
        }
        else if (action == ItemAction.Discard)
        {
            OnDiscard();
        }
        else if (action == ItemAction.Disassemble)
        {
            OnDisassemble();
        }
    }
    bool ValidateAction(ItemAction action)
    {
        bool validAction = false;
        if (action == ItemAction.Use)
        {
            if (currentSelectedItem.canUse)
                validAction = true;
        }
        else if (action == ItemAction.Inspect)
        {
            if (currentSelectedItem.canInspect)
                validAction = true;
        }
        else if (action == ItemAction.Combine)
        {
            if (currentSelectedItem.canCombine)
                validAction = true;
        }
        else if (action == ItemAction.Discard)
        {
            if (currentSelectedItem.canDiscard)
                validAction = true;
        }
        else if (action == ItemAction.Disassemble)
        {
            if (currentSelectedItem.canDisassemble)
                validAction = true;
        }

        return validAction;
    }

    public void OnUse()
    {
        if (currentSelectedItem.GetComponent<ItemBehaviour>() != null)
            currentSelectedItem.GetComponent<ItemBehaviour>().OnUse();
        else
            Debug.LogError("This item did not implement itembehaviour component");
    }

    public void OnInspect()
    {

    }

    public void OnCombine()
    {
        if (mode == InventoryMode.NA)
        {
            Mode = InventoryMode.Combine;

            ValidateSelectItem(currentSelectedItem);
            SelectItem(currentSelectedItem);

            //UpdateHighlightPosition(inventoryItemTemp);

            UpdatePrevItem(currentSelectedItem);
            UpdateChangeSelectedItem();
        }
        else if (mode == InventoryMode.Combine)
        {
            if (currentCombineItem == currentSelectedItem)
                InventoryMessage = "Cannot combine the same item";
            else
            {
                if (currentCombineItem.CheckCombineItem(currentSelectedItem))
                {
                    if (!spawnAtLastDestroy)
                    {
                        foreach (Item item in currentCombineItem.combineResult)
                            Add(item);
                        Remove(currentCombineItem);
                        Remove(currentSelectedItem);
                    }
                    else
                    {
                        Remove(currentSelectedItem);
                        foreach (Item item in currentCombineItem.combineResult)
                            Add(item);
                        Remove(currentCombineItem);
                    }
                }
                else
                    InventoryMessage = "Failed to combine item = " + currentSelectedItem.itemName + ", with item = " + currentCombineItem.itemName;
            }
        }
    }

    public void OnDiscard()
    {

    }

    public void OnDisassemble()
    {

    }

    public void OnCanel()
    {
        if (mode == InventoryMode.Combine)
        {
            Mode = InventoryMode.NA;

            ValidateSelectItem(currentCombineItem);
            DeselectOldItem();
            SelectItem(currentCombineItem);
            UpdatePrevItem(currentCombineItem);
            UpdateChangeSelectedItem();
            CurrentCombineItem = null;

        }

    }
}
