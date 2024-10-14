using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : SingletonMono<GameEventManager>
{
    // Dictionary để quản lý các sự kiện với các listener
    [SerializeField] private Dictionary<string, Delegate> eventDictionary;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Đảm bảo GameEventManager không bị hủy khi chuyển scene
        eventDictionary = new Dictionary<string, Delegate>();
    }

    // Đăng ký một listener cho một sự kiện
    public void StartListening(TyperEvent eventName, Action listener)
    {
        if (eventDictionary.TryGetValue(eventName.ToString(), out Delegate thisEvent))
        {
            thisEvent = (Action)thisEvent + listener;
            eventDictionary[eventName.ToString()] = thisEvent;
        }
        else
        {
            eventDictionary.Add(eventName.ToString(), listener);
        }
    }

    public void StartListening<T>(TyperEvent eventName, Action<T> listener)
    {
        if (eventDictionary.TryGetValue(eventName.ToString(), out Delegate thisEvent))
        {
            thisEvent = (Action<T>)thisEvent + listener;
            eventDictionary[eventName.ToString()] = thisEvent;
        }
        else
        {
            eventDictionary.Add(eventName.ToString(), listener);
        }
    }

    // Gỡ bỏ một listener khỏi một sự kiện
    public void StopListening(TyperEvent eventName, Action listener)
    {
        if (eventDictionary.TryGetValue(eventName.ToString(), out Delegate thisEvent))
        {
            thisEvent = (Action)thisEvent - listener;
            eventDictionary[eventName.ToString()] = thisEvent;
        }
    }

    public void StopListening<T>(TyperEvent eventName, Action<T> listener)
    {
        if (eventDictionary.TryGetValue(eventName.ToString(), out Delegate thisEvent))
        {
            thisEvent = (Action<T>)thisEvent - listener;
            eventDictionary[eventName.ToString()] = thisEvent;
        }
    }

    // Kích hoạt một sự kiện
    public void TriggerEvent(TyperEvent eventName)
    {
        if (eventDictionary.TryGetValue(eventName.ToString(), out Delegate thisEvent))
        {
            (thisEvent as Action)?.Invoke();
        }
    }

    public void TriggerEvent<T>(TyperEvent eventName, T param)
    {
        if (eventDictionary.TryGetValue(eventName.ToString(), out Delegate thisEvent))
        {
            (thisEvent as Action<T>)?.Invoke(param);
        }
    }
}

public enum TyperEvent
{
    none,
    OnUpdateLevelShipOnGame,
    OnUpdateShooting,
    OnMovingEnemies,
}
