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
        switch (state)
        { 
            
        }
    }


}
