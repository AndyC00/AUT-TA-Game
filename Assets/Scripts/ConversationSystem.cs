using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ConversationSystem : MonoBehaviour
{
    //UI elements
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI conversationText;
    private Button nextButton;
    private Image characterImage;

    //conversation data:
    private int conversationIndex;
    private Action<bool> onConversationEnd;

    private List<ConversationNode> conversationNodes;
    private ConversationNode currentNode;
    private Transform Multi_ChoiceDialogue;
    public Transform optionsContainer;
    public List<Button> optionButtons = new List<Button>();

    //NPC
    private string npcName;
    private NPC currentNPC;

    //singleton pattern
    public static ConversationSystem Instance { get; private set; }
    [System.Serializable]
    public class ConversationNode
    {
        public string dialogueText;
        public List<ConversationOption> options;

        public bool HasOptions()
        {
            return options != null && options.Count > 0;
        }
    }

    [System.Serializable]
    public class ConversationOption
    {
        public string optionText;
        public int nextNodeIndex;
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        //get all the conversation UI components
        nameText = transform.Find("NameTextBg/NameText").GetComponent<TextMeshProUGUI>();
        conversationText = transform.Find("ConversationText").GetComponent<TextMeshProUGUI>();
        characterImage = transform.Find("CharacterImage").GetComponent<Image>();

        nextButton = transform.Find("NextButton").GetComponent<Button>();
        nextButton.onClick.AddListener(this.OnNextButtonClick);

        //get all the multi-choice buttons in the options container
        optionsContainer = transform.Find("Multi_ChoiceDialogue/Grid");
        Multi_ChoiceDialogue = transform.Find("Multi_ChoiceDialogue");

        foreach (Transform child in optionsContainer)
        {
            Button btn = child.GetComponent<Button>();
            if (btn != null)
            {
                optionButtons.Add(btn);
                btn.gameObject.SetActive(false);
            }
        }

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Show(NPC npc, string name, ConversationNode[] nodes, Sprite image, Action<bool> onEndCallback = null)
    {
        currentNPC = npc;

        npcName = name;
        nameText.text = name;
        characterImage.sprite = image;

        conversationNodes = new List<ConversationNode>(nodes);
        conversationIndex = 0;
        currentNode = conversationNodes[conversationIndex];
        conversationText.text = currentNode.dialogueText;

        onConversationEnd = onEndCallback;

        gameObject.SetActive(true);
    }

    public void Hide(bool isCancelled = false)
    {
        gameObject.SetActive(false);

        if (currentNPC != null)
        {
            currentNPC.SetIsTalking(false);
            currentNPC = null;
        }

        onConversationEnd?.Invoke(isCancelled);
        onConversationEnd = null;
    }

    private void OnNextButtonClick()
    {
        throw new NotImplementedException();
    }

    public int GetCurrentConversationIndex()
    {
        return conversationIndex;
    }
}
