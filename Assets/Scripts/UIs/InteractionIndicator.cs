using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionIndicator : MonoBehaviour
{
    [SerializeField] private GameObject interactionButton;
    [SerializeField] private Vector3 buttonPosition = new Vector3(-0.5f, 1, 0);
    private bool isInteracting = false;

    void Start()
    {
        interactionButton ??= GameObject.Find("InteractionButton");
        interactionButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isInteracting && Input.GetKeyDown(KeyCode.E))
        {
            CardGameUI.Instance.Show();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            interactionButton.gameObject.SetActive(true);
            interactionButton.transform.position = transform.position + buttonPosition;

            isInteracting = true;
        }
        else if (collision.gameObject.CompareTag("NPC"))
        {
            interactionButton.gameObject.SetActive(true);
            interactionButton.transform.position = transform.position + buttonPosition;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            interactionButton.gameObject.SetActive(false);

            isInteracting = false;
        }
        else if (collision.gameObject.CompareTag("NPC"))
        {
            interactionButton.gameObject.SetActive(false);
        }
    }
}
