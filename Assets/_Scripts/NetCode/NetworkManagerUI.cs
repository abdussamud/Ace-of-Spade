using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    public GameObject SetProfilePanel;
    public GameObject NetworkJoinPanel;
    public GameObject InNetworkPanel;
    public GameObject[] NetworkPanel;
    public GameObject playerDPParentGO;
    public Image playerDPImage;
    public GameObject dpGO;
    public static GameObject playerDPParent;
    public static Color playerDPColor;
    public static GameObject dp;


    private void Start()
    {
        dp = dpGO;
        playerDPParent = playerDPParentGO;
        ActivatePanel(SetProfilePanel.name);
    }

    public void OnClickServerButton()
    {
        NetworkManager.Singleton.StartServer();
        ActivatePanel(InNetworkPanel.name);
    }

    public void OnClickHostButton()
    {
        NetworkManager.Singleton.StartHost();
        ActivatePanel(InNetworkPanel.name);
    }

    public void OnClickClientButton()
    {
        NetworkManager.Singleton.StartClient();
        ActivatePanel(InNetworkPanel.name);
    }

    private void ActivatePanel(string panelName)
    {
        NetworkPanel.ToList().ForEach(p => p.SetActive(p.name == panelName));
    }

    public void OnClickGreenButton1()
    {
        playerDPColor = Color.green;
        playerDPImage.color = playerDPColor;
    }

    public void OnClickRedButton2()
    {
        playerDPColor = Color.red;
        playerDPImage.color = playerDPColor;
    }

    public void OnClickYellowButton3()
    {
        playerDPColor = Color.yellow;
        playerDPImage.color = playerDPColor;
    }

    public void OnClickBlueButton4()
    {
        playerDPColor = Color.blue;
        playerDPImage.color = playerDPColor;
    }

    public void OnClickCyanButton5()
    {
        playerDPColor = Color.cyan;
        playerDPImage.color = playerDPColor;
    }

    public void OnClickWhiteButton6()
    {
        playerDPColor = Color.white;
        playerDPImage.color = playerDPColor;
    }

    public void OnClickProfileDone()
    {
        ActivatePanel(NetworkJoinPanel.name);
    }
}
