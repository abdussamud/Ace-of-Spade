using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerNumber;
    public List<Card> hand;
    public List<int> handIntList;

    private void Start()
    {
        GameController.gc.players.Add(this);
    }
}
