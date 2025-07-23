using UnityEngine;


[RequireComponent(typeof(NPC))]
public class ConversationDatabase : MonoBehaviour
{
    public string[] firstStageConversation;
    public string[] firstStageShort;
    public string[] secondStageConversation;
    public string[] secondStageShort;
    public string[] thirdStageConversation;
    public string[] thirdStageShort;
}
