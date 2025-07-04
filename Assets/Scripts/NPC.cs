using UnityEngine;
using static ConversationSystem;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    public string npcName;
    public ConversationNode[] conversationNodes;
    public Sprite CharacterImage;
    private bool haveInteracted = false;
    private Animator animator;

    private void Start()
    {


    }

    //show conversation UI once NPC touch the player
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !haveInteracted)
        {
            haveInteracted = true;

            if (animator != null)
            {
                animator.SetBool("isTalking", true);
            }

            System.Action<bool> onConversationEnd = null;

            //trigger the cardFacility UI after the conversation with the Library
            if (npcName == "Library")
            {
                onConversationEnd = (bool isCancelled) =>
                {
                    haveInteracted = false;
                    if (!isCancelled)
                    {
                        //CardFacilityUI.instance.Show();
                    }
                };
            }
            else
            {
                onConversationEnd = (bool isCancelled) =>
                {
                    haveInteracted = false;
                };
            }

            ConversationSystem.Instance.Show(this, npcName, conversationNodes, CharacterImage, onConversationEnd);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ConversationSystem.Instance.Hide();

            if (animator != null)
            {
                animator.SetBool("isTalking", false);
            }
        }
    }

    public void SetIsTalking(bool isTalking)
    {
        if (animator != null)
        {
            animator.SetBool("isTalking", isTalking);
        }
    }

    public void ResumeConversation(int startIndex)
    {
        if (animator != null)
        {
            animator.SetBool("isTalking", true);
        }

        System.Action<bool> onConversationEnd = (bool isCancelled) =>
        {
            haveInteracted = false;

            if (animator != null)
            {
                animator.SetBool("isTalking", false);
            }
        };

        ConversationSystem.Instance.Show(this, npcName, conversationNodes, CharacterImage, onConversationEnd);
    }
}
