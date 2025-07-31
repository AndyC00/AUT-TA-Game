using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using CardData;

public class CardUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image background;
    [SerializeField] private Image iconImage;
    [SerializeField] private Sprite matchedIcon;

    [SerializeField] private Color backColor = Color.white;
    private Color faceColor;

    public Card data { get; private set; }
    private bool isRevealed;
    private bool isLocked;

    public bool IsMatched => isLocked;

    private void Awake()
    {
        background = transform.Find("BG")?.GetComponent<Image>();
        iconImage ??= transform.Find("Image")?.GetComponent<Image>();
    }

    public void Setup(Card d, Sprite icon, Color color)
    {
        background ??= transform.Find("BG")?.GetComponent<Image>();
        iconImage ??= transform.Find("Image")?.GetComponent<Image>();

        data = d;
        iconImage.sprite = icon;
        faceColor = color;

        // card starts concealed
        background.color = backColor;
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

        background.color = faceColor;
        iconImage.enabled = true;
    }

    public void Conceal()
    {
        if (!isRevealed || isLocked) return;
        isRevealed = false;
        background.color = backColor;
        iconImage.enabled = false;
    }

    public void Lock()
    {
        isLocked = true;
        iconImage.sprite = matchedIcon;
        background.color = backColor;
    }
}
