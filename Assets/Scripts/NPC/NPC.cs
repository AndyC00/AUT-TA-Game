using UnityEngine;
using static ConversationSystem;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    [SerializeField] private string npcName;
    [SerializeField] private Sprite CharacterImage;
    public string[] conversationContent;

    //show conversation UI once NPC touch the player
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") )
        {
            switch(transform.name)
            { 
                case "Guide":
                    Guide.instance.LoadConversation();
                    Guide.instance.hasTalked = true;
                    break;
            }

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
}
