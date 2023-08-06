using System.Collections.Generic;
using UnityEngine;
// game mechanics
public class GameController : MonoBehaviour
{
    public static GameController gc;
    public List<PlayerController> players;

    private void Awake()
    {
        gc = this;
        int i = 0;
        foreach (PlayerController player in players)
        {
            player.playerNumber = i;
            i++;
        }
    }
}
