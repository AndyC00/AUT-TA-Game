using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private GameObject resourceNumber;
    private TextMeshProUGUI resourceText;

    void Start()
    {
        resourceNumber ??= transform.Find("number").gameObject;
        resourceText = resourceNumber.GetComponent<TextMeshProUGUI>();

        GameManager.instance.OnResourceCountChanged += UpdateUI;
    }

    private void UpdateUI(int newCount)
    {
        resourceText.text = newCount.ToString();
    }
}
