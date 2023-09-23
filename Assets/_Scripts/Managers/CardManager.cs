using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager cm;
    public int numPlayers;
    public int cardsPerPlayer;
    public List<Card> cards;
    [SerializeField]
    private bool startGame;
    private int ID = 0;
    private Coroutine coroutine = null;
    private GameplayUI Gui => GameplayUI.gUI;
    private GameController Gc => GameController.gc;

    private void Awake()
    {
        cm = this;
        for (int i = 0; i < 52; i++)
        {
            cards[i].cardID = i;
        }
    }

    private void Start()
    {
        SetGame();
    }

    private void SetGame()
    {
        numPlayers = Gui.playerCount;
        cardsPerPlayer = cards.Count / numPlayers;
        Helper.Shuffle(cards);
        Helper.Shuffle(cards);
        Helper.Shuffle(cards);
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
                Gc.players[currentPlayer].handIntList.Add(cards[ID].cardID);
                currentCard++;
                ID++;
                yield return Helper.GetWait(0.1f);
            }
            currentPlayer++;
        }
        DealRemaningCards();
        Gui.CardManagerPanel();
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
            CardController cc = Gc.players[i];
            cc.ActivatePlayerCards();
            if (cc.HaveAceOfSpade)
            {
                _ = StartCoroutine(cc.TurnRoutine(20f));
            }
        }
    }
}
