using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using CardData;

public class CardUI : MonoBehaviour, IPointerClickHandler
{
    private Image background;
    [SerializeField] private Image iconImage;

    public Card data { get; private set; }
    private bool isRevealed = false;
    private bool isLocked = false;

    private void Start()
    {
        background = GetComponent<Image>();
        iconImage ??= transform.Find("Image").GetComponent<Image>();
    }

    public void Setup(Card d, Sprite icon, Color bgColor)
    {
        data = d;
        iconImage.sprite = icon;
        background.color = bgColor;
        Conceal(immediate: true);
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

    public void Conceal(bool immediate = false)
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
