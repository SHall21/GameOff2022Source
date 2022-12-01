using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Time Data")]
public class Dev_TimeData : ScriptableObject
{
    [SerializeField] public string id;
    [SerializeField] public Sprite trophy;
    [SerializeField] public bool isLevelComplete;
    [SerializeField] private int _value;

    public int Value
    {
        get { return _value; }
        set { _value = value; }
    }
}
