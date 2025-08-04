using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionIndicator : MonoBehaviour
{
    [SerializeField] private GameObject interactionButton;
    [SerializeField] private Vector3 buttonPosition = new Vector3(-0.5f, 1, 0);
    private bool isInteracting = false;
    private string interactionItemName = string.Empty;

    // singleton pattern
    public static InteractionIndicator Instance { get; private set; }
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
        interactionButton ??= GameObject.Find("InteractionButton");
        interactionButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isInteracting && (interactionItemName == "Woods") && Input.GetKeyDown(KeyCode.E))
        {
            CardGameUI.Instance.Show();
        }
        else if (isInteracting && (interactionItemName == "MotorBikeToForrest") && Input.GetKeyDown(KeyCode.E))
        {
            // player move to forest
            PlayerTransmission.Instance.TeleportTo(new Vector3(-61.36f, -0.92f, 0));
        }
        else if (isInteracting && (interactionItemName == "MotorBikeToTown") && Input.GetKeyDown(KeyCode.E))
        {
            // player move to town
            PlayerTransmission.Instance.TeleportTo(new Vector3(-8.27f, -0.92f, 0));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            interactionButton.gameObject.SetActive(true);
            interactionButton.transform.position = transform.position + buttonPosition;

            isInteracting = true;
            interactionItemName = collision.gameObject.name;
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
