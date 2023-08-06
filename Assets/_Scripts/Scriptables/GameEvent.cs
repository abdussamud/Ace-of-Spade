using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable Objects/Game Event", fileName = "Game Event", order = 3)]
public class GameEvent : ScriptableObject
{
    private List<EventListener> listeners = new();

    public void TriggerEvent()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventTriggered();
        }
    }

    public void AddListener(EventListener listener) => listeners.Add(listener);

    public void RemoveListener(EventListener listener) => listeners.Remove(listener);
}
