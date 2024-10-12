using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace UnityEngine.Data
{
    [CreateAssetMenu(menuName = "ScriptableObject/DataHolders/Vector3")]
    [System.Serializable]
    public class VectorHolder : ScriptableObject, IHoldData<Vector3>
    {
        public Vector3 Variable { get; protected set; }

        public void ChangeData(Vector3 newVariable)
        {
            Variable = newVariable;
        }
    }
}
