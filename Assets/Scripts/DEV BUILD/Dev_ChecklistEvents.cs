using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dev_ChecklistEvents : MonoBehaviour
{
    [SerializeField] Dev_ItemData SOItem;
    [SerializeField] TextMeshProUGUI itemText;
    [SerializeField] Image itemImage;
    Dev_UIChecklist uIChecklist;

    void Awake()
    {
        uIChecklist = FindObjectOfType<Dev_UIChecklist>();
        MysteryItem(SOItem);
    }

    private void OnEnable()
    {
        Dev_Pickup.OnPickupCollected += CheckItem;
    }

    private void OnDisable()
    {
        Dev_Pickup.OnPickupCollected -= CheckItem;
    }

    public void CheckItem(Dev_ItemData item)
    {
        if (item.id == SOItem.id)
        {
            FoundItem(item);
            uIChecklist.displayPanel = true;
        }
    }

    public void MysteryItem(Dev_ItemData item)
    {
        if (item == null)
        {
            return;
        }

        itemImage.sprite = item.icon;
        itemImage.color = Color.black;
        itemText.text = item.clue;
    }

    public void FoundItem(Dev_ItemData item)
    {
        if (item == null)
        {
            return;
        }

        itemText.enabled = false;
        itemImage.sprite = item.icon;
        itemImage.color = Color.white;
    }
}
