using UnityEngine;


[RequireComponent(typeof(NPC))]
public class Guide : MonoBehaviour
{
    private NPC npcScript;
    [SerializeField] private string[] firstStageConversation;
    [SerializeField] private string[] secondStageConversation;
    [SerializeField] private string[] thirdStageConversation;

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
}
