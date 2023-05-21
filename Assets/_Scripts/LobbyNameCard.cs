using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyNameCard : MonoBehaviour
{
    public string lobbyName;
    public int playerCount;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playerCountText;


    public void SetLobbyName_and_PlayerCount()
    {
        lobbyNameText.text = lobbyName;
        playerCountText.text = playerCount.ToString();
    }

    public void OnJoinLobbyButtonClicked()
    {

    }
}
