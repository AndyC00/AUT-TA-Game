using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private GameObject interactionButton;

    void Start()
    {
        interactionButton ??= GameObject.Find("InteractionButton");
        interactionButton.gameObject.SetActive(false);


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactionButton.gameObject.SetActive(true);
            interactionButton.transform.position = transform.position + new Vector3(-1, 3, 0);

            if (Input.GetKeyDown(KeyCode.E))
            {
                // Trigger interaction here
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactionButton.gameObject.SetActive(false);
        }
    }
}
