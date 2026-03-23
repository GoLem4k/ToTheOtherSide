using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    public string id;
    public GameObject prefab;
    //public float interactionRange;
    public Sprite icon;
    public ItemType type; // enum, если нужно разграничение
    public ItemType destroyedByType;
    public ItemType metamorphosisByType;
    public GameObject metamorphosisInto;
}

public enum ItemType
{
    None = -1,
    Box = 0,
    Grain = 1,
    Chicken = 2,
    Fox = 3,
    Nut = 4,
    PeeledNut = 5,
    Squirrel = 6
}