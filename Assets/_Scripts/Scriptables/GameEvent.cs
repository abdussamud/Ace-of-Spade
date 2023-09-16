using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Event", menuName = "Scriptable Object/Game Event", order = 2)]
public class GameEvent : ScriptableObject
{
    public int sentInt;
    public bool sentBool;
    public float sentFloat;
    public string sentString;
    private readonly List<EventListener> listeners = new();


    public void AddListener(EventListener listener) => listeners.Add(listener);

    public void RemoveListener(EventListener listener) => listeners.Remove(listener);

    public void TriggerEvent() { for (int i = listeners.Count - 1; i >= 0; i--) { listeners[i].OnEventRaised(this); } }
}
