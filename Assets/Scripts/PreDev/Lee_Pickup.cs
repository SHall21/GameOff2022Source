using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Lee_Pickup : MonoBehaviour, Lee_ICollectible
{
    public static event HandleCollection OnPickupCollected;
    public delegate void HandleCollection(Lee_ItemData itemData);
    public Lee_ItemData pickData;

    public void Collect()
    {
        //Debug.Log("Object is stolen");
        OnPickupCollected?.Invoke(pickData);
    }
}
