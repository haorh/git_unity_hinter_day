using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.ComponentModel;

public class InventoryItem : Item, INotifyPropertyChanged
{
    [Header("Reference to own Object")]

    public int amount;
    public bool infinityUsage = false;

    //public Sprite image;
    public Button button;

    bool isSelected;

    public event PropertyChangedEventHandler PropertyChanged;

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
    
    //public Sprite Image
    //{
    //    get
    //    {
    //        return image;
    //    }
    //    set
    //    {
    //        image = value;
    //        OnPropertyChanged("Image");
    //    }
    //}

    public bool IsSelected
    {
        get
        {
            return isSelected;
        }
        set
        {
            isSelected = value;
            OnPropertyChanged("IsSelected");
        }
    }

    [Header("Its position in inventory")]
    public Vector2 inventoryPosition;
    // Use this for initialization
    void Start () {
        inventoryPosition = new Vector2(-1, -1);
    }
	
	protected override void OnInitialize(Item item)
    {
        OnPropertyChanged("Image");
        
    }

    public void Selected()
    {
        IsSelected = true;
        
    }
    public void Deselected()
    {
        IsSelected = false;        
    }
    public void Used()
    {
        amount--;
    }
    //public void SetPosition(Vector2 position)
    //{
    //    this.position.x = position.x;
    //    this.position.y = position.y;
    //}
}
