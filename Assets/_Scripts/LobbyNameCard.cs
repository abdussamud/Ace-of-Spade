using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyNameCard : MonoBehaviour
{
    public string lobbyName;
    public int playerCount;
    public int totalPlayerCount;
    public Button lobbyCardJoinButton;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playerCountText;


    public void SetLobbyName_and_PlayerCount()
    {
        lobbyNameText.text = lobbyName;
        playerCountText.text = (playerCount + " / " + totalPlayerCount).ToString();
    }

    public void OnJoinLobbyButtonClicked()
    {

    }
}
