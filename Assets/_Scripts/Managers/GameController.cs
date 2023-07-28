using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController gc;

    public List<PlayerController> players;
    public List<Card> deck;
    public int numPlayers;
    public int cardsPerPlayer;
    public List<List<Card>> playerHands = new();
    public Transform[] cardPositions;
    public CardsPlace[] cardPlaces;
    public List<Cell> cellPositions;

    private void Awake()
    {
        gc = this;
    }

    private void MyStart()
    {
        cardsPerPlayer = deck.Count / numPlayers;
        int place = numPlayers == 3 ? 0 : numPlayers == 4 ? 1 : numPlayers == 5 ? 2 : numPlayers == 6 ? 3 : 4;
        cardPlaces[place].ActivatePlace();
        cellPositions = cardPlaces[place].cells;
        Shuffle(deck);
        Invoke(nameof(DealCards), 0.5f);
        Invoke(nameof(SortCardOnTable), 1f);
    }

    public void Shuffle<T>(List<T> list)
    {
        System.Random rand = new();
        int n = list.Count;
        while (n > 1)
        {
            int k = rand.Next(n);
            n--;
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    public void DealCards()
    {
        List<GameObject> newGameObject = new();
        for (int i = 0; i < numPlayers; i++)
        {
            newGameObject.Add(Instantiate(new GameObject("Player" + i), gameObject.transform));
            newGameObject[i].AddComponent<PlayerController>();
            newGameObject[i].GetComponent<PlayerController>().playerNumber = i;
            List<Card> hand = new();
            for (int j = 0; j < cardsPerPlayer; j++)
            {
                Card card = deck[0];
                deck.RemoveAt(0);
                hand.Add(card);
            }
            playerHands.Add(hand);
        }
        int currentPlayerIndex = 0;
        while (deck.Count > 0)
        {
            Card card = deck[0];
            deck.RemoveAt(0);
            playerHands[currentPlayerIndex].Add(card);
            currentPlayerIndex++;
        }
        for (int i = 0; i < newGameObject.Count; i++)
        {
            newGameObject[i].GetComponent<PlayerController>().hand = playerHands[i];
        }
    }

    public void SortCardOnTable()
    {
        float yOffset = 0;
        float zOffset = 0;
        int j = 0;
        for (int i = 0; i < playerHands[0].Count; i++)
        {
            playerHands[0][i].gameObject.transform.position = new(cardPositions[j].position.x,
                cardPositions[j].position.y + yOffset, cardPositions[j].position.z + zOffset);
            playerHands[0][i].gameObject.SetActive(true);
            if (j < 4)
            {
                j++;
            }
            else
            {
                j = 0;
                yOffset -= 0.7f;
                zOffset -= 0.1f;
            }
        }
    }
}

[Serializable]
public class CardsPlace
{
    public string name;
    public int numberOfPlayers;
    public GameObject cardsPlaceParent;
    public Transform[] cardsPositions;
    public List<Cell> cells;


    public void ActivatePlace()
    {
        cardsPlaceParent.SetActive(true);
    }

    public void DeactivatePlace()
    {
        cardsPlaceParent.SetActive(false);
    }
}

[Serializable]
public class Cell
{
    public string name;
    public bool isOccupide;
    public int number;
    public Card currentCard;
    public Transform cellTransform;
}
