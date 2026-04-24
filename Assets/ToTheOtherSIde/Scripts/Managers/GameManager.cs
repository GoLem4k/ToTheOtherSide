using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IService
{
    [Header("Настройки")]
    [SerializeField] private LevelList levelList;
    
    private int levelIndex = 0;

    public void LoadNextLevel()
    {  
        IncreaseLevelIndex();
        G.SceneLoader.Load(levelList.levelsScenes[levelIndex]);
    }

    public void LoadFirstLevel()
    {
        G.SceneLoader.Load(levelList.levelsScenes[0]);
    }
    
    public bool IsMainMenu()
    {
        return SceneManager.GetActiveScene().name == "MainMenu";
    }

    public void ReloadLevel()
    {
        G.SceneLoader.Load(levelList.levelsScenes[levelIndex]);
    }

    public void LoadMainMenu()
    {
        G.SceneLoader.Load("MainMenu");
    }
    

    public void IncreaseLevelIndex()
    {
        levelIndex++;
    }
    
    public void Init()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        levelIndex = 0;
        levelList = Resources.Load<LevelList>("LevelList/Levels");
        G.SceneLoader.onLoadAction += SpawnPlayer;
    }
    
    public void SpawnPlayer()
    {
        if (G.GameManager.IsMainMenu()) return;
        // Поиск точки спавна
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.transform.position : Vector3.zero;
        Quaternion spawnRotation = spawnPoint != null ? spawnPoint.transform.rotation : Quaternion.identity;
        
        // Загрузка префаба игрока
        GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        
        if (playerPrefab != null)
        { 
            Debug.Log(spawnPosition + " " + spawnRotation);
            GameObject.Instantiate(playerPrefab, spawnPosition, spawnRotation);
            Debug.Log("[GameplayState] Игрок создан");
        }
        else
        {
            Debug.LogError("[GameplayState] Префаб игрока не найден!");
        }
    }
}