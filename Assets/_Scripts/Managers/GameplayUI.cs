using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public static GameplayUI gUI;

    public GameObject gameplayPanelGO;
    public GameObject cardManagerPanelGO;
    public GameObject startGamePanelGO;
    public Button dealCardButton;
    public Button shuffleCardButton;

    private void Awake()
    {
        gUI = this;
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
