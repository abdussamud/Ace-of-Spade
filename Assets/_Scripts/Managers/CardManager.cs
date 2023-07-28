using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public int numPlayers;
    public int cardsPerPlayer;
    public List<Card> cards;
    [SerializeField]
    private bool startGame;
    private int ID = 0;

    private void Awake()
    {
        for (int i = 0; i < 52; i++)
        {
            cards[i].cardID = i;
        }
    }

    private Coroutine coroutine = null;
    public void StartGame()
    {
        coroutine ??= StartCoroutine(StartGameOnPlayerComplete());
    }

    public void ShuffleCards()
    {
        if (startGame)
        {
            cardsPerPlayer = cards.Count / numPlayers;
            Shuffle(cards);
            GameplayUI.gUI.DealCardButton(interactable: true);
        }
        else
        {
            Debug.Log("Players are Less then 3");
        }
    }

    public void DealCard()
    {
        GameplayUI.gUI.DealCardButton();
        GameplayUI.gUI.ShuffleCardButton();
        _ = StartCoroutine(DealCardRoutine());
    }

    private IEnumerator DealCardRoutine()
    {
        int currentPlayer = 0;
        while (currentPlayer < numPlayers)
        {
            int currentCard = 0;
            while (currentCard < cardsPerPlayer)
            {
                GameController.gc.players[currentPlayer].handIntList.Add(cards[ID].cardID);
                currentCard++;
                ID++;
                yield return new WaitForSeconds(0.2f);
            }
            currentPlayer++;
            yield return new WaitForSeconds(0.7f);
        }
        GameplayUI.gUI.CardManagerPanel();
    }

    private IEnumerator StartGameOnPlayerComplete()
    {
        numPlayers = GameController.gc.players.Count;
        while (GameController.gc.players.Count < 3)
        {
            Debug.Log("Players are less then 3 wait for 1 second!");
            yield return new WaitForSeconds(1);
            numPlayers = GameController.gc.players.Count;
        }
        startGame = true;
        GameplayUI.gUI.StartGame();
    }


    private void Shuffle<T>(List<T> list)
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
}
