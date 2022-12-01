using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dev_Inventory : MonoBehaviour
{
    public UnityEvent CompletedList;
    public List<Dev_InventoryItem> inventory = new List<Dev_InventoryItem>();
    [SerializeField] public List<string> ShoppingList;
    Dev_SceneSession sceneSession;

    void Awake()
    {
        sceneSession = FindObjectOfType<Dev_SceneSession>();
    }

    void Update()
    {
        CheckCompleted();
    }

    private void OnEnable()
    {
        Dev_Pickup.OnPickupCollected += AddInventory;
    }

    private void OnDisable()
    {
        Dev_Pickup.OnPickupCollected -= AddInventory;
    }

    public void AddInventory(Dev_ItemData itemData)
    {
        Dev_InventoryItem newItem = new Dev_InventoryItem(itemData);
        inventory.Add(newItem);
    }

    public void RemoveInventory()
    {
        inventory.Clear();
    }

    public bool IsOnList (string id)
    {
        if (ShoppingList.Contains(id))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CheckCompleted()
    {
        if (ShoppingList.Count == 0) {
            CompletedList?.Invoke();
        }
    }
}
