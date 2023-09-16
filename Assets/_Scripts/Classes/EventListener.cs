using System.Collections.Generic;
using UnityEngine;

public class EventListener : MonoBehaviour
{
    public List<EventAndResponse> eventAndResponses = new();

    private void OnEnable()
    {
        if (eventAndResponses.Count >= 1) { eventAndResponses.ForEach(e => e.gameEvent.AddListener(this)); }
    }

    private void OnDisable()
    {
        if (eventAndResponses.Count >= 1) { eventAndResponses.ForEach(e => e.gameEvent.RemoveListener(this)); }
    }

    public void OnEventRaised(GameEvent passedEvent)
    {
        for (int i = eventAndResponses.Count - 1; i >= 0; i--)
        {
            if (passedEvent == eventAndResponses[i].gameEvent) { eventAndResponses[i].EventRaised(); }
        }
    }
}
