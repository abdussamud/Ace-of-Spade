using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AlphaKnight;
// shuffling, dealing, placing, collecting
public class CardManager : MonoBehaviour
{
    public static CardManager cm;
    public int numPlayers;
    public int cardsPerPlayer;
    public List<Card> cards;
    public List<Cell> cells;
    public Transform[] cardPositions;
    public CellsParent[] cellParents;
    [SerializeField]
    private bool startGame;
    private int ID = 0;
    private Coroutine coroutine = null;

    private void Awake()
    {
        cm = this;
        for (int i = 0; i < 52; i++)
        {
            cards[i].cardID = i;
        }
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
                yield return new WaitForSeconds(0.1f);
            }
            currentPlayer++;
        }
        DealRemaningCards();
        GameplayUI.gUI.CardManagerPanel();
        ActivateAllPlayersCards();
    }

    private void DealRemaningCards()
    {
        int currentPlayerIndex = 0;
        while (ID < cards.Count)
        {
            GameController.gc.players[currentPlayerIndex].handIntList.Add(cards[ID].cardID);
            ID++;
            currentPlayerIndex++;
        }
    }

    private void ActivateAllPlayersCards()
    {
        for (int i = 0; i < numPlayers; i++)
        {
            GameController.gc.players[i].ActivatePlayerCards();
        }
    }

    public void StartGame()
    {
        coroutine ??= StartCoroutine(StartGameOnPlayerComplete());
    }

    public void ShuffleCards()
    {
        int place = numPlayers - 3;
        int count = cells.Count;
        cellParents[place].gameObject.SetActive(true);
        cells = cellParents[place].cells.ToList();
        for (int i = 0; i < count; i++) { cells[i].number = i; }
        if (startGame)
        {
            cardsPerPlayer = cards.Count / numPlayers;
            Helper helper = new();
            helper.Shuffle(cards);
            GameplayUI.gUI.DealCardButton(interactable: true);
        }
        else { Debug.Log("Players are Less then 3"); }
    }

    public void DealCard()
    {
        GameplayUI.gUI.DealCardButton();
        GameplayUI.gUI.ShuffleCardButton();
        _ = StartCoroutine(DealCardRoutine());
    }
}
