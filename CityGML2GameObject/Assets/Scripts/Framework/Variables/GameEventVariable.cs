using Framework.Events;
using UnityEngine;

namespace Framework.Variables
{
    [CreateAssetMenu(menuName = "Variables/GameEvent")]
    public class GameEventVariable : ScriptableObject
    {
        [SerializeField]
        private GameEvent value;

        public GameEvent Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}