using UnityEngine;
using UnityEngine.InputSystem;

public class StartPanel : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()   // press Enter to hide this panel (new Unity input system)
    {
        if (Keyboard.current == null) { Debug.Log("keyboard doesn't applied!"); return; }

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.numpadEnterKey.wasPressedThisFrame)
        {
            Hide();
        }
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
