using System;
using System.Collections.Generic;
using UnityEngine;

// Базовый класс для всех состояний
public abstract class GameState
{
    protected GameStateMachine stateMachine;
    
    public GameState(GameStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void LateUpdate() { }
}

// Класс конечного автомата
public class GameStateMachine
{
    private Dictionary<Type, GameState> states;
    private GameState currentState;
    
    public GameState CurrentState => currentState;
    
    public GameStateMachine()
    {
        states = new Dictionary<Type, GameState>();
    }
    
    public void AddState(GameState state)
    {
        states.Add(state.GetType(), state);
    }
    
    public void ChangeState<T>() where T : GameState
    {
        if (states.TryGetValue(typeof(T), out GameState newState))
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
            
            Debug.Log($"[StateMachine] State changed to: {typeof(T).Name}");
        }
        else
        {
            Debug.LogError($"[StateMachine] State {typeof(T).Name} not found!");
        }
    }
    
    public void Update()
    {
        currentState?.Update();
    }
    
    public void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }
    
    public void LateUpdate()
    {
        currentState?.LateUpdate();
    }
}