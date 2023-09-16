using System.Collections.Generic;
using UnityEngine;
// game mechanics
public class GameController : MonoBehaviour
{
    public static GameController gc;
    public int playerCount;
    public List<CardController> players;

    private void Awake()
    {
        gc = this;
        int i = 0;
        foreach (CardController player in players)
        {
            player.playerNumber = i;
            i++;
        }
    }
}
