using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] firstStageObjects;
    [SerializeField] private GameObject[] secondStageObjects;
    [SerializeField] private GameObject[] thirdStageObjects;
    [SerializeField] private GameObject[] finalObjects;

    // Resource Count and update
    [SerializeField] private int _resourceCount;
    [HideInInspector] public event Action<int> OnResourceCountChanged;
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
                onFirstStageEnvironmentChange();
                break;
            case GameState.SecondStage:
                Guide.instance.OnSecondStageComplete();
                onSecondStageEnvironmentChange();
                break;
            case GameState.ThirdStage:
                Guide.instance.OnThirdStageComplete();
                onThirdStageEnvironmentChange();
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

    private void onFirstStageEnvironmentChange()
    { 
        foreach (GameObject gameObject in firstStageObjects)
        {
            gameObject.SetActive(false);
        }

        foreach (GameObject gameObject in secondStageObjects)
        { 
            gameObject.SetActive(true);
        }
    }

    private void onSecondStageEnvironmentChange()
    {
        foreach(GameObject gameObject in secondStageObjects)
        {
            gameObject.SetActive(false);
        }
        foreach (GameObject gameObject in thirdStageObjects)
        {
            gameObject.SetActive(true);
        }
    }

    private void onThirdStageEnvironmentChange()
    {
        foreach (GameObject gameObject in finalObjects)
        {
            gameObject.SetActive(true);
        }
    }
}
