using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private GameStateMachine gameStateMachine;
    
    [Header("Настройки")]
    [SerializeField] private bool startGameOnAwake = true;
    [SerializeField] private GameState initialState = GameState.Init;
    
    public enum GameState
    {
        Init,
        Gameplay,
        Pause
    }
    
    private void Awake()
    {
        // Синглтон
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Инициализация StateMachine
        InitializeStateMachine();
    }
    
    private void Start()
    {
        if (startGameOnAwake)
        {
            // Запускаем с состояния Init
            gameStateMachine.ChangeState<InitGameState>();
        }
    }
    
    private void InitializeStateMachine()
    {
        gameStateMachine = new GameStateMachine();
        
        // Добавляем все состояния
        gameStateMachine.AddState(new InitGameState(gameStateMachine));
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
        gameStateMachine?.ChangeState<InitGameState>();
    }
    
    // Получение текущего состояния
    public string GetCurrentStateName()
    {
        return gameStateMachine?.CurrentState?.GetType().Name ?? "None";
    }
}