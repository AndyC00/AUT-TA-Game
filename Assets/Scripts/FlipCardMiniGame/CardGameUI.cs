using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardGameUI : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winningPanel;

    [SerializeField] private TextMeshProUGUI resourceText;
    [SerializeField] private int resourcePoints = 0;

    [SerializeField] private Button winningPageButton;
    [SerializeField] private Button losePageButton;

    [SerializeField] private TextMeshProUGUI timerText;

    // singleton pattern
    public static CardGameUI Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        startPanel ??= transform.Find("StartPanel").gameObject;

        losePanel ??= transform.Find("LosePage").gameObject;
        losePageButton ??= losePanel.transform.Find("ConfirmButton").GetComponent<Button>();
        losePageButton.onClick.AddListener(Hide);

        winningPanel ??= transform.Find("WinningPage").gameObject;
        resourceText ??= winningPanel.transform.Find("number").GetComponent<TextMeshProUGUI>();
        winningPageButton ??= winningPanel.transform.Find("ConfirmButton").GetComponent<Button>();
        winningPageButton.onClick.AddListener(Hide);

        timerText ??= transform.Find("TimerText").GetComponent<TextMeshProUGUI>();

        Hide();
    }

    void Update()
    {
        if (startPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                startPanel.SetActive(false);

                CardGameManager.Instance.StartGame();
            }
        }
    }

    public void Show()
    { 
        gameObject.SetActive(true);

        CardGameManager.Instance.ResetForReplay();
        ResetResourcePoints();

        startPanel.SetActive(true);
    }

    public void Hide()
    {
        HideWinningPanel();
        HideLosePanel();
        gameObject.SetActive(false);
    }

    // winning panel
    public void ShowWinningPanel()
    {
        resourceText.text = resourcePoints.ToString();
        GameManager.instance.ResourceCount += resourcePoints;
        winningPanel.SetActive(true);
    }
    private void HideWinningPanel()
    {
        winningPanel.SetActive(false);
    }

    // lose panel
    public void ShowLosePanel()
    {
        losePanel.SetActive(true);
    }
    private void HideLosePanel()
    {
        losePanel.SetActive(false);
    }

    // resource points
    public void GainResourcePoints(int num)
    {
        resourcePoints += num;
    }
    public void ResetResourcePoints()
    {
        resourcePoints = 0;
    }

    // Timer display
    public void UpdateTimer(float timeLeft)
    { 
        timeLeft = Mathf.Max(0, timeLeft);

        int seconds = Mathf.CeilToInt(timeLeft);
        int minutes = seconds / 60;
        int sec = seconds % 60;

        timerText.text = $"{minutes:00}:{sec:00}";
    }
}
