using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isOccupide;
    public int number;
    public TextMeshPro playerName;
    public Card currentCard;

    public void SetName(string name)
    {
        playerName.text = name;
    }
}
