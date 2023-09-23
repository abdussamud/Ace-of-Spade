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
    public Sprite[] cardSprites;
    //public Image[] playerCards;
    public PlayerCard[] playerCards;
    public DiscardCard[] discardCards;

    private void Awake()
    {
        gUI = this;
    }

    public void SetPlayerCard(int cardNumber, int index)
    {
        playerCards[index].gameObject.SetActive(true);
        playerCards[index].SetSprite(cardSprites[cardNumber]);
    }

    public void SetPlayerProfile(int playerNumber = 0, string playerName = "")
    {
        discardCards[playerNumber].gameObject.SetActive(true);
        discardCards[playerNumber].numberText.text = (playerNumber + 1).ToString();
        discardCards[playerNumber].profileBG.color = Color.white;
        discardCards[playerNumber].turnText.text = "";
        discardCards[playerNumber].nameText.text = playerName;
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
