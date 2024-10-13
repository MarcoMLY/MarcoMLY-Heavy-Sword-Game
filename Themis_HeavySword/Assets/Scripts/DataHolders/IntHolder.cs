using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Data
{
    [CreateAssetMenu(menuName = "ScriptableObject/DataHolders/Int")]
    [System.Serializable]
    public class IntHolder : ScriptableObject, IHoldData<int>
    {
        [field: SerializeField] public int Variable { get; protected set; }

        public void ChangeData(int newVariable)
        {
            Variable = newVariable;
        }

        public void AddAmount(int newVariable)
        {
            Variable += newVariable;
        }
    }
}
