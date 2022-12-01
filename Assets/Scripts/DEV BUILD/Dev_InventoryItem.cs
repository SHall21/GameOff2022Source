using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Dev_InventoryItem
{
    public Dev_ItemData itemData;

    public Dev_InventoryItem(Dev_ItemData item)
    {
        itemData = item;
    }
}
