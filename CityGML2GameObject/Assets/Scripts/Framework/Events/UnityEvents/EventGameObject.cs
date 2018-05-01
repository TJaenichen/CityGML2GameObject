using System;
using UnityEngine;
using UnityEngine.Events;

namespace Framework.Events.UnityEvents
{
    [Serializable]
    public class EventGameObject: UnityEvent<GameObject>
    {
    }
}
