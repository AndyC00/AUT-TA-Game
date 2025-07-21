using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using CardData;

public class CardUI : MonoBehaviour, IPointerClickHandler
{
    private Image background;
    [SerializeField] private Image iconImage;

    public Card data { get; private set; }
    private bool isRevealed;
    private bool isLocked;

    private void Awake()
    {
        background = GetComponent<Image>();
        iconImage ??= transform.Find("Image")?.GetComponent<Image>();
    }

    public void Setup(Card d, Sprite icon, Color bgColor)
    {
        background ??= GetComponent<Image>();
        iconImage ??= transform.Find("Image")?.GetComponent<Image>();

        data = d;
        iconImage.sprite = icon;
        background.color = bgColor;

        // card starts concealed
        iconImage.enabled = false;
        isRevealed = false;
        isLocked = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isLocked) return;
        CardGameManager.Instance.OnCardClicked(this);
    }

    public void Reveal(bool immediate = false)
    {
        if (isRevealed) return;
        isRevealed = true;

        iconImage.enabled = true;
    }

    public void Conceal()
    {
        if (!isRevealed || isLocked) return;
        isRevealed = false;
        iconImage.enabled = false;
    }

    public void Lock()
    {
        isLocked = true;
        iconImage.sprite = null;
        background.color = Color.gray;
    }
}
