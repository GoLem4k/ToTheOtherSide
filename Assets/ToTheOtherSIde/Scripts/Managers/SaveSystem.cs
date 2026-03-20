using UnityEngine;

public static class SaveSystem
{
    public static void Initialize()
    {
        Debug.Log("Инициализация SaveSystem...");
    }
    
    public static void SaveGame(string slot, float time)
    {
        Debug.Log($"Сохранение игры в {slot}, время: {time}");
    }
}