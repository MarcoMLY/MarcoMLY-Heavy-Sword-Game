using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Data
{
    [CreateAssetMenu(menuName = "ScriptableObject/DataHolders/TransformList")]
    [System.Serializable]
    public class TransformListHolder : IHoldListData
    {
        public List<Transform> Variable { get; private set; } = new List<Transform>();

        public void AddData(Transform newVariable)
        {
            Variable.Add(newVariable);
        }

        public void RemoveData(Transform newVariable)
        {
            Variable.Remove(newVariable);
        }

        public override void ClearData()
        {
            for (int i = Variable.Count - 1; i >= 0; i--)
            {
                Variable.RemoveAt(i);
            }
            Variable.Clear();
        }
    }
}
