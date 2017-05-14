using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : Item {
    [Header("Reference to own Object")]
    public Image baseImage;
    public Image image;
    public Button button;

    public Color selectedColour = Color.green;
    public Color deselectedColour = Color.white;

    [Header("Its position in inventory")]
    public Vector2 inventoryPosition;
    // Use this for initialization
    void Start () {
        inventoryPosition = new Vector2(-1, -1);

    }
	
	protected override void OnInitialize(Item item)
    {
        //Debug.Log("Subclass initialize");
        image.sprite = item.sprite;
    }

    public void Selected()
    {
        baseImage.color = selectedColour;
    }
    public void Deselected()
    {
        baseImage.color = deselectedColour;
    }

    //public void SetPosition(Vector2 position)
    //{
    //    this.position.x = position.x;
    //    this.position.y = position.y;
    //}
}
