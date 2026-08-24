using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public struct Ingredients
{
    public Item item;
    public int quantity;
   
}
[CreateAssetMenu(fileName = "ItemRecipes", menuName = "Scriptable Objects/ItemRecipes")]
public class ItemRecipes : ScriptableObject
{
    public List<Ingredients> recipe;
    public Item output;
}
