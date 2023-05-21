using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;


public class LobbyManager : MonoBehaviour
{
    [SerializeField]
    private GameData gameData;
    [SerializeField]
    private TMP_InputField InputField;
    [SerializeField]
    private TextMeshProUGUI lobbyState;
    private Lobby hostLobby;
    private float heartbeatTimer;
    private string playerName;
    [SerializeField]
    private GameObject lobiesListParentGO;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializeUnityServices();
    }

    public static LobbyManager Instance { get; private set; }

    private async void InitializeUnityServices()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in: " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        playerName = "Sam" + Random.Range(10, 99);
        Debug.Log(playerName);
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
    }

    private async void HandleLobbyHeartbeat()
    {
        if (hostLobby != null)
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    private async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby";
            int maxPlayer = 4;
            CreateLobbyOptions createLobbyOptions = new()
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, "Ace Of Spades") }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer, createLobbyOptions);

            hostLobby = lobby;

            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);

            PrintPlayers(hostLobby);

            lobbyState.text = "Joined";
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public void OnCreateLobbyButtonClicked()
    {
        CreateLobby();
    }

    private async void ListLobbiesOld()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new()
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

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Data["GameMode"].Value);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
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

    private async void JoinLobby()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            _ = await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);

            Debug.Log("Joined Lobby");
            lobbyState.text = "Joined";
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public void OnJoinLobbyButtonClicked()
    {
        JoinLobby();
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

            lobbyState.text = "Joined";
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public void OnJoinLobbyWithCodeButtonClicked()
    {
        JoinLobbyWithCode(InputField.text.ToString());
        Debug.Log(InputField.text.ToString());
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
        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
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
}
