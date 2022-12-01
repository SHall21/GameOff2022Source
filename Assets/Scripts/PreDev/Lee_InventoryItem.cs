using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Lee_InventoryItem
{
    public Lee_ItemData itemData;

    public Lee_InventoryItem(Lee_ItemData item)
    {
        itemData = item;
    }
}
