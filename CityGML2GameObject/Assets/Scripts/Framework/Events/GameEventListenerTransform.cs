using Framework.Events.UnityEvents;
using UnityEngine;
using UnityEngine.Events;

namespace Framework.Events
{
    public class GameEventListenerTransform : GameEventListenerBase
    {
        public GameEventTransform Event;
        public EventTransform Response;

        private void OnEnable()
        {
            if (Event != null)
            {
                Event.RegisterListener(this);
            }
        }

        private void OnDisable()
        {
            if (Event != null)
            {
                Event.UnregisterListener(this);
            }
        }

        public void OnEventRaised(Transform value)
        {
            LastFired = Time.time;
            SetTriggered();
            OnEvent(value);
            Response.Invoke(value);
        }

        public override GameEventBase GetEvent()
        {
            return Event;
        }

        public override UnityEventBase GetResponse()
        {
            return Response;
        }
    }
}