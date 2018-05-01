using System;
using UnityEngine;
using UnityEngine.Events;

namespace Framework.Events.UnityEvents
{
    [Serializable]
    public class EventTransform: UnityEvent<Transform>
    {
    }
}
