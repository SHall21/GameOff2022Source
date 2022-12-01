using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lee_Alarms : MonoBehaviour
{
    Lee_Inventory inventory;
    int newValue;

    void Awake()
    {
        inventory = FindObjectOfType<Lee_Inventory>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && inventory.inventory.Count > 0)
        {
            //LosingValue(inventory.inventory[0].itemData);
        }
    }

    // public void LosingValue(Lee_ItemData itemData)
    // {
    //     Lee_InventoryItem newItem = new Lee_InventoryItem(itemData);
    //     newValue = newItem.itemData.pointValue;
    //     newValue -= 50;
    //     inventory.UpdatePointValue(newValue);
    //     Debug.Log($"{itemData.displayName} is slowly losing value: {newValue}");
    // }
}
