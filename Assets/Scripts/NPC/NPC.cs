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

    private void Start()
    {
        appliedDatabase = transform.GetComponent<ConversationDatabase>();
        hasTalked = false;
    }

    //show conversation UI once NPC touch the player
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") )
        {
            LoadConversation();
            hasTalked = true;

            ConversationSystem.Instance.Show(npcName, CharacterImage, conversationContent);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ConversationSystem.Instance.Hide();
        }
    }

    public void OnFirstStageComplete()
    {
        conversationContent = appliedDatabase.firstStageConversation;
    }

    public void OnSecondStageComplete()
    {
        conversationContent = appliedDatabase.secondStageConversation;
    }

    public void OnThirdStageComplete()
    {
        conversationContent = appliedDatabase.thirdStageConversation;
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
