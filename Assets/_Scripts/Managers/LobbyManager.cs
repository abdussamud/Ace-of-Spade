using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;


public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance { get; private set; }

    public GameData gameData;

    [Header("UI Panel")]
    public GameObject CreateAccountPanel;
    public GameObject CreateLobbyPanel;
    public GameObject LobbiesListPanel;
    public GameObject InLobbyPanel;

    [Header("Account")]
    private string playerName;
    [SerializeField]
    private TextMeshProUGUI invalidNamePrompter;

    [Header("Player")]
    [SerializeField]
    private List<PlayerNameCard> playerNameCard;
    [SerializeField]
    private List<string> playerIdList;

    [Header("Lobby")]
    [SerializeField]
    private Slider lobbyMaxPlayerCount;
    private string lobbyName;
    [SerializeField]
    private TextMeshProUGUI invalidLobbyNamePrompter;
    [SerializeField]
    private TextMeshProUGUI lobyyNameForInLobby;
    [SerializeField]
    private TextMeshProUGUI lobyyPlayerCountText;
    private Lobby joinedLobby;
    private float heartbeatTimer;
    private float lobbyPollTimer;
    [SerializeField]
    private GameObject lobiesListParentGO;
    [SerializeField]
    private GameObject lobbyNameCard;
    [SerializeField]
    private bool isLobbyPrivate;
    [SerializeField]
    private List<GameObject> lobbyListGO = new();


    private void Awake() { Instance = this; }

    private void Start()
    {
        playerNameCard.ForEach(pNC => pNC.gameObject.SetActive(false));
        if (!string.IsNullOrEmpty(gameData.userName))
        {
            playerName = gameData.userName;
            AccountLogin();
        }
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
    }

    private void OnApplicationQuit()
    {
        LeaveLobby();
    }

    public void OnCreateAccountButtonClicked()
    {
        if (!string.IsNullOrEmpty(playerName) && playerName.All(char.IsLetter) && playerName.Length >= 5)
        {
            AccountLogin();
        }
        else
        {
            if (string.IsNullOrEmpty(playerName))
            {
                invalidNamePrompter.text = "Player Name shoulden't be empty!";
                Invoke(nameof(PrompterHider), 4f);
            }
            else if (playerName.Length is > 0 and < 5)
            {
                invalidNamePrompter.text = "Player Name Length Should be Less then 5!";
                Invoke(nameof(PrompterHider), 4f);
            }
            else
            {
                invalidNamePrompter.text = "Spaces aren't allowed!";
                Invoke(nameof(PrompterHider), 4f);
            }
            Debug.Log("Invalid Player Name!");
        }
    }

    private async void AccountLogin()
    {
        try
        {
            await UnityServices.InitializeAsync();

            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("Signed in: " + AuthenticationService.Instance.PlayerId);
            };
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            gameData.userName = playerName;
            DataManager.Instance.SaveData();
            Debug.Log(playerName);
            CreateLobbyPanel.SetActive(true);
            CreateAccountPanel.SetActive(false);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log("Error occurred during Unity services initialization: " + e.Message);
        }
    }

    private async void HandleLobbyHeartbeat()
    {
        if (IsLobbyHost())
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
        if (joinedLobby != null && joinedLobby.Players.Count > 0)
        {
            lobbyPollTimer -= Time.deltaTime;
            if (lobbyPollTimer < 0f)
            {
                float lobbyPollTimerMax = 1.1f;
                lobbyPollTimer = lobbyPollTimerMax;
                joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                if (joinedLobby != null && joinedLobby.Players[0].Id != null)
                {
                    PrintPlayers(joinedLobby);
                }
                else
                {
                    CreateLobbyPanel.SetActive(true);
                    InLobbyPanel.SetActive(false);
                }
            }
        }
    }


    private async void CreateLobby()
    {
        try
        {
            int maxPlayer = (int)lobbyMaxPlayerCount.value;
            CreateLobbyOptions createLobbyOptions = new()
            {
                IsPrivate = isLobbyPrivate,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, "Ace Of Spades") }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer, createLobbyOptions);

            joinedLobby = lobby;

            if (!isLobbyPrivate)
            {

            }
            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);
            InLobbyPanel.SetActive(true);
            CreateLobbyPanel.SetActive(false);
            lobbyPollTimer = 15;
            PrintPlayers(joinedLobby);
            lobyyNameForInLobby.text = joinedLobby.Name;
            lobyyPlayerCountText.text = (lobby.Players.Count + " / " + lobby.MaxPlayers).ToString();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public void OnCreateLobbyButtonClicked()
    {

        if (!string.IsNullOrEmpty(lobbyName) && lobbyName.All(char.IsLetter) && lobbyName.Length > 5)
        {
            CreateLobby();
        }
        else
        {
            if (string.IsNullOrEmpty(lobbyName))
            {
                invalidLobbyNamePrompter.text = "Lobby Name shoulden't be empty!";
                Invoke(nameof(PrompterHider), 4f);
            }
            else if (lobbyName.Length is > 0 and < 5)
            {
                invalidLobbyNamePrompter.text = "Lobby Name Length Should be greater then 5!";
                Invoke(nameof(PrompterHider), 4f);
            }
            else
            {
                invalidLobbyNamePrompter.text = "Spaces aren't allowed!";
                Invoke(nameof(PrompterHider), 4f);
            }
            Debug.Log("Invalid Lobby Name!");
        }
    }

    private async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = CreateQueryLobbiesOptions();
            QueryResponse queryResponse = await QueryLobbiesAsync(queryLobbiesOptions);
            LogLobbies(queryResponse);
        }
        catch (LobbyServiceException e)
        {
            LogException(e);
        }
    }

    private QueryLobbiesOptions CreateQueryLobbiesOptions()
    {
        return new QueryLobbiesOptions
        {
            Count = 25,
            Filters = new List<QueryFilter>
            {
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
            },
            Order = new List<QueryOrder>
            {
                new QueryOrder(false, QueryOrder.FieldOptions.Created)
            }
        };
    }

    private async Task<QueryResponse> QueryLobbiesAsync(QueryLobbiesOptions options)
    {
        return await Lobbies.Instance.QueryLobbiesAsync(options);
    }

    private void LogLobbies(QueryResponse queryResponse)
    {
        Debug.Log("Lobbies found: " + queryResponse.Results.Count);
        foreach (Lobby lobby in queryResponse.Results)
        {
            Debug.Log(lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Data["GameMode"].Value);

            GameObject lobbyCard = Instantiate(lobbyNameCard, lobiesListParentGO.transform);
            lobbyListGO.Add(lobbyCard);
            LobbyNameCard lobbyNameCardComponent = lobbyCard.GetComponent<LobbyNameCard>();
            lobbyNameCardComponent.playerCount = lobby.Players.Count;
            lobbyNameCardComponent.totalPlayerCount = lobby.MaxPlayers;
            lobbyNameCardComponent.lobbyName = lobby.Name;
            lobbyNameCardComponent.SetLobbyName_and_PlayerCount();
            lobbyNameCardComponent.lobbyCardJoinButton.onClick.AddListener(OnJoinLobbyById);

            lobyyNameForInLobby.text = lobby.Name;
            lobyyPlayerCountText.text = (lobby.Players.Count + " / " + lobby.MaxPlayers).ToString();
        }
    }

    private void LogException(LobbyServiceException e)
    {
        Debug.Log(e);
    }

    public void OnGetLobbiesListButtonClicked()
    {
        ListLobbies();
    }

    private async void JoinLobbyById()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            joinedLobby = await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id, new JoinLobbyByIdOptions
            {
                Player = GetPlayer(),
            });
            InLobbyPanel.SetActive(true);
            LobbiesListPanel.SetActive(false);
            Debug.Log("Joined Lobby");
            lobyyNameForInLobby.text = joinedLobby.Name;
            lobyyPlayerCountText.text = (joinedLobby.Players.Count + " / " + joinedLobby.MaxPlayers).ToString();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public void OnJoinLobbyById()
    {
        if (joinedLobby == null)
        {
            JoinLobbyById();
        }
        else
        {
            Debug.Log("You are already in a Lobby name: " + joinedLobby.Name + "'");
        }
    }

    private async void JoinLobbyWithCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new()
            {
                Player = GetPlayer()
            };

            Lobby joinLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            Debug.Log("Joined Lobby with code " + lobbyCode);

            PrintPlayers(joinLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public void OnJoinLobbyWithCodeButtonClicked()
    {
        //JoinLobbyWithCode(InputField.text.ToString());
        //Debug.Log(InputField.text.ToString());
    }

    private async void QuickJoinLobbby()
    {
        try
        {
            await LobbyService.Instance.QuickJoinLobbyAsync();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public void OnQuickJoinLobbyButtonClicked()
    {
        QuickJoinLobbby();
    }

    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Players in Lobby " + lobby.Name + " GameMode: " + lobby.Data["GameMode"].Value);
        lobyyPlayerCountText.text = (lobby.Players.Count + " / " + lobby.MaxPlayers).ToString();
        for (int i = 0; i < lobby.Players.Count; i++)
        {
            Debug.Log(lobby.Players[i].Id + " " + lobby.Players[i].Data["PlayerName"].Value);
            playerNameCard[i].gameObject.SetActive(true);
            playerNameCard[i].playerName = lobby.Players[i].Data["PlayerName"].Value;
            playerNameCard[i].SetPlayerName();
            if (IsLobbyHost())
            {
                playerIdList[i] = lobby.Players[i].Id;
                playerNameCard[i].kickPlayerButton.interactable = true;
            }
            else
            {
                playerNameCard[i].kickPlayerButton.interactable = false;
            }
        }
        for (int i = lobby.Players.Count; i < playerNameCard.Count; i++)
        {
            playerNameCard[i].gameObject.SetActive(false);
        }
    }

    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                {
                    "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)
                }
            }
        };
    }

    public void LobbyPrivacyButton(TextMeshProUGUI lobbyPrivacyText)
    {
        isLobbyPrivate = !isLobbyPrivate;
        lobbyPrivacyText.text = isLobbyPrivate ? "Private Lobby" : "Public Lobby";
    }

    public void OnCheckAvailableLobbiesButtonClicked()
    {
        LobbiesListPanel.SetActive(true);
        CreateLobbyPanel.SetActive(false);
        ListLobbies();
    }

    public void ExitLobbiesListPanel()
    {
        Debug.Log("lobbyListGO.Count" + lobbyListGO.Count);
        lobbyListGO.ForEach(lLGO => Destroy(lLGO));
        lobbyListGO.Clear();
        CreateLobbyPanel.SetActive(true);
        LobbiesListPanel.SetActive(false);
    }

    public void GetPlayerName(string playerNameIn)
    {
        playerName = playerNameIn;
    }

    public void GetLobbyName(string lobbyNameIn)
    {
        lobbyName = lobbyNameIn;
    }

    private void PrompterHider()
    {
        invalidLobbyNamePrompter.text = "";
        invalidNamePrompter.text = "";
    }

    private async void LeaveLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                playerNameCard.ForEach(pNC => pNC.gameObject.SetActive(false));
                if (joinedLobby != null && joinedLobby.Players.Count > 2)
                {
                    PrintPlayers(joinedLobby);
                }
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);


                joinedLobby = null;
                Debug.Log("Lobby Leaved! ");
                CreateLobbyPanel.SetActive(true);
                InLobbyPanel.SetActive(false);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public void OnClickLeaveLobbyButton()
    {
        LeaveLobby();
    }

    private async void KickPlayer(string playerId)
    {
        if (IsLobbyHost())
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }
    public bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    public void OnClickKickPlayer(int playerIdNumber)
    {
        KickPlayer(playerIdList[playerIdNumber]);
    }
}
