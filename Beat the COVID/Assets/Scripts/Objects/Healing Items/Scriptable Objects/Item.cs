using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public string objectName;
    public Sprite sprite;
    public int quantity;
    public bool stackable;
    public enum ItemType {BIG_HEAL, MEDIUM_HEAL, SMALL_HEAL, INJECTION};
    public ItemType itemType;
}
