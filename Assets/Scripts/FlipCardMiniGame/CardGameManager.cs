using CardData;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Net.Sockets;

public class CardGameManager : MonoBehaviour
{
    [SerializeField] private CardUI cardPrefab;
    [SerializeField] private GameObject cardPanel;
    [SerializeField] private CardFaceLibrary cardFaceLibrary;

    [SerializeField] private int pairCount = 8;

    List<CardUI> spawnedCards = new List<CardUI>();

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

        var deck = new List<CardData.Card>(pairCount * 2);
        foreach (var card in uniquePairs)
        {
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
            int j = randomNum.Next(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
