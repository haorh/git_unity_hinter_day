using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    public InventoryManager inventoryManager;
    public InventoryControl inventoryControl;

    public GameObject inventoryObj;
    public Text itemTitle;
    public Text itemDescription;

    public GameObject highlight;
    public GameObject combineHighlight;

    public GameObject use, inspect, combine, discard, disassemble, cancel;

    public Text useKey, inspectKey, combineKey, discardKey, disassembleKey, cancelKey;

    private void Awake()
    {
        UpdateModeKey();
        inventoryManager.PropertyChanged += UpdateUI;
    }
    private void OnDestroy()
    {
        inventoryManager.PropertyChanged -= UpdateUI;
    }
    void UpdateModeKey()
    {
        useKey.text = inventoryControl.useKey.ToString();
        inspectKey.text = inventoryControl.inspectKey.ToString();
        combineKey.text = inventoryControl.combineKey.ToString();
        discardKey.text = inventoryControl.discardKey.ToString();
        disassembleKey.text = inventoryControl.disassembleKey.ToString();
        cancelKey.text = inventoryControl.cancelKey.ToString();
    }
    void UpdateUI(object sender, PropertyChangedEventArgs e)
    {
        //Debug.Log("In InventoryUI, e.PropertyName = " + e.PropertyName);
        if (e.PropertyName == MemberInfoGetting.GetMemberName(() => inventoryManager.ShowInventory))
            //UpdateShowInventory();
        UpdateAll();
        else if (e.PropertyName == MemberInfoGetting.GetMemberName(() => inventoryManager.CurrentSelectedItem))
        {
            UpdateItemTitle();
            UpdateItemDescription();
            UpdateHighLight();
            UpdateMode();
        }
        //else if (e.PropertyName == MemberInfoGetting.GetMemberName(() => inventoryManager.HighlightPosition))
        //    UpdateHighLight();
        else if (e.PropertyName == MemberInfoGetting.GetMemberName(() => inventoryManager.CurrentCombineItem))
            UpdateCombineHighLight();
        else if (e.PropertyName == MemberInfoGetting.GetMemberName(() => inventoryManager.Mode))
            UpdateMode();
        else if (e.PropertyName == "all")
            UpdateAll();
    }
    void UpdateAll()
    {
        UpdateShowInventory();
        UpdateItemTitle();
        UpdateItemDescription();
        UpdateHighLight();
        UpdateCombineHighLight();
        UpdateMode();
    }

    void UpdateShowInventory()
    {
        inventoryObj.SetActive(inventoryManager.ShowInventory);
    }
    void UpdateItemTitle()
    {
        if(inventoryManager.CurrentSelectedItem!=null)
        itemTitle.text = inventoryManager.CurrentSelectedItem.itemName;
    }
    void UpdateItemDescription()
    {
        if (inventoryManager.CurrentSelectedItem != null)
            itemDescription.text = inventoryManager.CurrentSelectedItem.description;
    }
    void UpdateHighLight()
    {
        if (inventoryManager.CurrentSelectedItem == null)
            //if (inventoryManager.HighlightPosition == new Vector2(-1, -1))
            highlight.SetActive(false);
        else
        {
            highlight.SetActive(true);
            //highlight.transform.localPosition = inventoryManager.FindInventoryItem(inventoryManager.HighlightPosition).gameObject.transform.localPosition;
            //because currentselected position is only update atthe end of frame

            
            //highlight.transform.localPosition = inventoryManager.CurrentSelectedItem.gameObject.transform.localPosition;
            
            highlight.GetComponent<RectTransform>().anchoredPosition = inventoryManager.CurrentSelectedItem.gameObject.GetComponent<RectTransform>().anchoredPosition;
            //Debug.Log(" position = " + highlight.transform.localPosition);

            if(highlight.GetComponent<RectTransform>().anchoredPosition.x ==0)
            {

                var temp = highlight.GetComponent<RectTransform>().anchoredPosition;
                    temp.x = 42f;
                    temp.y = -55f;

                    highlight.GetComponent<RectTransform>().anchoredPosition = temp;
                    //Debug.Log("++position = " + highlight.transform.localPosition);
                
            }
            //if (highlight.transform.localPosition.x == 0)

        }
    }
    void UpdateCombineHighLight()
    {
        if (inventoryManager.CurrentCombineItem == null)
            combineHighlight.SetActive(false);
        else
        {
            combineHighlight.SetActive(true);
            combineHighlight.transform.localPosition = inventoryManager.CurrentCombineItem.gameObject.transform.localPosition;
        }
    }

    void UpdateMode()
    {
        ResetMode();

        if (inventoryManager.Mode == InventoryManager.InventoryMode.NA)
        {
            UpdateAvailableMode();
        }
        else if (inventoryManager.Mode == InventoryManager.InventoryMode.Combine)
        {
            combine.SetActive(true); cancel.SetActive(true);
        }
        else if (inventoryManager.Mode == InventoryManager.InventoryMode.Inspect)
        {
            cancel.SetActive(true);
        }
    }

    void UpdateAvailableMode()
    {
        if (inventoryManager.CurrentSelectedItem!=null)
        {
            if (inventoryManager.CurrentSelectedItem.canUse)
                use.SetActive(true);
            if (inventoryManager.CurrentSelectedItem.canInspect)
                inspect.SetActive(true);
            if (inventoryManager.CurrentSelectedItem.canDiscard)
                discard.SetActive(true);
            if (inventoryManager.CurrentSelectedItem.canCombine)
                combine.SetActive(true);
            if (inventoryManager.CurrentSelectedItem.canDisassemble)
                disassemble.SetActive(true);
        }
    }

    void ResetMode()
    {
        use.SetActive(false); inspect.SetActive(false); combine.SetActive(false); discard.SetActive(false); disassemble.SetActive(false); cancel.SetActive(false);
    }
}
public static class MemberInfoGetting
{
    public static string GetMemberName<T>(Expression<System.Func<T>> memberExpression)
    {
        MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
        return expressionBody.Member.Name;
    }
}
