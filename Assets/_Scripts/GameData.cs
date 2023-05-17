using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Game Data Object", order = 2, fileName = "Game Data Object")]
public class GameData : ScriptableObject
{
    public string objectName;
    public int objectNumber;
    public int numberLoad;
    public bool isLoaded;
    public float speedPoint;
    public float speedValue;
    public List<int> gameList = new();
}
