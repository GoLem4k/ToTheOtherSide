using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public GameObject ActiveRail;
    public GameObject NotActiveRail;
    
    private void Awake()
    {
        Instance = this;
    }
    
    public void LoadResources()
    {
        Debug.Log("Загрузка ресурсов...");
        // ActiveRail = Resources.Load("Prefabs/RailSegmentActive");
        // NotActiveRail = Resources.Load("Prefabs/RailSegmentNotActive");
        Debug.Log("Загрузка ресурсов завершена!");
    }
}