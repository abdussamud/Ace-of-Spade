using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private NetworkVariable<int> m_RandomNumber = new(1, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);


    public override void OnNetworkSpawn()
    {
        m_RandomNumber.OnValueChanged += (int previousValue, int newValue) =>
        {
            Debug.Log(OwnerClientId + ": randomNumber: " + m_RandomNumber.Value);
        };
    }

    private void Update()
    {
        if (IsOwner)
        {
            if (Input.GetKeyUp(KeyCode.R))
            {
                m_RandomNumber.Value = Random.Range(0, 100);
            }

            Vector3 moveDirection = new();

            if (Input.GetKey(KeyCode.W)) moveDirection.z = +1f;
            if (Input.GetKey(KeyCode.S)) moveDirection.z = -1f;
            if (Input.GetKey(KeyCode.D)) moveDirection.x = +1f;
            if (Input.GetKey(KeyCode.A)) moveDirection.x = -1f;

            float moveSpeed = 3f;
            transform.position += moveSpeed * Time.deltaTime * moveDirection;
        }
    }
}

/*
 using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new();

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Move();
        }
    }

    public void Move()
    {
        
    }

    [ServerRpc]
    private void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        Position.Value = GetRandomPositionOnPlane();
    }

    private static Vector3 GetRandomPositionOnPlane()
    {
        Vector3 moveDirection = new();


        if (Input.GetKey(KeyCode.W)) moveDirection.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDirection.z = -1f;
        if (Input.GetKey(KeyCode.D)) moveDirection.x = +1f;
        if (Input.GetKey(KeyCode.A)) moveDirection.x = -1f;

        float moveSpeed = 3f;
        return moveSpeed * Time.deltaTime * moveDirection;
    }

    private void Update()
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            SubmitPositionRequestServerRpc();
        }
        if (IsOwner && NetworkManager.Singleton.IsServer)
        {
            Vector3 moveDirection = new();


            if (Input.GetKey(KeyCode.W)) moveDirection.z = +1f;
            if (Input.GetKey(KeyCode.S)) moveDirection.z = -1f;
            if (Input.GetKey(KeyCode.D)) moveDirection.x = +1f;
            if (Input.GetKey(KeyCode.A)) moveDirection.x = -1f;

            float moveSpeed = 3f;
            transform.position += moveSpeed * Time.deltaTime * moveDirection;
        }
    }
}
*/
