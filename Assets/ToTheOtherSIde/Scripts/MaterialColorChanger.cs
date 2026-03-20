using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Renderer))]
public class MaterialColorChanger : MonoBehaviour
{
    [Header("Настройки материала")]
    [SerializeField] private string materialPath = "Materials/Hologram";
    
    private Renderer objectRenderer;
    private Material materialInstance;

    // Этот метод вызывается при изменении значений в инспекторе
    private void OnValidate()
    {
        objectRenderer = GetComponent<Renderer>();
        Material originalMaterial = Resources.Load<Material>(materialPath);
        materialInstance = new Material(originalMaterial);
        materialInstance.SetColor("_Main_Color", new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f));
        objectRenderer.material = materialInstance;
    }

    private void Start()
    {
        objectRenderer.enabled = false;
    }
}