using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This line allows us to create a new Item element to create instances of this scriptable object
[CreateAssetMenu(fileName = "New item", menuName = "Item")]
public class Item : ScriptableObject
{
    public string objectName;
    public Sprite sprite;
    public int quantity;
    public bool stackable;
    public enum ItemType {BIG_HEAL, MEDIUM_HEAL, SMALL_HEAL, INJECTION};
    public ItemType itemType;
}
