using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item Data")]
public class Lee_ItemData : ScriptableObject
{
    public string id;
    public string displayName;
    public string clue;
    public Sprite icon;
}
