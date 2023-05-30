using Unity.Netcode;
using UnityEngine;


public class PlayerNetwork : NetworkBehaviour
{
    private NetworkVariable<int> m_RandomNumber = new(1, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    private NetworkVariable<SpriteRenderer> m_ProfileColor = new(null, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);


    private void Awake()
    {
        transform.GetComponent<SpriteRenderer>().color = NetworkManagerUI.playerDPColor;
        m_ProfileColor.CanClientRead(OwnerClientId);
    }

    private void Update()
    {
        if (IsOwner) { Move(); }
    }

    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position += Vector3.up;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.position += Vector3.down;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position += Vector3.left;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += Vector3.right;
        }
    }

    public override void OnNetworkSpawn()
    {
        m_RandomNumber.OnValueChanged += (int previousValue, int newValue) =>
        {
            Debug.Log(OwnerClientId + ": randomNumber: " + m_RandomNumber.Value);
        };
    }

}
