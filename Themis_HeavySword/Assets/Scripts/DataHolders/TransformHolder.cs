using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Data
{
    [CreateAssetMenu(menuName = "ScriptableObject/DataHolders/Transform")]
    [System.Serializable]
    public class TransformHolder : ScriptableObject, IHoldData<Transform>
    {
        public Transform Variable { get; set; }

        public void ChangeData(Transform newVariable)
        {
            Variable = newVariable;
        }
    }
}
