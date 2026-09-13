using UnityEngine;
using System;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Items", menuName = "Scriptable Objects/Items")]
public class Item : ScriptableObject
{
    [Header("Descriptors")]
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;

    [Header("Stacking Parameters")]
    public bool canStack;
    public int baseQuantity;
    public int stackLimit;


}
