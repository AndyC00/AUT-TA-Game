using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // Resource Count and update
    [SerializeField] private int _resourceCount;
    public event Action<int> OnResourceCountChanged;
    public int ResourceCount
    {
        get => _resourceCount;
        set
        {
            if (_resourceCount == value) return;

            _resourceCount = value;
            OnResourceCountChanged?.Invoke(_resourceCount);
        }
    }

    // Game States
    public enum GameState
    {
        Start,
        FirstStage,
        SecondStage,
        ThirdStage
    }

    public UnityEvent<GameState> OnStateChanged;
    private GameState _currentState;

    // singleton pattern
    public static GameManager instance { get; private set; }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _currentState = GameState.Start;
        _resourceCount = 0;
    }

    // --------------- State Management ---------------
    public void ChangeState(GameState newState)
    {
        if (_currentState == newState) { return; }

        ExitState(_currentState);
        _currentState = newState;
        EnterState(newState);
        OnStateChanged?.Invoke(newState);

        Debug.Log($"Game state has changed to: {newState}");
    }

    private void EnterState(GameState state)
    { 
        switch(state)
        { 
            case GameState.FirstStage:
                Guide.instance.OnFirstStageComplete();
                break;
            case GameState.SecondStage:
                Guide.instance.OnSecondStageComplete();
                break;
            case GameState.ThirdStage:
                Guide.instance.OnThirdStageComplete();
                break;
        }
    }

    private void ExitState(GameState state)
    {
        //switch (state)
        //{ 
        // use for unsubscribing events or cleaning up resources
        //}
    }

    public void CheckStateChange()     // triggered by resource count change
    {
        switch (_resourceCount)
        {
            case > 100 and < 200:
                ChangeState(GameState.FirstStage);
                break;
            case >= 200 and < 300:
                ChangeState(GameState.SecondStage);
                break;
            case >= 300:
                ChangeState(GameState.ThirdStage);
                break;
        }
    }

    public GameState GetCurrentState()
    {
        return _currentState;
    }
}
