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
    [SerializeField] private float firstFadePeriod = 1.0f;
    [SerializeField] private float secondFadePeriod = 0.5f;
    private Color timerTextBaseColor;

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
        timerTextBaseColor = timerText.color;

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

        timerText.text = "01:30";

        startPanel.SetActive(true);
    }

    public void Hide()
    {
        timerText.text = string.Empty;
        timerText.color = timerTextBaseColor;

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

        FMODUnity.RuntimeManager.PlayOneShot("event:/cardGameComplete");
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
        // update text
        timeLeft = Mathf.Max(0, timeLeft);

        int seconds = Mathf.CeilToInt(timeLeft);
        int minutes = seconds / 60;
        int sec = seconds % 60;

        timerText.text = $"{minutes:00}:{sec:00}";

        // update text color
        if (timeLeft <= 20f)
            ApplyBlink(Color.red, secondFadePeriod);
        else if (timeLeft <= 45f)
            ApplyBlink(Color.yellow, firstFadePeriod);
        else
            timerText.color = timerTextBaseColor;
    }

    public void HideTimer() => timerText.text = string.Empty;

    private void ApplyBlink(Color target, float period)
    {
        float alpha = (Mathf.Sin(Time.time * (Mathf.PI * 2f) / period) + 1f) * 0.5f;
        target.a = alpha;
        timerText.color = target;
    }
}
