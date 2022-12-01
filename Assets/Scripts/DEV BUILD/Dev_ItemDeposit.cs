using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dev_ItemDeposit : MonoBehaviour
{
    [SerializeField] float waitTimeToChangeState = 0.25f;
    Dev_Inventory inventory;
    Dev_CharacterController controller;
    public UnityEvent OnDropOff;

    void Awake()
    {
        inventory = FindObjectOfType<Dev_Inventory>();
        controller = FindObjectOfType<Dev_CharacterController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && inventory.inventory.Count > 0)
        {
            AddStolenItems(inventory.inventory[0].itemData);
            inventory.RemoveInventory();
            OnDropOff?.Invoke();
            StartCoroutine(ChangeState());
        }
    }

    public void AddStolenItems(Dev_ItemData itemData)
    {
        Dev_InventoryItem newItem = new Dev_InventoryItem(itemData);

        if (inventory.IsOnList(itemData.id))
        {
            inventory.ShoppingList.Remove(itemData.id);
        }
    }

    IEnumerator ChangeState()
    {
        yield return new WaitForSeconds(waitTimeToChangeState);
        controller.NormalStateEvent.Invoke();
    }
}
