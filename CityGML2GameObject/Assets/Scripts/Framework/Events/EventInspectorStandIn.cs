using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Events
{
    public class EventInspectorStandIn : MonoBehaviour
    {
        public List<EventDescription> Events = new List<EventDescription>();
        public Action OnEvent;
        public EventDescription LastEventDescription;
        public void OnEventListener(EventDescription eventDescription)
        {
            Events.Add(eventDescription);
            LastEventDescription = eventDescription;
            if (OnEvent != null)
            {
                OnEvent.Invoke();
            }
        }
    }
}
