using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartPanel : MonoBehaviour
{
    [SerializeField] private GameObject startText;
    [SerializeField] private float period = 3.5f;
    private TextMeshProUGUI text;
    private Color baseColor;

    void Start()
    {
        startText ??= transform.Find("Title").gameObject;
        text = startText.GetComponent<TextMeshProUGUI>();
        baseColor = text.color;
    }

    void Update()   // press Enter to hide this panel (new Unity input system)
    {
        if (Keyboard.current == null) { Debug.Log("keyboard doesn't applied!"); return; }

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.numpadEnterKey.wasPressedThisFrame || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Hide();
        }

        // color change of the "Start"
        float alpha = (Mathf.Sin(Time.time * (Mathf.PI * 2f) / period) + 1f) * 0.5f;
        text.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
