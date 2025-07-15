using UnityEngine;

public class CardGameUI : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;

    void Start()
    {
        startPanel ??= transform.Find("StartPanel").gameObject;
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
}
