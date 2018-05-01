using System;
using UnityEngine.Events;

namespace Framework.Events.UnityEvents
{
    [Serializable]
    public class EventBase<T> : UnityEvent<T>
    {
    }
}
