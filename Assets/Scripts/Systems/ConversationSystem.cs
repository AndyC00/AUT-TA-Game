using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class ConversationSystem : MonoBehaviour
{
    //UI elements
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI conversationText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Sprite characterSprite;
    private Image characterImage;

    private List<string> contentList;
    private int contentIndex = 0;


    //------------- singleton pattern -------------
    public static ConversationSystem Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    //------------- Start -------------
    private void Start()
    {
        //get all the conversation UI components
        nameText ??= transform.Find("NPCname").GetComponent<TextMeshProUGUI>();
        conversationText ??= transform.Find("ConversationText").GetComponent<TextMeshProUGUI>();
        characterImage ??= transform.Find("CharacterImage").GetComponent<Image>();
        characterSprite = characterImage.sprite;

        nextButton ??= transform.Find("NextButton").GetComponent<Button>();
        nextButton.onClick.AddListener(this.OnNextButtonClick);

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Show(string name, Sprite image, string[] content)
    {
        nameText.text = name;
        characterSprite = image;

        contentList = new List<string>();
        contentList.AddRange(content);
        contentIndex = 0;
        conversationText.text = contentList[0];

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnNextButtonClick()
    {
        contentIndex++;

        if (contentIndex >= contentList.Count)
        {
            Hide();
            return;
        }

        conversationText.text = contentList[contentIndex];
    }
}
