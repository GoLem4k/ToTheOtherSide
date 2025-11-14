using UnityEngine;

[CreateAssetMenu(menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    public string id;
    public GameObject prefab;
    public Sprite icon;
    public ItemType type; // enum, если нужно разграничение
    public ItemType destroyedByType;
}

public enum ItemType
{
    Box = -1,
    None = 0,
    Grain = 1,
    Chicken = 2,
    Fox = 3
}