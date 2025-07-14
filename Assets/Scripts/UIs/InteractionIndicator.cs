using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionIndicator : MonoBehaviour
{
    [SerializeField] private GameObject interactionButton;
    private bool isInteracting = false;

    void Start()
    {
        interactionButton ??= GameObject.Find("InteractionButton");
        interactionButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isInteracting && Keyboard.current.eKey.wasPressedThisFrame)
        {
            //GameManager.instance.ResourceCount += 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactionButton.gameObject.SetActive(true);
            interactionButton.transform.position = transform.position + new Vector3(-1, 1, 0);

            isInteracting = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactionButton.gameObject.SetActive(false);
        }

        isInteracting = false;
    }
}
