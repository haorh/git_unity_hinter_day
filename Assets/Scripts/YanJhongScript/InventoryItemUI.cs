using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    public InventoryItem inventoryItem;

    public Image baseImage;
    public Image image;

    public Color selectedColour = Color.green;
    public Color deselectedColour = Color.white;

    private void Awake()
    {
        inventoryItem.PropertyChanged += UpdateUI;
    }
    private void OnDestroy()
    {
        inventoryItem.PropertyChanged -= UpdateUI;
    }

    void UpdateUI(object sender, PropertyChangedEventArgs e)
    {
        //Debug.Log("In InventoryUI, e.PropertyName = " + e.PropertyName);
        if (e.PropertyName == MemberInfoGetting.GetMemberName(() => inventoryItem.IsSelected))
            UpdateIsSelected();
        else if (e.PropertyName == "Image")
            UpdateImage();
        else if (e.PropertyName == "all")
            UpdateAll();
    }

    void UpdateImage()
    {
        //Debug.Log("Here");
        image.sprite = inventoryItem.sprite;
    }
    void UpdateIsSelected()
    {
        if (inventoryItem.IsSelected)
        {
            baseImage.color = selectedColour;
        }
        else
        {
            baseImage.color = deselectedColour;
        }
    }

    void UpdateAll()
    {
        UpdateImage();
        UpdateIsSelected();
    }
}
