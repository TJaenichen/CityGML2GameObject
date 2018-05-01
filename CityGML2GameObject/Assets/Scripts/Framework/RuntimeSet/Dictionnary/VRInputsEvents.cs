using Framework.Events;
using UnityEngine;

namespace Framework.RuntimeSet
{
    [CreateAssetMenu(menuName = "RuntimeSet/Dictionnary/VRInputsEvents")]
    public class VRInputsEvents : RuntimeDictionnary<string, GameEventBase>
    {
    }
}