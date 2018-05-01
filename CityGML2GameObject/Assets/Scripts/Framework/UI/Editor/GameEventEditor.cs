using Framework.Events;
using UnityEditor;
using UnityEngine;

namespace Assets.Plugins.Framework.UI.Editor
{
    [CustomEditor(typeof(GameEvent))]
    public class GameEventEditor : GameEventEditorBase
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUI.enabled = Application.isPlaying;

            var e = target as GameEvent;

            if (GUILayout.Button("Raise"))
            {
                UnityEngine.Debug.Log("Raise");
                e.Raise();
            }
            DrawListeners();
        }
    }
}
 