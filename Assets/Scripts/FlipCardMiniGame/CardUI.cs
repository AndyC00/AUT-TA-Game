using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image iconImage;

    public Card data;
    bool isRevealed, isLocked;

    public void Setup(Card d, Sprite icon, Color bgColor)
    {
        data = d;
        iconImage.sprite = icon;
        background.color = bgColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isLocked) return;
        CardGameManager.Instance.OnCardClicked(this);
    }
}
