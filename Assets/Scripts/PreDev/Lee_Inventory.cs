using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lee_Inventory : MonoBehaviour
{
    public List<Lee_InventoryItem> inventory = new List<Lee_InventoryItem>();

    private void OnEnable()
    {
        Lee_Pickup.OnPickupCollected += AddInventory;
    }

    private void OnDisable()
    {
        Lee_Pickup.OnPickupCollected -= AddInventory;
    }

    public void AddInventory(Lee_ItemData itemData)
    {
        Lee_InventoryItem newItem = new Lee_InventoryItem(itemData);
        inventory.Add(newItem);
    }

    // public void UpdatePointValue(int value)
    // {
    //     inventory[0].itemData.pointValue = value;
    // }

    public void RemoveInventory()
    {
        //Debug.Log("Clear inventory");
        inventory.Clear();
    }
}
