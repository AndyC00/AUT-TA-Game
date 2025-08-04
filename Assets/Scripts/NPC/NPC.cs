using UnityEngine;
using static ConversationSystem;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    private ConversationDatabase appliedDatabase;

    [SerializeField] private string npcName;
    [SerializeField] private Sprite CharacterImage;

    public string[] conversationContent;
    [HideInInspector] public bool hasTalked;

    private bool isInteracting = false;

    private void Start()
    {
        appliedDatabase = transform.GetComponent<ConversationDatabase>();
        hasTalked = false;
    }

    private void Update()
    {
        if (isInteracting && Input.GetKeyDown(KeyCode.E))
        {
            LoadConversation();
            hasTalked = true;
            ConversationSystem.Instance.Show(npcName, CharacterImage, conversationContent);
        }
    }

    //show conversation UI once NPC touch the player
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isInteracting = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInteracting = false;

            ConversationSystem.Instance.Hide();
        }
    }

    public void OnFirstStageComplete()
    {
        conversationContent = appliedDatabase.firstStageConversation;
        hasTalked = false;
    }

    public void OnSecondStageComplete()
    {
        conversationContent = appliedDatabase.secondStageConversation;
        hasTalked = false;
    }

    public void OnThirdStageComplete()
    {
        conversationContent = appliedDatabase.thirdStageConversation;
        hasTalked = false;
    }

    public void LoadConversation()
    {
        if (!hasTalked) return;

        GameManager.GameState currentState = GameManager.instance.GetCurrentState();
        conversationContent = currentState switch
        {
            GameManager.GameState.FirstStage => appliedDatabase.firstStageShort,
            GameManager.GameState.SecondStage => appliedDatabase.secondStageShort,
            GameManager.GameState.ThirdStage => appliedDatabase.thirdStageShort,
            _ => conversationContent
        };
    }
}
