using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Lee_CheckListEvents : MonoBehaviour
{
    [SerializeField] Lee_ItemData SOItem;
    [SerializeField] TextMeshProUGUI itemText;
    [SerializeField] Image itemImage;
    Lee_UIChecklist uIChecklist;

    void Awake()
    {
        uIChecklist = FindObjectOfType<Lee_UIChecklist>();
        MysteryItem(SOItem);
    }

    private void OnEnable()
    {
        Lee_Pickup.OnPickupCollected += CheckItem;
    }

    private void OnDisable()
    {
        Lee_Pickup.OnPickupCollected -= CheckItem;
    }

    public void CheckItem(Lee_ItemData item)
    {
        if (item.id == SOItem.id)
        {
            FoundItem(item);
            uIChecklist.displayPanel = true;
        }
    }

    public void MysteryItem(Lee_ItemData item)
    {
        if (item == null)
        {
            return;
        }

        itemImage.sprite = item.icon;
        itemImage.color = Color.black;
        itemText.text = item.clue;
    }

    public void FoundItem(Lee_ItemData item)
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
