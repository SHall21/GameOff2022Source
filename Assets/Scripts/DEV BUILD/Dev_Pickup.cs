using UnityEngine;

public class Dev_Pickup : MonoBehaviour, Dev_ICollectable
{
    public static event HandleCollection OnPickupCollected;
    public delegate void HandleCollection(Dev_ItemData itemData);
    public Dev_ItemData pickData;

    public void Collect()
    {
        //Debug.Log("Object is stolen");
        OnPickupCollected?.Invoke(pickData);
    }
}
