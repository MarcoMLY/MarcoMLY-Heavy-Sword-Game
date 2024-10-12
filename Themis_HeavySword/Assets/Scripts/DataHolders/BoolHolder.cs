using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Data
{
    [CreateAssetMenu(menuName = "ScriptableObject/DataHolders/Bool")]
    [System.Serializable]
    public class BoolHolder : ScriptableObject, IHoldData<bool>
    {
        [field: SerializeField] public bool Variable { get; protected set; }

        public void ChangeData(bool newVariable)
        {
            Variable = newVariable;
        }
    }
}
