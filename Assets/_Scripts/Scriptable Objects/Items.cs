using UnityEngine;
using System;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Items", menuName = "Scriptable Objects/Items")]
public class Item : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("Descriptors")]
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;

    [Header("Sprites and Prefabs")]
    public GameObject spriteTrapPrefab;
    public GameObject itemPrefab;
    public GameObject handPrefab;

    [Header("Item Type")]
    public bool isTrap;

    public bool isMaterial;

    [Header("Stacking Parameters")]
    public bool canStack;
    public int baseQuantity;
    public int stackLimit;
    [System.NonSerialized]
    public int combinedQuantity;

    public void OnAfterDeserialize()
    {
        combinedQuantity = baseQuantity;
    }
    public void OnBeforeSerialize()
    {

    }
}
