using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Card> deck;
    public int numPlayers;
    public int cardsPerPlayer;
    public List<List<Card>> playerHands = new();

    private void Start()
    {
        AceOfSpade.Shuffle(deck);
        cardsPerPlayer = deck.Count / numPlayers;
        Invoke(nameof(DealCards), 0.5f);
    }

    public void DealCards()
    {
        List<GameObject> newGameObject = new();
        for (int i = 0; i < numPlayers; i++)
        {
            newGameObject.Add(Instantiate(new GameObject("Player" + i), gameObject.transform));
            newGameObject[i].AddComponent<Player>();
            newGameObject[i].GetComponent<Player>().playerNumber = i;
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
            newGameObject[i].GetComponent<Player>().hand = playerHands[i];
        }
    }
}
