using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryControl : MonoBehaviour
{
    [Header("Reference to own Object")]
    public InventoryManager inventoryManager;

    [Header("Attribute")]
    public KeyCode useKey = KeyCode.E;
    public KeyCode inspectKey = KeyCode.R;
    public KeyCode combineKey = KeyCode.T;
    public KeyCode discardKey = KeyCode.Y;
    public KeyCode disassembleKey = KeyCode.U;
    public KeyCode cancelKey = KeyCode.I;

    

    public Control control = Control.Arrow;
    public enum Control { WASD, Arrow };

    [Header("Data, should be private")]
    public bool canControlInventory = true;

    public enum Direction { Up, Down, Left, Right }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryManager.ToggleInventory();
        }

        ControlInventory();
        SelectAction();
    }

    void ControlInventory()
    {
        if (!canControlInventory)
            return;

        if (inventoryManager.ShowInventory)
        {
            if (control == Control.WASD)
            {
                if (Input.GetKeyDown(KeyCode.W))
                    SelectItem(Direction.Up);
                else if (Input.GetKeyDown(KeyCode.A))
                    SelectItem(Direction.Left);
                else if (Input.GetKeyDown(KeyCode.S))
                    SelectItem(Direction.Down);
                else if (Input.GetKeyDown(KeyCode.D))
                    SelectItem(Direction.Right);
            }
            else if (control == Control.Arrow)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                    SelectItem(Direction.Up);
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    SelectItem(Direction.Left);
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                    SelectItem(Direction.Down);
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                    SelectItem(Direction.Right);
            }
        }
    }

    void SelectItem(Direction direction)
    {
        //var newPosition = inventoryManager.HighlightPosition;
        var newPosition = inventoryManager.CurrentSelectedItem.inventoryPosition;
        if (newPosition.x == -1)//if position never initialize, select 1st item
        {
            if (inventoryManager.inventoryItemList.Count > 0)
            {
                Debug.LogError("Rarely enter here");
                inventoryManager.SelectItemViaKeyboard(inventoryManager.inventoryItemList[0]);
            }
        }
        else
        {
            if (direction == Direction.Left)
                newPosition.x--;
            else if (direction == Direction.Right)
                newPosition.x++;
            else if (direction == Direction.Up)//y=0 means top
                newPosition.y--;
            else if (direction == Direction.Down)
                newPosition.y++;

            //Debug.Log("Finding item at position = " + newPosition);
            var item = inventoryManager.FindInventoryItem(newPosition);
            if (item != null)
                inventoryManager.SelectItemViaKeyboard(item);
        }
    }

    void SelectAction()
    {
        if (Input.GetKeyDown(useKey))
            inventoryManager.SelectAction(InventoryManager.ItemAction.Use);
        else if (Input.GetKeyDown(inspectKey))
            inventoryManager.SelectAction(InventoryManager.ItemAction.Inspect);
        else if (Input.GetKeyDown(combineKey))
            inventoryManager.SelectAction(InventoryManager.ItemAction.Combine);
        else if (Input.GetKeyDown(discardKey))
            inventoryManager.SelectAction(InventoryManager.ItemAction.Discard);
        else if (Input.GetKeyDown(disassembleKey))
            inventoryManager.SelectAction(InventoryManager.ItemAction.Disassemble);
    }
}

