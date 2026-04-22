using System.Collections;
using UnityEngine;

// Состояние инициализации игры
public class LoadLevel : GameState
{
    private float loadingProgress = 0f;
    private bool isLoadingComplete = false;
    
    private GameObject currentLevel = null;
    private GameObject player = null;
    
    public LoadLevel(GameStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        Debug.Log("[InitState] Enter - Загрузка уровня...");
        
        // Сбрасываем флаги
        loadingProgress = 0f;
        isLoadingComplete = false;
        
        // Начинаем инициализацию
        Load();
    }
    
    public override void Exit()
    {
        Debug.Log("[InitState] Exit - Инициализация завершена");
    }
    
    public override void Update()
    {
        if (!isLoadingComplete)
        {
            // Обновляем прогресс загрузки
            loadingProgress += Time.deltaTime * 0.5f;
            
            if (loadingProgress >= 1f)
            {
                loadingProgress = 1f;
                isLoadingComplete = true;
                
                // После завершения инициализации переходим в Gameplay
                stateMachine.ChangeState<GameplayGameState>();
            }
            
            UIManager.Instance?.UpdateLoadingProgress(loadingProgress);
        }
    }
    
    public void Load()
    {
        Debug.Log("[LoadLevel] Загрузка...");
        
        //G.GameManager.CurrentLevel = GameObject.Instantiate(G.GameManager.GetCurrentLevelPrefab());
        // Спавн игрока
        SpawnPlayer();
        
        Debug.Log("[LoadLevel] Уровень загружен");
    }
    
    private void SpawnPlayer()
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

// Состояние геймплея
public class GameplayGameState : GameState
{
    private bool isPaused = false;
    private float gameTime = 0f;
    
    public GameplayGameState(GameStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        Debug.Log("[GameplayState] Enter - Начало игры");
        
        // Скрываем экран загрузки
        UIManager.Instance?.HideLoadingScreen();
        
        // Показываем игровой интерфейс
        UIManager.Instance?.ShowGameplayUI();
        
        // Настройка игровых систем
        Time.timeScale = 1f;
        gameTime = 0f;
        isPaused = false;
    }
    
    public override void Exit()
    {
        Debug.Log("[GameplayState] Exit - Выход из игры");
        
        // Сохраняем прогресс
        //SaveGameProgress();
        
        // Очищаем игровые объекты
        //ClearGameplayObjects();
    }
    
    public override void Update()
    {
        if (isPaused) return;
        
        // Обновляем игровое время
        gameTime += Time.deltaTime;
        
        // Проверка ввода
        HandleInput();
        
        // Обновление UI
        UpdateUI();
    }
    
    private void HandleInput()
    {
        // Пауза
        if (Input.GetKeyDown(KeyCode.P))
        {
            stateMachine.ChangeState<PauseGameState>();
        }
        
        // Другая логика ввода...
    }
    
    
    private void UpdateUI()
    {
        // Форматируем время в минуты:секунды
        int minutes = Mathf.FloorToInt(gameTime / 60f);
        int seconds = Mathf.FloorToInt(gameTime % 60f);
        string timeString = $"{minutes:00}:{seconds:00}";
        
        UIManager.Instance?.UpdateGameplayUI(timeString);
    }
    
    private void SaveGameProgress()
    {
        // Сохраняем прогресс
        SaveSystem.SaveGame("save_slot_1", gameTime);
        Debug.Log("[GameplayState] Прогресс сохранен");
    }
    
    private void ClearGameplayObjects()
    {
        // Находим и удаляем все игровые объекты
        GameObject[] gameplayObjects = GameObject.FindGameObjectsWithTag("Gameplay");
        foreach (GameObject obj in gameplayObjects)
        {
            GameObject.Destroy(obj);
        }
    }
}

// состояние паузы
public class PauseGameState : GameState
{
    public PauseGameState(GameStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        Debug.Log("[PauseState] Enter - Игра на паузе");
        Time.timeScale = 0f;
        UIManager.Instance?.ShowPauseMenu(true);
    }
    
    public override void Exit()
    {
        Debug.Log("[PauseState] Exit - Возврат в игру");
        Time.timeScale = 1f;
        UIManager.Instance?.ShowPauseMenu(false);
    }
    
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Попытка выйти из паузы");
            stateMachine.ChangeState<GameplayGameState>();
        }
    }
}