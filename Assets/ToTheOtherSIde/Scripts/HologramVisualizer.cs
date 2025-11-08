 using UnityEngine;

public class HologramVisualizer : MonoBehaviour
{
    public Material hologramMaterial;
    private GameObject currentHologram;

    public void Show(ItemData item, Transform targetPoint)
    {
        if (currentHologram == null)
        {
            currentHologram = Instantiate(item.prefab);
            ApplyMaterial(currentHologram);
        }

        currentHologram.transform.position = targetPoint.position;
        currentHologram.transform.rotation = targetPoint.rotation;
        currentHologram.SetActive(true);
    }

    public void Hide()
    {
        if (currentHologram != null)
            currentHologram.SetActive(false);
    }

    private void ApplyMaterial(GameObject obj)
    {
        foreach (var renderer in obj.GetComponentsInChildren<Renderer>())
            renderer.material = hologramMaterial;
    }
}