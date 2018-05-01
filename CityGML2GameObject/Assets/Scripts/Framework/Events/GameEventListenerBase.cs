using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

namespace Framework.Events
{
    /// <summary>
    /// WIP!
    /// </summary>
    public class GameEventListenerBase : MonoBehaviour
    {
        protected float LastFired = float.NegativeInfinity;
        private string _triggeredByObject;
        private string _triggeredByMethod;
        protected int FrameNum = 3;
        private EventInspectorStandIn _gameEventObject;

        public virtual GameEventBase GetEvent()
        {
            return null;
        }

        public virtual UnityEventBase GetResponse()
        {
            return null;
        }

        void SetEventSystem()
        {
            if (_gameEventObject == null)
            {
                _gameEventObject = FindObjectOfType<EventInspectorStandIn>();
            }
        }

        EventDescription GetEventDescription(object parameter)
        {
            SetEventSystem();
            
            return new EventDescription
            {
                Time = Time.time,
                Event = GetEvent(),
                Response = GetResponse(),
                Parameter = parameter,
                CallerObject = _triggeredByObject,
                CallerMethod = _triggeredByMethod
            };
        }

        protected void OnEvent(object value)
        {
            if (GetEvent().name == "OnEvent")
            {
                return;
            }
            SetEventSystem();
            _gameEventObject.OnEventListener(GetEventDescription(value));
        }

        protected void SetTriggered()
        {
            var mb = new StackTrace().GetFrame(FrameNum).GetMethod();
            _triggeredByMethod = mb.Name;

            if (mb.DeclaringType != null)
            {
                _triggeredByObject = mb.DeclaringType.ToString();
            }
            else
            {
                UnityEngine.Debug.LogWarning("No type " + mb);
            }
        }

        //void OnDrawGizmos()
        //{
        //    if (!Application.isPlaying)
        //    {
        //        return;
        //    }

        //    if (Time.time - LastFired > 5)
        //    {
        //        return;
        //    }
        //    DrawGizmos();
        //}

        //void DrawGizmos()
        //{
        //    var curEvent = GetEvent();
        //    var curResponse = GetResponse();

        //    if (curEvent == null || curResponse == null)
        //    {
        //        return;
        //    }
        //    var writtenTo = new List<Vector3>();

        //    foreach (var curListener in curEvent.GetListeners())
        //    {
        //        var target = curListener as GameEventListenerBase;

        //        if (target != null)
        //        {
        //            for (int i = 0; i < curResponse.GetPersistentEventCount(); i++)
        //            {
        //                var curMethod = curResponse.GetPersistentMethodName(i);
        //                var curTarget = curResponse.GetPersistentTarget(i).name;
        //                var text = string.Format("{0}: {1}.{2} -> {3}.{4}",
        //                    curEvent.name,
        //                    _triggeredByObject,
        //                    _triggeredByMethod,
        //                    curTarget,
        //                    curMethod);

        //                var style = new GUIStyle();
        //                style.stretchWidth = true;
        //                style.clipping = TextClipping.Overflow;
        //                var curPos = (target.transform.position + transform.position) / 2;
        //                while (writtenTo.Contains(curPos))
        //                {
        //                    curPos -= new Vector3(0, 1, 0);
        //                }
        //                writtenTo.Add(curPos);

        //                Handles.Label(curPos, text, style);
        //                Gizmos.DrawCube(target.transform.position, Vector3.one);
        //                Gizmos.DrawCube(transform.position, Vector3.one);
        //                Gizmos.DrawLine(target.transform.position, transform.position);
        //            }

        //        }
        //        else
        //        {
        //            UnityEngine.Debug.LogWarning(GetType().Name);
        //        }
        //    }
        //}


        //void OnDrawGizmosSelected()
        //{
        //    if (!Application.isPlaying)
        //    {
        //        return;
        //    }

        //    DrawGizmos();
        //}
    }
}
