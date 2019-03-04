using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Event containing game object as sender and a reciever
[System.Serializable]
public class GameEvent : UnityEvent<GameObject,GameObject>
{
}

// Singleton event manager for scripts based on events to inherit from
public class EventManager : MonoBehaviour {
    /*
    -------------------------------------------------------------------
                                PUBLIC FIELDS
    -------------------------------------------------------------------
    */


    /*
    -------------------------------------------------------------------
                                PRIVATE FIELDS
    -------------------------------------------------------------------
    */
    Dictionary<string, GameEvent> m_event_dictionary;  // The map of events to listen for
    static EventManager m_emanager;                     // Event manager instance

    // Singleton instance
    public static EventManager instance
    {
        get
        {
            // If current instance doesn't exist, find it 
            if (!m_emanager)
            {
                m_emanager = FindObjectOfType(typeof(EventManager)) as EventManager;

                // If found init
                if (m_emanager)
                    m_emanager.Init();
                else
                    Debug.LogError("No active Event Manager attached to an object in the scene!");
            }

            return m_emanager;
        }
    }

    // Instantiate dictionary
    void Init()
    {
        if (m_event_dictionary == null)
            m_event_dictionary = new Dictionary<string, GameEvent>();
    }

    // Register events onto the dictionary
    public static void Register(string eventName, UnityAction<GameObject,GameObject> listener)
    {
        GameEvent u_event = null; 
        // See if event is in dictionary
        if (instance.m_event_dictionary.TryGetValue(eventName, out u_event))
        {
            // Add the listener callback if found
            u_event.AddListener(listener);
        }
        // If event is not in dictionary
        else
        {
            // Create a new unity event and add the new event to the dictionary
            u_event = new GameEvent();
            u_event.AddListener(listener);
            instance.m_event_dictionary.Add(eventName, u_event);
        }
    }

    // Unregister events from the dictionary
    public static void Unregister(string eventName, UnityAction<GameObject,GameObject> listener)
    {
        if (m_emanager == null)
            return;

        GameEvent u_event = null;
        // remove the listener if value found
        if (instance.m_event_dictionary.TryGetValue(eventName, out u_event))
            u_event.RemoveListener(listener);
    }

    // Trigger an event - reciever is optional
    public static void TriggerEvent(string eventName, GameObject sender, GameObject reciever = null)
    {
        GameEvent u_event = null;
        // Invoke the event if it exists
        if (instance.m_event_dictionary.TryGetValue(eventName, out u_event))
            u_event.Invoke(sender, reciever);
    }
}
