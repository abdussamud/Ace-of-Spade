using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;


public class TestLobby : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField InputField;
    [SerializeField]
    private TextMeshProUGUI lobbyState;
    private Lobby hostLobby;
    private float heartbeatTimer;
    public int test;
    private string playerName;
    public TextMeshProUGUI randomNumberTest;
    public GameData gameData;
    public TextMeshProUGUI[] testText;


    private void Awake()
    {
        Invoke(nameof(FuncDelay), 1f);
    }

    private void FuncDelay()
    {
        testText[0].text = gameData.gameList[0].ToString();
        testText[1].text = gameData.gameList[1].ToString();
        testText[2].text = gameData.gameList[2].ToString();
        testText[3].text = gameData.isLoaded.ToString();
        testText[4].text = gameData.isLoaded.ToString();
        testText[5].text = gameData.numberLoad.ToString();
        testText[6].text = gameData.gameList.Count.ToString();
        testText[7].text = gameData.speedPoint.ToString();
        testText[8].text = gameData.speedValue.ToString();
        LocalFileStorage._go.SaveData();
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in: " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        playerName = "Sam" + UnityEngine.Random.Range(10, 99);
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
                Player = GetPlayer()
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

    private async void ListLobbies()
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
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
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
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions()
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
        Debug.Log("Players in Lobby " + lobby.Name);
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

    public void ButtonTester(int number)
    {
        if (number == 0)
        {
            gameData.gameList[0] = Random.Range(100, 1000);
            FuncDelay();
        }
        else if (number == 1)
        {
            gameData.gameList[1] = Random.Range(0, 8);
            FuncDelay();
        }
        else if (number == 2)
        {
            gameData.gameList[2] = Random.Range(9, 90);
            FuncDelay();
        }
        else if (number == 3)
        {
            gameData.isLoaded = true;
            FuncDelay();
        }
        else if (number == 4)
        {
            gameData.isLoaded = false;
            FuncDelay();
        }
        else if (number == 5)
        {
            gameData.numberLoad  = Random.Range(9000, 900000);
            FuncDelay();
        }
        else if (number == 6)
        {
            gameData.numberLoad = Random.Range(100032, 1233433);
            gameData.speedValue = Random.Range(1f, 900f);
            FuncDelay();
        }
        else if (number == 7)
        {
            gameData.numberLoad = 1324;
            gameData.speedPoint = Random.Range(10001f, 10005f);
            FuncDelay();
        }
        else if (number == 8)
        {
            gameData.numberLoad = 325242;
            gameData.speedValue = Random.Range(10009f, 10010f); ;
            FuncDelay();
        }
    }
}
