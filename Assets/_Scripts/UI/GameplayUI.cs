using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public static GameplayUI gUI;

    public int playerCount;
    public GameObject gameplayPanelGO;
    public GameObject cardManagerPanelGO;
    public GameObject startGamePanelGO;
    public Button dealCardButton;
    public Button shuffleCardButton;
    public GameObject[] playerCardsGO;
    public Sprite[] allCards;
    public Image[] playerCards;
    public DiscardCard[] opponentCards;

    private void Awake()
    {
        gUI = this;
    }

    private void Start()
    {
        SetPlayers();
    }

    public void SetPlayerCard(int cardNumber, int index)
    {
        playerCardsGO[index].SetActive(true);
        playerCards[index].sprite = allCards[cardNumber];
    }

    private void SetPlayers()
    {
        for (int i = 0; i < playerCount; i++)
        {
            opponentCards[i].gameObject.SetActive(true);
            opponentCards[i].numberText.text = (i + 1).ToString();
            opponentCards[i].profileBG.color = Color.white;
            opponentCards[i].turnText.text = "";
            opponentCards[i].nameText.text = i == 0 ? "YOU" : "IC";
        }
    }

    public void DealCardButton(bool interactable = false)
    {
        dealCardButton.interactable = interactable;
    }

    public void ShuffleCardButton(bool interactable = false)
    {
        shuffleCardButton.interactable = interactable;
    }

    public void StartGame()
    {
        startGamePanelGO.SetActive(false);
        cardManagerPanelGO.SetActive(true);
    }

    public void CardManagerPanel(bool activate = false)
    {
        cardManagerPanelGO.SetActive(activate);
    }
}
