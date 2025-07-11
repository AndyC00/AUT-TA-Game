using UnityEngine;


[RequireComponent(typeof(NPC))]
public class Guide : MonoBehaviour
{
    private NPC npcScript;
    [SerializeField] private string[] firstStageConversation;
    [SerializeField] private string[] secondStageConversation;

    void Start()
    {
        npcScript = GetComponent<NPC>();
        if (npcScript != null && npcScript.conversationContent != null)
        {
            Debug.Log("conversation strings on NPC script are empty!");
        }
    }

    private void OnFirstStageComplete()
    {
        npcScript.conversationContent = firstStageConversation;
    }

    private void OnSecondStageComplete()
    {
        npcScript.conversationContent = secondStageConversation;
    }
}
