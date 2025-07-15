using UnityEngine;
using CardData;
using NUnit.Framework;
using System.Collections.Generic;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class CardGameManager : MonoBehaviour
{


    void Start()
    {
        
    }

    List<Card> GenerateUniquePairs(int pairCount)
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
}
