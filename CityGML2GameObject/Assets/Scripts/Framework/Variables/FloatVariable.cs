using UnityEngine;

namespace Framework.Variables
{
    [CreateAssetMenu(menuName = "Variables/Float")]
    public class FloatVariable : VariableBase<float>
    {
        public void ApplyChange(float amount)
        {
            Value += amount;
        }

        public void ApplyChange(FloatVariable amount)
        {
            Value += amount.Value;
        }
    }
}