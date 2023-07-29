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
        Invoke(nameof(SortCardOnTable), 1f);
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
