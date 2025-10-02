using UnityEngine;
using UnityEngine.Tilemaps;

public class TileHighlighter : MonoBehaviour
{
    private Grid grid;
    [SerializeField] private Tilemap tileMap = null;
    [SerializeField] private GameObject hoverPrefab = null;

    private GameObject currentHighlight;
    private Vector3Int previousMousePos = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);

    void Start() {
        grid = GetComponent<Grid>();
    }

    void Update() {
        Vector3Int? mousePos = GetMouseCell();

        if (mousePos.HasValue && mousePos.Value != previousMousePos) 
        {
            if (currentHighlight != null)
                Destroy(currentHighlight);

            Vector3 worldPos = grid.GetCellCenterWorld(mousePos.Value);
            Quaternion rotation = Quaternion.Euler(-90, 0, 0); // повернуть на -90° по X
            currentHighlight = Instantiate(hoverPrefab, worldPos, rotation, tileMap.transform);


            previousMousePos = mousePos.Value;
        }
    }

    Vector3Int? GetMouseCell() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit)) {
            // Попали в плоскость/объект
            return grid.WorldToCell(hit.point);
        }

        return null;
    }
}