using UnityEngine;

public class InteractionNPC : MonoBehaviour
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
