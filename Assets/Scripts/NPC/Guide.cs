using UnityEngine;


[RequireComponent(typeof(NPC))]
public class Guide : MonoBehaviour
{
    private NPC npcScript;

    [HideInInspector] public bool hasTalked;
    [SerializeField] private string[] firstStageConversation;
    [SerializeField] private string[] firstStageShort;
    [SerializeField] private string[] secondStageConversation;
    [SerializeField] private string[] secondStageShort;
    [SerializeField] private string[] thirdStageConversation;
    [SerializeField] private string[] thirdStageShort;

    // singleton pattern
    public static Guide instance { get; private set; }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        npcScript = GetComponent<NPC>();
        if (npcScript != null && npcScript.conversationContent != null)
        {
            Debug.Log("conversation strings on NPC script are empty!");
        }
    }

    public void OnFirstStageComplete()
    {
        npcScript.conversationContent = firstStageConversation;
    }

    public void OnSecondStageComplete()
    {
        npcScript.conversationContent = secondStageConversation;
    }

    public void OnThirdStageComplete()
    {
        npcScript.conversationContent = thirdStageConversation;
    }

    public void LoadConversation()
    {
        if (!hasTalked) return;

        GameManager.GameState currentState = GameManager.instance.GetCurrentState();
        npcScript.conversationContent = currentState switch
        {
            GameManager.GameState.FirstStage => firstStageShort,
            GameManager.GameState.SecondStage => secondStageShort,
            GameManager.GameState.ThirdStage => thirdStageShort,
            _ => npcScript.conversationContent
        };
    }
}
