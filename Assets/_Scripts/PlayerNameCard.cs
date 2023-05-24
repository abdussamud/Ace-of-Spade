using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameCard : MonoBehaviour
{
    public string playerName;
    public Button kickPlayerButton;
    [SerializeField] private TextMeshProUGUI playerNameText;


    public void SetPlayerName()
    {
        playerNameText.text = playerName;
    }

    public void OnJoinLobbyButtonClicked()
    {

    }
}
