using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item Data")]
public class Dev_ItemData : ScriptableObject
{
    public string id;
    public string displayName;
    public string clue;
    public Sprite icon;
}
