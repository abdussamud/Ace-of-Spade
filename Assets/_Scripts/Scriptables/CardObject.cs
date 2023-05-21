using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Card Object", order = 2, fileName = "Card Object")]
public class CardObject : ScriptableObject
{
    public int cardNumber;
    public string cardName;
    public string suit;
    public string value;
    public GameObject card;
}
