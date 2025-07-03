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

    // For use in tutorial scene:
    public bool waitingForKeysInput = false;
    public bool waitingForleftInput = false;
    public bool waitingForRightInput = false;

    private void Start()
    {
        animator = GetComponent<Animator>();

        // trigger tutorial conversation once loaded into the tutorial scene
        if (SceneManager.GetActiveScene().name == "Tutorial Scene")
        {
            TriggerConversation();
        }
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

    private void TriggerConversation()
    {
        if (!haveInteracted)
        {
            haveInteracted = true;

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

        ConversationSystem.Instance.Show(this, npcName, conversationNodes, CharacterImage, onConversationEnd, startIndex);
    }

    // trigger the conversation to resume when the player has pressed all the arrow keys
    public void OnConversationAdvance(int currentIndex)
    {
        if (currentIndex == 3)
        {
            ConversationSystem.Instance.Hide();

            waitingForKeysInput = true;
        }

        if (currentIndex == 4)
        {
            ConversationSystem.Instance.Hide();

            waitingForleftInput = true;
        }

        if (currentIndex == 5)
        {
            ConversationSystem.Instance.Hide();

            waitingForRightInput = true;
        }
    }
}
