using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Data
{
    [CreateAssetMenu(menuName = "ScriptableObject/DataHolders/Float")]
    [System.Serializable]
    public class FloatHolder : ScriptableObject, IHoldData<float>
    {
        [field: SerializeField] public float Variable { get; protected set; }

        public void ChangeData(float newVariable)
        {
            Variable = newVariable;
        }
    }
}
