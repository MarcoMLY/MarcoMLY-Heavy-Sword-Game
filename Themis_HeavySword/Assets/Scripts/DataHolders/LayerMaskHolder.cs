using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Data
{
    [CreateAssetMenu(menuName = "ScriptableObject/DataHolders/LayerMask")]
    [System.Serializable]
    public class LayerMaskHolder : ScriptableObject, IHoldData<LayerMask>
    {
        [field: SerializeField] public LayerMask Variable { get; protected set; }

        public void ChangeData(LayerMask newVariable)
        {
            Variable = newVariable;
        }
    }
}
