using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using CardData;
using System.Collections;

public class CardGameManager : MonoBehaviour
{
    [SerializeField] private CardUI cardPrefab;
    [SerializeField] private GameObject cardPanel;
    [SerializeField] private CardFaceLibrary cardFaceLibrary;

    [SerializeField] private int pairCount = 8;
    [HideInInspector] public bool inputLocked;

    List<CardUI> spawnedCards = new List<CardUI>();
    private CardUI firstCard = null;

    private int matchedPairs = 0;

    // singleton pattern
    public static CardGameManager Instance { get; private set; }
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
        if (!cardPrefab) Debug.LogError("CardPrefab is not assigned in the inspector.");
        cardPanel ??= transform.Find("CardPanel").gameObject;

        GameInit();
    }

    private void GameInit()
    {
        // reset the card panel
        Transform parent = cardPanel.transform;
        foreach (Transform card in parent)
        {
            Destroy(card.gameObject);
        }
        spawnedCards.Clear();

        // generate unique pairs of cards
        var uniquePairs = GenerateUniquePairs(pairCount);
        //Debug.Log($"Unique pairs generated: {uniquePairs.Count}");

        var deck = new List<CardData.Card>(pairCount * 2);
        foreach (var card in uniquePairs)
        {
            //Debug.Log($"Unique pairs generated: {uniquePairs.Count}");

            deck.Add(card);
            deck.Add(card);
        }
        Shuffle(deck);

        // instantiate card UI elements
        foreach (var data in deck)
        {
            var card = Instantiate(cardPrefab, parent);
            var sprite = cardFaceLibrary.cardSprites[(int)data.icon];
            var color = cardFaceLibrary.cardColors[(int)data.color];
            card.Setup(data, sprite, color);
            spawnedCards.Add(card);
        }
    }

    public List<Card> GenerateUniquePairs(int pairCount)
    {
        var pool = new List<Card>();
        var usedCards = new HashSet<int>();

        System.Random randomNum = new System.Random();
        while (pool.Count < pairCount)
        {
            var color = (CardColor)randomNum.Next(0, 4);
            var icon = (CardIcon)randomNum.Next(0, 8);
            var data = new Card { color = color, icon = icon };

            if (usedCards.Add(data.Id))      // true = first time generating this card
                pool.Add(data);
        }

        return pool;
    }

    private void Shuffle<T>(IList<T> list)
    {
        var randomNum = new System.Random();
        for (int i = list.Count - 1; i > 0; i--)
        {
            int k = randomNum.Next(i + 1);
            (list[i], list[k]) = (list[k], list[i]);
        }
    }

    public void OnCardClicked(CardUI clickedCard)
    {
        if (inputLocked || clickedCard == firstCard) return;

        if (firstCard == null)
        {
            firstCard = clickedCard;
            clickedCard.Reveal();
        }
        else
        {
            clickedCard.Reveal();

            // if the clicked card matches the first card
            if (clickedCard.data.Id == firstCard.data.Id)
            {
                clickedCard.Lock();
                firstCard.Lock();

                firstCard = null;

                // reward count
                matchedPairs++;
                CardGameUI.Instance.GainResourcePoints(10);

                // check if the game is won and complete
                if (matchedPairs >= pairCount)
                {
                    StartCoroutine(ShowWinningPage());
                }
            }
            else
            {
                StartCoroutine(FlipBackRoutine(clickedCard, firstCard));
                firstCard = null;
            }
        }
    }

    IEnumerator ShowWinningPage()
    {
        inputLocked = true;
        // play winning sound effect

        yield return new WaitForSeconds(0.4f);

        CardGameUI.Instance.ShowWinningPanel();
    }

    private System.Collections.IEnumerator FlipBackRoutine(CardUI card1, CardUI card2)
    {
        inputLocked = true;
        yield return new WaitForSeconds(0.5f); // wait for 0.5 seconds before flipping back

        card1.Conceal();
        card2.Conceal();

        inputLocked = false;
    }
}
