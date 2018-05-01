using UnityEngine;

namespace Framework.Variables
{
    [CreateAssetMenu(menuName = "Variables/Int")]
    public class IntVariable : VariableBase<int>
    {
        public void ApplyChange(int amount)
        {
            Value += amount;
        }

        public void ApplyChange(IntVariable amount)
        {
            Value += amount.Value;
        }
    }
}