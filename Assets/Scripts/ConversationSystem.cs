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

        UpdateOptions();

        gameObject.SetActive(true);
    }

    // overloaded method to start the conversation at a specific index
    public void Show(NPC npc, string name, ConversationNode[] nodes, Sprite image, Action<bool> onEndCallback = null, int startIndex = 0)
    {
        currentNPC = npc;

        npcName = name;
        nameText.text = name;
        characterImage.sprite = image;

        conversationNodes = new List<ConversationNode>(nodes);
        conversationIndex = startIndex;
        currentNode = conversationNodes[conversationIndex];
        conversationText.text = currentNode.dialogueText;

        onConversationEnd = onEndCallback;

        UpdateOptions();

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
        //show the training ground UI after the option when talking to the NPCs
        if (npcName == "Ms. Guide")
        {
            if (conversationIndex == 3)
            {
                TownFacilityUpgradeUI.Instance.Show();
                Hide();
                return;
            }
            else if (conversationIndex == 4)
            {
                ReinforcementUI.instance.Show();
                Hide();
                return;
            }
        }

        if (npcName == "Training Ground")
        {
            if (conversationIndex == 2)
            {
                TrainingGroundUI.Instance.Show();
                Hide();
                return;
            }
            else if (conversationIndex == 3)
            {
                var PlayerChar = GameObject.FindWithTag("Player").GetComponent<PlayerChar>();
                if (!PlayerChar) { print("Can't find PlayerChar script on the player!"); return; }

                int classIndex = PlayerChar.GetCurrentClassIndex();
                ClassSelectionUI.Instance.Show(classIndex);

                Hide();
                return;
            }
        }

        if (npcName == "RandomRoom NPC")
        {
            if (conversationIndex == 3)
            {
                GameManager.Instance.GainGold(100);

                Hide();
                return;
            }
            else if (conversationIndex == 4)
            {
                GameManager.Instance.GainSoul(100);

                Hide();
                return;
            }
        }

        currentNPC?.OnConversationAdvance(conversationIndex);

        conversationIndex++;

        if (conversationIndex < conversationNodes.Count)
        {
            currentNode = conversationNodes[conversationIndex];
            conversationText.text = currentNode.dialogueText;
            UpdateOptions();
        }
        else
        {
            Hide();
        }
    }

    private void UpdateOptions()
    {
        if (currentNode.HasOptions())
        {
            Multi_ChoiceDialogue.gameObject.SetActive(true);

            foreach (Transform child in optionsContainer)
            {
                Button btn = child.GetComponent<Button>();
            }

            for (int i = 0; i < currentNode.options.Count && i < optionButtons.Count; i++)
            {
                var option = currentNode.options[i];
                var btn = optionButtons[i];
                btn.gameObject.SetActive(true);
                btn.GetComponentInChildren<TextMeshProUGUI>().text = option.optionText;

                int capturedIndex = option.nextNodeIndex;
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    OnOptionSelected(capturedIndex);
                });
            }

            nextButton.gameObject.SetActive(false);
        }
        else
        {
            Multi_ChoiceDialogue.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(true);
        }
    }

    private void OnOptionSelected(int nextNodeIndex)
    {
        //Debug.Log("Option has been pressed, the next node will be: " + nextNodeIndex);

        if (nextNodeIndex >= 0 && nextNodeIndex < conversationNodes.Count)
        {
            conversationIndex = nextNodeIndex;
            currentNode = conversationNodes[conversationIndex];
            conversationText.text = currentNode.dialogueText;
            UpdateOptions();
        }
        else
        {
            Hide();
        }
    }

    public int GetCurrentConversationIndex()
    {
        return conversationIndex;
    }
}
