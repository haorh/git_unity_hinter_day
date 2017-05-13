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
    // Use this for initialization
    void Start () {
		
	}
	
	protected override void OnInitialize(Item item)
    {
        Debug.Log("Subclass initialize");
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
}
