using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }
    
    public void LoadResources()
    {
        Debug.Log("Загрузка ресурсов...");
    }
}