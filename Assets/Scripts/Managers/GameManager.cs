using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] firstStageObjects;
    [SerializeField] private GameObject[] secondStageObjects;
    [SerializeField] private GameObject[] thirdStageObjects;
    [SerializeField] private GameObject[] finalObjects;

    const int firstStageResourceCount = 80;
    const int secondStageResourceCount = 170;
    const int thirdStageResourceCount = 300;

    [SerializeField] private GameObject globalLight;
    private Light2D globalLightComponent;

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
            CheckStateChange();
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
    private GameState _currentState;
    [HideInInspector] public UnityEvent<GameState> OnStateChanged;

    private List<NPC> npcs;

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

        globalLight ??= GameObject.Find("GlobalLight");
        globalLightComponent = globalLight.GetComponent<Light2D>();

        GetAllNPCs();
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
                LoadNpcConversationsOnFirstStage();
                onFirstStageEnvironmentChange();
                break;
            case GameState.SecondStage:
                LoadNpcConversationsOnSecondStage();
                onSecondStageEnvironmentChange();
                break;
            case GameState.ThirdStage:
                LoadNpcConversationsOnThirdStage();
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
            case >= firstStageResourceCount and < secondStageResourceCount:
                ChangeState(GameState.FirstStage);
                break;
            case >= secondStageResourceCount and < thirdStageResourceCount:
                ChangeState(GameState.SecondStage);
                break;
            case >= thirdStageResourceCount:
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

        // changed to noon:
        globalLightComponent.color = Color.white;
        globalLightComponent.intensity = 1.2f;
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

        // changed to afternoon:
        globalLightComponent.color = new Color(1f, 0.8f, 0.6f);
        globalLightComponent.intensity = 1.0f;
    }

    private void onThirdStageEnvironmentChange()
    {
        foreach (GameObject gameObject in finalObjects)
        {
            gameObject.SetActive(true);
        }

        // changed to evening:
        globalLightComponent.color = Color.white;
        globalLightComponent.intensity = 0.5f;

        MoveSoldierToTower();
    }

    private void GetAllNPCs()
    {
        npcs = new List<NPC>();

        GameObject[] allNpcs = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject npc in allNpcs)
        { 
            NPC script = npc.GetComponent<NPC>();
            if (script != null)
            {
                npcs.Add(script);
            }
        }
    }

    private void LoadNpcConversationsOnFirstStage()
    {
        foreach (NPC npc in npcs)
        {
            npc.OnFirstStageComplete();
        }
    }

    private void LoadNpcConversationsOnSecondStage()
    {
        foreach (NPC npc in npcs)
        {
            npc.OnSecondStageComplete();
        }
    }

    private void LoadNpcConversationsOnThirdStage()
    {
        foreach (NPC npc in npcs)
        {
            npc.OnThirdStageComplete();
        }
    }

    private void MoveSoldierToTower()
    { 
        GameObject soldier = GameObject.Find("Soldier_Grey");
        soldier.transform.position = new Vector3(8.3f, -0.35f, 0);
    }
}
