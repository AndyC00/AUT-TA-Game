using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // Game States
    public enum GameState
    {
        Start,
        FirstStage,
        SecondStage,
        ThirdStage
    }

    public UnityEvent<GameState> OnStateChanged;
    private GameState currentState;

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
        currentState = GameState.Start;
    }

    public void ChangeState(GameState newState)
    {
        if (currentState == newState) { return; }

        ExitState(currentState);
        currentState = newState;
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
