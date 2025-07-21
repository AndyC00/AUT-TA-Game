using UnityEngine;

public class CardGameUI : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;

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
        Hide();
    }

    void Update()
    {
        if (startPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                startPanel.SetActive(false);
            }
        }
    }

    public void Show()
    { 
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
