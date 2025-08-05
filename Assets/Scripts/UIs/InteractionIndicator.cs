using UnityEngine;
using System.Collections;


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

    private void Update()
    {
        if (isInteracting && (interactionItemName == "Woods") && Input.GetKeyDown(KeyCode.E))
        {
            CardGameUI.Instance.Show();
        }
        else if (isInteracting && (interactionItemName == "MotorBikeToForrest") && Input.GetKeyDown(KeyCode.E))
        {
            // player move to forest
            StartCoroutine(FadeAndTeleport(new Vector3(-56.02f, -0.92f, 0)));
        }
        else if (isInteracting && (interactionItemName == "MotorBikeToTown") && Input.GetKeyDown(KeyCode.E))
        {
            // player move to town
            ScreenFadeAnime.Instance.Play();
            StartCoroutine(FadeAndTeleport(new Vector3(-4.94f, -0.92f, 0)));
        }
    }

    private void LateUpdate()
    {
        if (interactionButton.activeSelf)
        {
            interactionButton.transform.position = transform.position + buttonPosition;
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
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            interactionButton.gameObject.SetActive(false);

            isInteracting = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            interactionButton.SetActive(true);
            interactionButton.transform.position = transform.position + buttonPosition;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            interactionButton.SetActive(false);
        }
    }

    private IEnumerator FadeAndTeleport(Vector3 target)
    {
        ScreenFadeAnime.Instance.Play();
        yield return new WaitForSeconds(1f);    //delay for 1 second
        PlayerTransmission.Instance.TeleportTo(target);
    }
}
