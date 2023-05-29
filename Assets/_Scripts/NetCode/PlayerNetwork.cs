using Unity.Netcode;
using UnityEngine;


public class PlayerNetwork : NetworkBehaviour
{
    private NetworkVariable<int> m_RandomNumber = new(1, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    private NetworkVariable<SpriteRenderer> m_ProfileColor = new(null, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);


    private void Start()
    {
        GetComponent<SpriteRenderer>().color = NetworkManagerUI.playerDPColor;
    }

    public override void OnNetworkSpawn()
    {
        m_RandomNumber.OnValueChanged += (int previousValue, int newValue) =>
        {
            Debug.Log(OwnerClientId + ": randomNumber: " + m_RandomNumber.Value);
        };
    }

}
