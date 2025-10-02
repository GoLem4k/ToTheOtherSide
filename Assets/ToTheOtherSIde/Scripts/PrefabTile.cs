using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Custom/Prefab Tile")]
public class PrefabTile : TileBase
{
    public GameObject prefab;
    public Sprite previewSprite; // картинка для Tile Palette

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = previewSprite; // показывается в Tilemap/Palette
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (prefab != null)
        {
            Tilemap realTilemap = tilemap.GetComponent<Tilemap>();
            if (realTilemap != null)
            {
                Vector3 worldPos = realTilemap.CellToWorld(position) + realTilemap.tileAnchor;
                var obj = GameObject.Instantiate(prefab, worldPos, Quaternion.identity);
                obj.transform.SetParent(realTilemap.transform, true);
            }
        }
        return true;
    }
}