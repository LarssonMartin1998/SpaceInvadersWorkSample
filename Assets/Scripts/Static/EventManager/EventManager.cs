using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    private Dictionary<AllEventTypes, UnityEvent> eventDictionary;

    private void Awake()
    {
        InitializeThisSingleton();

        eventDictionary = new Dictionary<AllEventTypes, UnityEvent>();
    }

    private void InitializeThisSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        // Means that there already is an instance of this singleton and were trying to create another.
        else if (instance != null)
        {
            Debug.Log("Tried to initialize an already created singleton: \"EventManager\"");
            Destroy(gameObject);
        }
    }

    public void StartListening(AllEventTypes eventType, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (eventDictionary.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            eventDictionary.Add(eventType, thisEvent);
        }
    }

    public void StopListening(AllEventTypes eventType, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (eventDictionary.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public void TriggerEvent(AllEventTypes eventType)
    {
        UnityEvent thisEvent = null;
        if (eventDictionary.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}