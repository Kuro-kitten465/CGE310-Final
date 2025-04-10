using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kuro.Utilities.DesignPattern
{
    public static class EventBus
    {
        private static readonly Dictionary<string, Action> _events = new();

        public static void Subscribe(string key, Action action)
        {
            if (!_events.ContainsKey(key))
            {
                _events.Add(key, action);
            }
        }

        public static void Unsubscribe(string key)
        {
            if (_events.ContainsKey(key))
            {
                _events.Remove(key);
            }
        }

        public static void Publish(string key)
        {
            if (_events.TryGetValue(key, out Action action))
            {
                action?.Invoke();
            }
        }
    }
}
