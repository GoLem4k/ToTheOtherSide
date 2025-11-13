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
    None,
    Box
}