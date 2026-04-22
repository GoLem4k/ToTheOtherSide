using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IService
{
    private GameStateMachine gameStateMachine;
    
    [Header("Настройки")]
    [SerializeField] private bool startGameOnAwake = true;
    [SerializeField] private GameState initialState = GameState.LoadLevel;
    [SerializeField] private LevelList levelList;
    
    private int levelIndex = 0;

    public void LoadNextLevel()
    {  
        IncreaseLevelIndex();
        Debug.Log(levelList.levelsScenes[levelIndex]);
        SceneManager.LoadScene(levelList.levelsScenes[levelIndex]);
        StartCoroutine(SetLoadState());
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene(levelList.levelsScenes[0]);
        StartCoroutine(SetLoadState());
    }
    
    public bool IsMainMenu()
    {
        return SceneManager.GetActiveScene().name == "MainMenu";
    }
    
    public IEnumerator SetLoadState()
    {
        yield return new WaitForSeconds(1f);
        gameStateMachine?.ChangeState<LoadLevel>();
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(levelList.levelsScenes[levelIndex]);
        StartCoroutine(SetLoadState());
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    

    public void IncreaseLevelIndex()
    {
        levelIndex++;
    }
    
    public enum GameState
    {
        LoadLevel,
        Gameplay,
        Pause
    }
    
    public void Init()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        levelIndex = 0;
        levelList = Resources.Load<LevelList>("LevelList/Levels");
        // Инициализация StateMachine
        InitializeStateMachine();
    }
    
    private void Start()
    {
        if (startGameOnAwake)
        {
            // Запускаем с состояния Init
            gameStateMachine.ChangeState<LoadLevel>();
        }
    }
    
    private void InitializeStateMachine()
    {
        gameStateMachine = new GameStateMachine();
        
        // Добавляем все состояния
        gameStateMachine.AddState(new LoadLevel(gameStateMachine));
        gameStateMachine.AddState(new GameplayGameState(gameStateMachine));
        gameStateMachine.AddState(new PauseGameState(gameStateMachine));
        
        Debug.Log("[GameManager] StateMachine инициализирована");
    }
    
    private void Update()
    {
        gameStateMachine?.Update();
    }
    
    private void FixedUpdate()
    {
        gameStateMachine?.FixedUpdate();
    }
    
    private void LateUpdate()
    {
        gameStateMachine?.LateUpdate();
    }
    
    // Публичные методы для смены состояний
    public void StartGame()
    {
        gameStateMachine?.ChangeState<GameplayGameState>();
    }
    
    public void PauseGame()
    {
        gameStateMachine?.ChangeState<PauseGameState>();
    }
    
    public void ResumeGame()
    {
        gameStateMachine?.ChangeState<GameplayGameState>();
    }
    
    public void RestartGame()
    {
        // Сначала в Init, потом автоматически в Gameplay
        gameStateMachine?.ChangeState<LoadLevel>();
    }
    
    // Получение текущего состояния
    public string GetCurrentStateName()
    {
        return gameStateMachine?.CurrentState?.GetType().Name ?? "None";
    }
}