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
            interactionButton.transform.position = transform.position + new Vector3(-1, 1, 0);

            isInteracting = true;
        }
        else if (collision.gameObject.CompareTag("NPC"))
        {
            interactionButton.gameObject.SetActive(true);
            interactionButton.transform.position = transform.position + new Vector3(-1, 1, 0);
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
