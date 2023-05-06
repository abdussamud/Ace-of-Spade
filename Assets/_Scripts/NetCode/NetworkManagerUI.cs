using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;


    private void Awake()
    {
        serverButton.onClick.AddListener(() => { _ = NetworkManager.Singleton.StartServer(); });
        hostButton.onClick.AddListener(() => { _ = NetworkManager.Singleton.StartHost(); });
        clientButton.onClick.AddListener(() => { _ = NetworkManager.Singleton.StartClient(); });
    }
}
