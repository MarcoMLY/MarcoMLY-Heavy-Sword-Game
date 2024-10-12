using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Data
{
    [CreateAssetMenu(menuName = "ScriptableObject/DataHolders/FloatConstant")]
    [System.Serializable]
    public class FloatConstantHolder : ScriptableObject, IHoldData<float>
    {
        [field: SerializeField] public float Variable { get; protected set; }

        public void ChangeData(float FunctionUnusable) { }
    }
}
